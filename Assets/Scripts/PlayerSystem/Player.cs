using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour, IEntity
    {
        private int _health = 3;
        public int Money { get; private set; }

        public delegate void PlayerEvent();

        public static event PlayerEvent PlayerDeath;

        private void Start()
        {
            Money = PlayerPrefs.GetInt("Money");
        }

        private void Update()
        {
            if (!IsAlive())
                PlayerDeath?.Invoke();
        }

        //IENTITY
        public void Heal(int addition) => _health += addition;
        public int GetMaxHealth => 3;

        public void Damage(int damage) => _health -= damage;

        public void IncrementMoney(int value) => Money += value;
        public void DecrementMoney(int value) => Money -= value;


        // functionality
        public int GetHealth() => _health;

        public bool IsAlive() => GetHealth() > 0;
        //
    }
}