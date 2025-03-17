using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weather
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField] private List<Weathers> _weathers = new();

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
