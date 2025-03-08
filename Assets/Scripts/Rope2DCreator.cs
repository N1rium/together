using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Rope2DCreator : MonoBehaviour
{
    [SerializeField, Range(2, 50)] private int numSegments = 2;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private Sprite plugSprite;

    [SerializeField] private Transform testPlayer;
    [SerializeField] private PowerUnit powerUnit;
    private DistanceJoint2D _plug;
    
    public Transform pointA;
    public Transform pointB;

    [HideInInspector] public Transform[] segments;

    private Vector2 GetSegmentPosition(int segmentIndex)
    {
        var posA = pointA.position;
        var posB = pointB.position;
        var fraction = 1f / numSegments;

        return Vector2.Lerp(posA, posB, fraction * segmentIndex);
    }

    [Button]
    void GenerateRope()
    {
        segments = new Transform[numSegments];

        for (var i = 0; i < numSegments; i++)
        {
            var currJoint = new GameObject("JointNode");
            currJoint.transform.SetParent(transform);
            currJoint.transform.position = GetSegmentPosition(i);
            var rb = currJoint.gameObject.AddComponent<Rigidbody2D>();
            var dj = currJoint.AddComponent<DistanceJoint2D>();

            rb.mass = 5f;
            rb.linearDamping = 2f;
            rb.freezeRotation = true;

            dj.autoConfigureDistance = false;
            dj.maxDistanceOnly = true;
            
            segments[i] = currJoint.transform;

            if (i == 0)
            {
                dj.connectedAnchor = Vector2.zero;
                dj.distance = 0.1f;
                dj.connectedBody = GetComponent<Rigidbody2D>();
                continue;
            }

            dj.distance = Vector3.Distance(segments[i].transform.position, segments[i - 1].transform.position);

            var prevIndex = i - 1;
            dj.connectedBody = segments[prevIndex].GetComponent<Rigidbody2D>();

            if (i != numSegments - 1) continue;
            var sr = currJoint.gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = plugSprite;
            sr.flipY = true;
        }
    }

    [Button]
    void DeleteSegments()
    {
        if (transform.childCount > 0)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        segments = null;
    }

    public float GetDistance()
    {
        var res = 0f;

        for (var i = 0; i < segments.Length; i++)
        {
            var curr = segments[i];
            var next = segments[i - 1];
            res += Vector3.Distance(curr.position, next.position);
        }
        
        return res;
    }

    private void Start()
    {
        var djs = GetComponentsInChildren<DistanceJoint2D>();
        _plug = djs[^1];
        /*Delay.For(2f).OnComplete(() =>
        {
            _plug.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            _plug.transform.SetParent(testPlayer);
            _plug.transform.localPosition = Vector2.zero;

            Delay.For(4f).OnComplete(() =>
            {
                _plug.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                _plug.transform.SetParent(powerUnit.transform);
                _plug.transform.localPosition = Vector2.zero;
                powerUnit.Connect();
            });
        });*/
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < numSegments; i++)
        {
            var posAtIndex = GetSegmentPosition(i);
            Gizmos.DrawSphere(posAtIndex, 0.1f);
        }
    }
}
