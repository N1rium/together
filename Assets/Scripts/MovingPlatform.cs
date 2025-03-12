using System;
using Unity.Netcode;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }

    public void Update()
    {
        if (NetworkManager.Singleton == null) return;
        // Move up and down by 5 meters and change direction every 3 seconds.
        var positionY = Mathf.PingPong(NetworkManager.Singleton.LocalTime.TimeAsFloat / 3f, 1f) * 5f;
        transform.position = startPos + new Vector3(0, positionY, 0);
    }
}
