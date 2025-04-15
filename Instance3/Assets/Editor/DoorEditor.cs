#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class DoorInfo
{
    public string doorName;
    public Vector3 position;
}

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    // Liste des informations récupérées sur les portes de la scène cible
    private List<DoorInfo> retrievedDoorInfos = new List<DoorInfo>();
    // Permet de vérifier si le RoomId a changé depuis la dernière récupération
    private RoomId lastScannedRoom = (RoomId)(-1);
    // Index actuellement sélectionné dans le popup
    private int selectedIndex = -1;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Afficher le champ nextRoom dans l'inspecteur
        SerializedProperty nextRoomProp = serializedObject.FindProperty("nextRoom");
        EditorGUILayout.PropertyField(nextRoomProp);

        Door doorComponent = (Door)target;
        RoomId currentRoom = doorComponent.NextRoom;

        // Si la salle a changé ou qu'aucun scan n'a encore été fait, recharge la liste.
        if (retrievedDoorInfos == null || currentRoom != lastScannedRoom)
        {
            retrievedDoorInfos = RetrieveDoorInfosForRoom(currentRoom);
            lastScannedRoom = currentRoom;
            selectedIndex = -1;
        }

        // Bouton pour recharger manuellement la liste.
        if (GUILayout.Button("Recharger la liste pour " + currentRoom.ToString()))
        {
            retrievedDoorInfos = RetrieveDoorInfosForRoom(currentRoom);
            selectedIndex = -1;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Portes trouvées dans la scène " + currentRoom + " :", EditorStyles.boldLabel);

        // Construire un tableau de chaînes pour le popup (affichage du nom de la porte)
        string[] doorNames = retrievedDoorInfos.Select(info => info.doorName + " @ " + info.position.ToString("F2")).ToArray();

        if (retrievedDoorInfos.Count > 0)
        {
            SerializedProperty selectedDoorNameProp = serializedObject.FindProperty("selectedDoorName");
            SerializedProperty targetPositionProp = serializedObject.FindProperty("targetPosition");
            
            // Pré-sélection : si aucune sélection n'est faite et qu'une valeur est déjà présente
            if (selectedIndex == -1 && !string.IsNullOrEmpty(selectedDoorNameProp.stringValue))
            {
                for (int i = 0; i < doorNames.Length; i++)
                {
                    if (doorNames[i].StartsWith(selectedDoorNameProp.stringValue))
                    {
                        selectedIndex = i;
                        break;
                    }
                }
            }

            // Afficher le popup pour sélectionner la porte cible
            int newSelectedIndex = EditorGUILayout.Popup("Porte cible", selectedIndex, doorNames);
            if (newSelectedIndex != selectedIndex)
            {
                selectedIndex = newSelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < retrievedDoorInfos.Count)
                {
                    // Mise à jour du champ selectedDoorName
                    selectedDoorNameProp.stringValue = retrievedDoorInfos[selectedIndex].doorName;
                    
                    // Utilisation de l'accesseur TargetPosition dans le script Door
                    Door door = (Door)target;
                    door.TargetPosition = retrievedDoorInfos[selectedIndex].position;
                    
                    // Synchronisation avec la SerializedProperty pour conserver les modifications dans l'inspecteur
                    targetPositionProp.vector3Value = door.TargetPosition;
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Aucune porte trouvée ou scène introuvable.");
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    /// <summary>
    /// Charge temporairement la scène correspondant au RoomId donné, récupère les informations
    /// (nom et position) des objets Door présents dans cette scène, puis ferme la scène.
    /// </summary>
    private List<DoorInfo> RetrieveDoorInfosForRoom(RoomId roomId)
    {
        List<DoorInfo> doorInfos = new List<DoorInfo>();
        string sceneName = roomId.ToString();

        // Recherche de la scène par son nom (assurez-vous que le nom correspond à roomId.ToString())
        string[] guids = AssetDatabase.FindAssets("t:Scene " + sceneName);
        if (guids == null || guids.Length == 0)
        {
            Debug.LogError("Aucune scène trouvée avec le nom '" + sceneName + "'.");
            return doorInfos;
        }
        string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);

        // Ouvrir la scène en mode additif (temporaire)
        Scene openedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
        
        // Recherche de tous les objets Door dans la scène ouverte
        Door[] doors = Resources.FindObjectsOfTypeAll<Door>();
        foreach (Door d in doors)
        {
            // Vérifier que l'objet se trouve bien dans la scène temporairement chargée
            if (d.gameObject.scene.name == openedScene.name)
            {
                DoorInfo info = new DoorInfo
                {
                    doorName = d.gameObject.name,
                    position = d.transform.position
                };
                doorInfos.Add(info);
            }
        }

        // Fermer la scène temporaire pour éviter de surcharger l'éditeur
        EditorSceneManager.CloseScene(openedScene, true);

        return doorInfos;
    }
}
#endif
