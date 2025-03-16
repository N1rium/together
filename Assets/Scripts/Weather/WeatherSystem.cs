using System;
using System.Collections.Generic;
using Rooms;
using UnityEngine;

namespace Weather
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private List<Weathers> _weathers = new();

        private void Start()
        {
            // TODO - Handle this autonomous from a GameManager or such
            var rooms = FindObjectsByType<Room>(FindObjectsSortMode.None);
            foreach (var room in rooms)
            {
                room.OnEnter += OnRoomEnter;
            }
        }

        // TODO - Handle this autonomous from a GameManager or such
        private void OnRoomEnter(Room room)
        {
            Process(room.GetWeathers());
        }

        public void Process(List<WeatherCondition> conditions)
        {
            foreach (var weather in _weathers)
            {
                var active = conditions.Find(c => c.weatherType == weather.weatherType) != null;
                weather.weatherGameObject.SetActive(active);
            }
        }
    }

    [Serializable]
    public class Weathers
    {
        public WeatherType weatherType;
        public GameObject weatherGameObject;
    }
}
