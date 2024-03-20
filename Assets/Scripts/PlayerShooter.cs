using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Object = System.Object;

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
            Cursor.visible = false;
        }

        private void Update()
        {
            if (CanFireGun()) _fireRate.DecreaseTimer(Time.deltaTime);
            if (_fireRate.IsCooldown()) _fireRate.DecreaseTimer(Time.deltaTime);
        }

        private void Reload(InputAction.CallbackContext obj)
        {
            _ammunition = gunOrigin.ammunition;
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            if (_ammunition <= 0) return;
            if (_fireRate.IsCooldown()) return;
            //Somehow instantiating with quaternion.euler deforms the rotation.
            //Below is the post-rotation setting, which works.
            //I suspect, that Quaternion.Euler rotates a bit and it alters the rotation.
            var shot = Instantiate(bullet, transform.position, quaternion.identity);
            shot.transform.localEulerAngles = new Vector3(0, 0, GetCrosshairRotation()); 
            _fireRate.ResetTimer();
            _ammunition--;
        }

        public Vector2 GetPositionVector2() => transform.position;

        private bool CanFireGun() => _fireRate.IsCooldown() is false && _ammunition > 0;
        public Vector3 GetMousePosition() => Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        public Vector2 GetMousePositionVector2() => GetMousePosition();

        public float GetCrosshairRotation() =>
            Vector2.SignedAngle(
                ObjectSpinner.DirectionVector(
                    GetPositionVector2(),
                    GetPositionVector2() + Vector2.up),
                ObjectSpinner.DirectionVector(
                    GetPositionVector2(),
                    GetMousePositionVector2()));

        public int GetAmmunition() => _ammunition;
    }
}