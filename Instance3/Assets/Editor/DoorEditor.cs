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

        // Affiche le champ nextRoom normalement dans l'inspecteur.
        SerializedProperty nextRoomProp = serializedObject.FindProperty("nextRoom");
        EditorGUILayout.PropertyField(nextRoomProp);

        // Récupérer le composant Door et la salle cible sélectionnée.
        Door doorComponent = (Door)target;
        RoomId currentRoom = doorComponent.NextRoom;

        // Si la salle a changé ou aucun scan n'a encore été effectué, rechargez la liste.
        if (retrievedDoorInfos == null || currentRoom != lastScannedRoom)
        {
            retrievedDoorInfos = RetrieveDoorInfosForRoom(currentRoom);
            lastScannedRoom = currentRoom;
            selectedIndex = -1;
        }

        // Bouton pour forcer une nouvelle récupération de la liste.
        if (GUILayout.Button("Recharger la liste pour " + currentRoom.ToString()))
        {
            retrievedDoorInfos = RetrieveDoorInfosForRoom(currentRoom);
            selectedIndex = -1;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Portes trouvées dans la scène " + currentRoom + " :", EditorStyles.boldLabel);

        // Confectionne un tableau de chaînes pour le popup.
        string[] doorNames = retrievedDoorInfos.Select(info => info.doorName + " @ " + info.position.ToString("F2")).ToArray();

        if (retrievedDoorInfos.Count > 0)
        {
            // Récupère les propriétés sérialisées pour selectedDoorName et targetPosition.
            SerializedProperty selectedDoorNameProp = serializedObject.FindProperty("selectedDoorName");
            SerializedProperty targetPositionProp = serializedObject.FindProperty("targetPosition");

            // Si aucune sélection n'est faite, essayer de pré-sélectionner si une valeur est déjà stockée.
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

            // Affiche un popup pour sélectionner la porte cible.
            int newSelectedIndex = EditorGUILayout.Popup("Porte cible", selectedIndex, doorNames);
            if (newSelectedIndex != selectedIndex)
            {
                selectedIndex = newSelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < retrievedDoorInfos.Count)
                {
                    // Met à jour le champ selectedDoorName et targetPosition selon la porte choisie.
                    selectedDoorNameProp.stringValue = retrievedDoorInfos[selectedIndex].doorName;
                    targetPositionProp.vector3Value = retrievedDoorInfos[selectedIndex].position;
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Aucune porte trouvée ou scène introuvable.");
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Charge temporairement la scène correspondant au RoomId donné, récupère les informations
    /// (nom et position) des objets Door présents dans cette scène, puis ferme la scène.
    /// </summary>
    private List<DoorInfo> RetrieveDoorInfosForRoom(RoomId roomId)
    {
        List<DoorInfo> doorInfos = new List<DoorInfo>();
        string sceneName = roomId.ToString();

        // Recherche de la scène en se basant sur son nom (Assurez-vous que celui-ci correspond à roomId.ToString())
        string[] guids = AssetDatabase.FindAssets("t:Scene " + sceneName);
        if (guids == null || guids.Length == 0)
        {
            Debug.LogError("Aucune scène trouvée avec le nom '" + sceneName + "'.");
            return doorInfos;
        }
        string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);

        // Ouvrir la scène en mode additif (temporaire)
        Scene openedScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

        // Recherche des objets Door dans la scène ouverte (y compris les objets inactifs)
        Door[] doors = Resources.FindObjectsOfTypeAll<Door>();

        foreach (Door d in doors)
        {
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

        // Fermer la scène temporaire pour ne pas encombrer l'éditeur
        EditorSceneManager.CloseScene(openedScene, true);

        return doorInfos;
    }
}
#endif
