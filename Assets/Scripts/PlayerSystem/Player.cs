using System;
using ManagerSystem;
using PlayerSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour, IEntity
    {
        private CanvasManager _canvasManager;
        private PlayerMovement _playerMovement;
        private IngameManager _ingameManager;
        private int _health;
        private Animator _animator;
        public int Money { get; private set; }
        private Timer deathTouchTimer; //KELL EGY ILYEN MÉRT

        public delegate void PlayerEvent();

        public static event PlayerEvent PlayerDeath;

        private void Start()
        {
            _health = PlayerPrefs.GetInt(TagManager.Instance.PlayerHealthSaveTag);
            _canvasManager = GameObject.FindGameObjectWithTag(TagManager.Instance.CanvasManagerTag)
                .GetComponent<CanvasManager>();
            _playerMovement = GetComponent<PlayerMovement>();
            Money = PlayerPrefs.GetInt("Money");
            PlayerDeath += Die;
            _ingameManager = GetComponent<IngameManager>();
            _canvasManager.SetBloodPanelStatus(_health);
            deathTouchTimer = new Timer(0.1f);
            _animator = transform.GetChild(0).GetComponent<Animator>();
            _ingameManager = GameObject.FindGameObjectWithTag(TagManager.Instance.IngameManagerTag)
                .GetComponent<IngameManager>();
        }

        private void Update()
        {
            if (deathTouchTimer.IsCooldown()) deathTouchTimer.DecreaseTimer(Time.deltaTime);
            if (!IsAlive()) PlayerDeath?.Invoke();
        }

        private void Die()
        {
            _animator.SetBool("isAlive", false);
            _playerMovement.GetRigidbody().velocity = Vector2.zero;
            _playerMovement.enabled = false;
            _canvasManager.EnableDeathButtons();
            _ingameManager.EnableCursorVisibility();
            enabled = false;
        }

        //IENTITY
        public void Heal(int addition)
        {
            _health += addition;
            _canvasManager.SetBloodPanelStatus(_health);
            PlayerPrefs.SetInt(TagManager.Instance.PlayerHealthSaveTag, _health);
        }

        public int GetMaxHealth => 3;

        public void Damage(int damage)
        {
            _health -= damage;
            _canvasManager.SetBloodPanelStatus(_health);
            PlayerPrefs.SetInt(TagManager.Instance.PlayerHealthSaveTag, _health);
        }

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
            if (deathTouchTimer.IsCooldown()) return;
            Damage(1);
        }
    }
}