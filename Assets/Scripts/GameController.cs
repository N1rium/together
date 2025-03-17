using Rooms;
using UnityEngine;
using Weather;

public class GameController : MonoBehaviour
{
    [SerializeField] private WeatherSystem weatherSystem;

    void Start()
    {
        Application.targetFrameRate = 120;
        
        // TODO - Load active room and its neighbours
        var rooms = FindObjectsByType<Room>(FindObjectsSortMode.None);
        foreach (var room in rooms)
        {
            room.OnEnter += OnRoomEnter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRoomEnter(Room room)
    {
        weatherSystem.Process(room.GetWeathers());
    }
}
