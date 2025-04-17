using UnityEngine;

namespace Fountain
{
    [System.Serializable]
    public class FountainData
    {
        public RoomId room;
        public Transform transform;
        public Vector3 position;
        
        public FountainData(RoomId room, Vector3 position)
        {
            this.room = room;
            this.position = position;
        }
    }
}