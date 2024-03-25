using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour, IEntity
    {
        private float _health = 3;

        public  delegate void PlayerEvent();

        public static event PlayerEvent PlayerDeath;

        private void Update()
        {
            if (!IsAlive())
                PlayerDeath?.Invoke();
        }

        //IENTITY
        public void Heal(int addition)
        {
            _health += addition;
        }

        public void Damage(int damage)
        {
            _health -= damage;
        }
        
        // functionality
        public float GetHealth() => _health;

        public bool IsAlive() => GetHealth() > 0;
        //
    }
}