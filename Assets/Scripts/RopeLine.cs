using System;
using UnityEngine;

public class RopeLine : MonoBehaviour
{
    [SerializeField] private float cableWidth = 0.2f;
    [SerializeField] private Color color;
    
    private Rope2DCreator rope;
    private LineRenderer line;

    private void Awake()
    {
        rope = GetComponent<Rope2DCreator>();
        line = GetComponent<LineRenderer>();

        line.enabled = true;
        line.positionCount = rope.segments.Length;
        line.startWidth = line.endWidth = cableWidth;
        line.startColor = color;
        line.endColor = color;
    }
    
    void Update()
    {
        for (var i = 0; i < rope.segments.Length; i++)
        {
            line.SetPosition(i, rope.segments[i].position);
        }
    }
}
