using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPoses;
    public Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;
    
    void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }
    
    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
        segmentPoses[0] = targetDir.position;
        for (var i = 1; i < segmentPoses.Length; i++)
        {
            var target = segmentPoses[i - 1] + targetDir.right * targetDist;
            var damp = Vector3.SmoothDamp(segmentPoses[i], target, ref segmentV[i], smoothSpeed + i / trailSpeed);
            segmentPoses[i] = damp;
        }
        
        lineRenderer.SetPositions(segmentPoses);
    }
}
