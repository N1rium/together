using System;
using UnityEngine;

namespace Rooms
{
    public class RoomConfiner : MonoBehaviour
    {
        public Action<PlayerCamera> OnEnter, OnExit;
    }
}
