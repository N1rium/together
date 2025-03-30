using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weather;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomConfiner _confiner;
        [SerializeField] private float orthoSize = 10f;
        [SerializeField] private List<WeatherCondition> weathers;

        private List<IRoomObject> _roomObjects = new();
        private float _orthoSize;

        public event Action<Room> OnEnter, OnExit;

        private void Start()
        {
            _roomObjects = GetComponentsInChildren<IRoomObject>(true).ToList();
            _orthoSize = Camera.main.orthographicSize;
        }

        private void OnEnable()
        {
            _confiner.OnEnter += OnConfinerEnter;
            _confiner.OnExit += OnConfinerExit;
        }

        private void OnDisable()
        {
            _confiner.OnEnter -= OnConfinerEnter;
            _confiner.OnExit -= OnConfinerExit;
        }

        private void OnConfinerEnter(PlayerCamera cam)
        {
            cam.GetCamera().Lens.OrthographicSize = orthoSize;
            OnEnter?.Invoke(this);
            foreach (var ro in _roomObjects)
            {
                ro.OnRoomEnter(new()
                {
                    PlayerCamera = cam,
                    Room = this,
                });
            }
        }
    
        private void OnConfinerExit(PlayerCamera cam)
        {
            cam.GetCamera().Lens.OrthographicSize = _orthoSize;
            OnExit?.Invoke(this);
            foreach (var ro in _roomObjects)
            {
                ro.OnRoomExit(new()
                {
                    PlayerCamera = cam,
                    Room = this,
                });
            }
        }

        private void OnValidate()
        {
            if (_confiner != null) return;
            try
            {
                _confiner = GetComponentInChildren<RoomConfiner>();
            }
            catch
            {
                Debug.LogWarning("No confiner for room: " + transform.name);
            }
        }

        public List<WeatherCondition> GetWeathers() => weathers;
    }

    // Data passed down to all IRoomObjects in each room
    public class RoomData
    {
        public PlayerCamera PlayerCamera;
        public Room Room;
    }
}
