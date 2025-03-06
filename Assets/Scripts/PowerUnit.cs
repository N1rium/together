using UnityEngine;
using UnityEngine.Events;

// Used with Rope2DCreator (Power cable) to trigger electricity-based events
public class PowerUnit : MonoBehaviour
{
    
    public UnityEvent<bool> onActiveChanged;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Connect()
    {
        onActiveChanged?.Invoke(true);
    }
}
