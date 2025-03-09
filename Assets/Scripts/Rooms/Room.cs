using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private RoomConfiner _confiner;
        [SerializeField] private float orthoSize = 10f;

        private List<RoomObject> _roomObjects = new();
        private float _orthoSize;

        private void Start()
        {
            _roomObjects = GetComponentsInChildren<RoomObject>(true).ToList();
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
            foreach (var ro in _roomObjects)
            {
                ro.OnRoomExit(new()
                {
                    PlayerCamera = cam,
                    Room = this,
                });
            }
        }
    }

    // Data passed down to all IRoomObjects in each room
    public class RoomData
    {
        public PlayerCamera PlayerCamera;
        public Room Room;
    }
}
