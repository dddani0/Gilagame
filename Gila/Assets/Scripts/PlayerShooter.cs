using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class PlayerShooter : MonoBehaviour
    {
        [FormerlySerializedAs("originObject")] public Gun gunOrigin;
        private string _gunName;
        private int _ammunition;

        private Timer _fireRate;

        //
        public InputAction shoot;
        public InputAction reload;
        public InputAction aim;
        public Transform cursor;
        public GameObject bullet;

        private void OnEnable()
        {
            shoot.Enable();
            reload.Enable();
            aim.Enable();
        }

        private void OnDisable()
        {
            shoot.Disable();
            reload.Enable();
            aim.Enable();
        }

        private void Start()
        {
            _ammunition = gunOrigin.ammunition;
            _gunName = gunOrigin.name;
            _fireRate = new Timer(gunOrigin.fireRate);
            shoot.performed += Shoot;
            reload.performed += Reload;
        }

        private void Update()
        {
            if (CanFireGun()) _fireRate.DecreaseTimer(Time.deltaTime);
            if (_fireRate.IsCooldown()) _fireRate.DecreaseTimer(Time.deltaTime);
            if (IsMouseMoving()) //works but wonky. Will need to fix later.
                ObjectSpinner.SpinObject(transform, cursor, GetMousePositionVector2());
        }

        private void Reload(InputAction.CallbackContext obj)
        {
            _ammunition = gunOrigin.ammunition;
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            if (_ammunition <= 0) return;
            if (_fireRate.IsCooldown()) return;
            Instantiate(bullet, cursor.position, cursor.rotation);
            _fireRate.ResetTimer();
            _ammunition--;
        }

        private bool CanFireGun() => _fireRate.IsCooldown() is false && _ammunition > 0;
        private bool IsMouseMoving() => Mouse.current.delta.ReadValue() != Vector2.zero;
        private Vector3 GetMousePosition() => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        private Vector2 GetMousePositionVector2() => GetMousePosition();
        public int GetAmmunition() => _ammunition;
    }
}