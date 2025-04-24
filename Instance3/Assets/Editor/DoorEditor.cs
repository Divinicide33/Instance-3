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

[CustomEditor(typeof(Door), true)]
public class DoorEditor : Editor
{
    private List<DoorInfo> retrievedDoorInfos = new List<DoorInfo>();
    private RoomId lastScannedRoom = (RoomId)(-1);
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

        #region Buttons
        // Bouton pour recharger manuellement la liste.
        EditorGUILayout.Space(20);
        if (GUILayout.Button("Recharger la liste pour " + currentRoom.ToString()))
        {
            retrievedDoorInfos = RetrieveDoorInfosForRoom(currentRoom);
            selectedIndex = -1;
        }
        
        EditorGUILayout.Space(20);
        if (GUILayout.Button("Recharger la position de la RoomCible"))
        {
            ReloadTarget();
        }
        #endregion
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Portes trouvées dans la scène " + currentRoom + " :", EditorStyles.boldLabel);
        
        string[] doorNames = retrievedDoorInfos.Select(
            info => info.doorName + " @ " + info.position.ToString("F2")).ToArray(); // Construire un tableau de chaînes pour le popup (affichage du nom de la porte)

        if (retrievedDoorInfos.Count > 0)
        {
            SerializedProperty selectedDoorNameProp = serializedObject.FindProperty("selectedDoorName");
            SerializedProperty targetPositionProp = serializedObject.FindProperty("targetPosition");

            #region Pre Selection
            // si aucune sélection n'est faite et qu'une valeur est déjà présente
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
            #endregion

            #region PopUp
            int newSelectedIndex = EditorGUILayout.Popup("Porte cible", selectedIndex, doorNames);
            if (newSelectedIndex != selectedIndex)
            {
                selectedIndex = newSelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < retrievedDoorInfos.Count)
                {
                    selectedDoorNameProp.stringValue = retrievedDoorInfos[selectedIndex].doorName; // Mise à jour du champ selectedDoorName
                    
                    Door door = (Door)target;
                    door.TargetPosition = retrievedDoorInfos[selectedIndex].position; // Utilisation de l'accesseur TargetPosition dans le script Door
                    
                    targetPositionProp.vector3Value = door.TargetPosition; // Synchronisation avec la SerializedProperty pour conserver les modifications dans l'inspecteur
                }
            }
            #endregion
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
                    position = d.transform.GetChild(0).position
                };
                doorInfos.Add(info);
            }
        }

        // Fermer la scène temporaire pour éviter de surcharger l'éditeur
        EditorSceneManager.CloseScene(openedScene, true);

        return doorInfos;
    }
    
    /// <summary>
    /// Recharge la target en recherchant la porte correspondant au nom sauvegardé et en mettant à jour sa position.
    /// </summary>
    private void ReloadTarget()
    {
        serializedObject.Update();

        Door doorComponent = (Door)target;
        RoomId currentRoom = doorComponent.NextRoom;

        // Récupérer la liste des infos de portes dans la scène associée à currentRoom
        List<DoorInfo> doorInfos = RetrieveDoorInfosForRoom(currentRoom);

        // Récupérer les propriétés sérialisées pour le nom et la position de la porte sélectionnée
        SerializedProperty selectedDoorNameProp = serializedObject.FindProperty("selectedDoorName");
        SerializedProperty targetPositionProp = serializedObject.FindProperty("targetPosition");

        // Si une porte a déjà été sélectionnée, on tente de la retrouver dans la liste chargée
        if (!string.IsNullOrEmpty(selectedDoorNameProp.stringValue))
        {
            DoorInfo match = doorInfos.FirstOrDefault(info => info.doorName.Equals(selectedDoorNameProp.stringValue));

            if (match != null)
            {
                doorComponent.TargetPosition = match.position;
                targetPositionProp.vector3Value = match.position;
                Debug.Log("Target rechargé : " + match.doorName + " @ " + match.position.ToString("F2"));
            }
            else
            {
                Debug.LogWarning("Aucune porte correspondante trouvée pour le nom '" + selectedDoorNameProp.stringValue + "'.");
            }
        }
        else
        {
            Debug.LogWarning("Aucun nom de porte sélectionné pour recharger la target.");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif