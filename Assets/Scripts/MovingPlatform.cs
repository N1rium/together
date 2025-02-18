using System;
using System.Diagnostics;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class MovingPlatform : NetworkBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float duration = 5f;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            transform.DOMove(endPosition, duration)
                .From(startPosition)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
