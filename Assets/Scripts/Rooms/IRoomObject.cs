namespace Rooms
{
    public interface IRoomObject
    {
        public void OnRoomEnter(RoomData data);
        public void OnRoomExit(RoomData data);
    }
}
