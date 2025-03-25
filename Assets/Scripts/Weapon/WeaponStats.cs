using UnityEngine;

namespace Weapon
{
    public class WeaponStats : ScriptableObject
    {
        public string Name = "Weapon Name";
        public string Description = "Weapon Description";
        public float Cooldown = 0.5f;
        public float BaseDamage = 1f;
        public AudioClip Sound;
    }
}
