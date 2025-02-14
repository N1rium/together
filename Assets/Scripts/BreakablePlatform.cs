using DG.Tweening;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float timeToBreak = 0.25f;
    public float timeToRevive = 2.5f;

    [SerializeField] private Collider2D collider;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("COLLISION");
        Delay.For(timeToBreak).OnComplete(() =>
        {
            collider.enabled = false;
            Delay.For(timeToRevive).OnComplete(() => collider.enabled = true);
        });
    }
}
