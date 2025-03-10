using DG.Tweening;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private ParticleSystem subPs;

    private ParticleSystem.EmissionModule _emission;
    
    void Start()
    {
        _emission = ps.emission;

        Delay.For(1f).OnComplete(() => Spawn(transform.position, Color.cyan));
        Delay.For(2f).OnComplete(() => Spawn(transform.position, Color.yellow));
    }

    public void Spawn(Vector3 worldPos, Color color)
    {
        ps.transform.position = worldPos;
        var main = ps.main;
        main.startColor = color;

        var subMain = subPs.main;
        subMain.startColor = color;
        
        _emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(0f, 10, 1, 0.010f)
        });
        
        ps.Play();
    }
}
