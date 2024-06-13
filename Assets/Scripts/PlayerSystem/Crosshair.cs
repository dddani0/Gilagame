using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class Crosshair : MonoBehaviour
    {
        private PlayerShooter _playerShooter;
        private GameObject _weaponDisplay;
        private bool _isActive = true;

        private void Start()
        {
            _playerShooter = GameObject.FindGameObjectWithTag(TagManager.Instance.PlayerTag)
                .GetComponent<PlayerShooter>();
            _weaponDisplay = GameObject.FindGameObjectWithTag(TagManager.Instance.WeaponDisplayTag);
            Player.PlayerDeath += () =>
            {
                _isActive = false;
            };
        }

        private void Update()
        {
            AimCursor();
            transform.position = _playerShooter.GetMousePositionVector2();
        }

        private void AimCursor()
        {
            switch (_isActive)
            {
                case true:
                    _weaponDisplay.transform.LookAt(GetPosition());
                    break;
                case false:
                    transform.LookAt(-_weaponDisplay.transform.right);
                    break;
            }
        }

        private Vector2 GetPosition() => transform.position;

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }
    }
}