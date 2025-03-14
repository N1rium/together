using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed;
    private Vector2 direction;
    
    private void Update()
    {
        direction = target.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
