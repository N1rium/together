using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TarodevController;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private ParticleSystem subPs;

    private ParticleSystem.EmissionModule _emission;
    private List<PlayerAnimator> _players = new();

    private void OnDisable()
    {
        ClearListeners();
    }

    void Start()
    {
        _emission = ps.emission;
        _players = FindObjectsByType<PlayerAnimator>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        RegisterListeners();
    }

    private void RegisterListeners()
    {
        foreach (var p in _players)
        {
            p.OnDeath += Spawn;
        }
    }

    private void ClearListeners()
    {
        foreach (var p in _players)
        {
            p.OnDeath -= Spawn;
        }
    }

    private void Spawn(Vector3 worldPos, Color color)
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
