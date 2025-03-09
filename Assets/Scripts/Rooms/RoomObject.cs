using UnityEngine;

namespace Rooms
{
    public class RoomObject : MonoBehaviour, IRoomObject
    {
        public bool enableWithRoom = true;
        
        public void OnRoomEnter(RoomData data)
        {
            if (!enableWithRoom) return;
            gameObject.SetActive(true);
        }

        public void OnRoomExit(RoomData data)
        {
            if (!enableWithRoom) return;
            gameObject.SetActive(false);
        }
    }
}