using System;
using DG.Tweening;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public Ease ease;

    private Tween _rotationTween;
    private void OnEnable()
    {
        _rotationTween = transform
            .DORotate(Vector3.forward * 360f, rotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(ease);
    }

    private void OnDisable()
    {
        _rotationTween.Kill();
    }
}
