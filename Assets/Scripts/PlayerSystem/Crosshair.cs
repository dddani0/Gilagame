using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class Crosshair : MonoBehaviour
    {
        private PlayerShooter _playerShooter;
        private GameObject _weaponDisplay;

        private void Start()
        {
            _playerShooter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooter>();
            _weaponDisplay = GameObject.FindGameObjectWithTag("DisplayWeapon");
        }

        private void Update()
        {
            transform.position = _playerShooter.GetMousePositionVector2();
            _weaponDisplay.transform.LookAt(GetPosition());
        }

        private Vector2 GetPosition() => transform.position;
    }
}