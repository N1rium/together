using UnityEngine;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform target; // The object to track
    public RectTransform indicator; // The UI indicator (arrow/circle)
    public float padding = 50f; // Padding from the screen edge

    private bool _isActive;
    
    private void Start()
    {
        _isActive = target != null && indicator != null;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        // Get the target's position in screen space
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        // Check if the target is off-screen
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width ||
                           screenPos.y <= 0 || screenPos.y >= Screen.height ||
                           screenPos.z < 0;

        if (isOffScreen)
        {
            // Show the indicator
            indicator.gameObject.SetActive(true);

            // Clamp the screen position to the screen edges
            screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
            screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);

            // Set the indicator's position
            indicator.position = screenPos;

            // Rotate the indicator to point toward the target
            Vector3 direction = (target.position - Camera.main.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            indicator.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Hide the indicator if the target is on-screen
            indicator.gameObject.SetActive(false);
        }
    }
}