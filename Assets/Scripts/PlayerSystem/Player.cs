using System;
using ManagerSystem;
using PlayerSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour, IEntity
    {
        private PlayerMovement _playerMovement;
        private int _health = 3;
        public int Money { get; private set; }

        public delegate void PlayerEvent();

        public static event PlayerEvent PlayerDeath;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            Money = PlayerPrefs.GetInt("Money");
            PlayerDeath += Die;
        }

        private void Update()
        {
            if (!IsAlive())
                PlayerDeath?.Invoke();
        }

        private void Die()
        {
            _playerMovement.GetAnimator().SetBool("isAlive", false);
            _playerMovement.GetRigidbody().velocity = Vector2.zero;
            _playerMovement.enabled = false;
            //disable crosshair
            //canvas manager initatiate death menu.
            enabled = false;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other is null) return;
            if (other.CompareTag(TagManager.Instance.BulletTag) is false) return;
            Destroy(other.gameObject);
            Damage(1);
        }
    }
}