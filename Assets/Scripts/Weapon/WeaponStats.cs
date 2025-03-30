using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapon
{
    [CreateAssetMenu]
    public class WeaponStats : ScriptableObject
    {
        public string Name = "Weapon Name";
        public string Description = "Weapon Description";
        public float Cooldown = 0.5f;
        public float BaseDamage = 1f;
        public float Knockback = 1f;
        public AudioData AttackSound;
        public AudioData AttackHitSound;
    }

    [Serializable]
    public class AudioData
    {
        public AudioClip[] Sounds;
        public float PitchRandomFactor = 0f;

        public AudioClip GetSound()
        {
            return Sounds[Random.Range(0, Sounds.Length)];
        }
    }
}