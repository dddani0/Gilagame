using ManagerSystem;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class PlayerShooter : MonoBehaviour
    {
        public Gun gunOrigin;
        private string _gunName;
        private int _ammunition;

        private IngameManager _ingameManager;

        private Timer _fireRate;

        //
        public InputAction shoot;
        public InputAction reload;
        public InputAction aim;
        public GameObject bullet;

        public TMPro.TextMeshPro buttonPrompter;

        private void OnEnable()
        {
            shoot.Enable();
            reload.Enable();
            aim.Enable();
        }

        private void OnDisable()
        {
            shoot.Disable();
            reload.Disable();
            aim.Disable();
        }

        private void Start()
        {
            _ammunition = gunOrigin.ammunition;
            _gunName = gunOrigin.name;
            _fireRate = new Timer(gunOrigin.fireRate);
            shoot.performed += Shoot;
            reload.performed += Reload;
            _ingameManager = GameObject.Find(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>();
            _ingameManager.DisableCursorVisibility();
            buttonPrompter = transform.GetChild(1).GetComponent<TextMeshPro>();
            buttonPrompter.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_ingameManager.IsActive is false) return;
            if (CanFireGun()) _fireRate.DecreaseTimer(Time.deltaTime);
            if (_fireRate.IsCooldown()) _fireRate.DecreaseTimer(Time.deltaTime);
        }

        private void Reload(InputAction.CallbackContext obj)
        {
            _ammunition = gunOrigin.ammunition;
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            if (_ingameManager.IsActive is false) return;
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

        public void ShowButtonPrompter(InputAction key)
        {
            EnableButtonPrompter();
            buttonPrompter.text = $"Press '{key.GetBindingDisplayString()}'";
        }

        private void EnableButtonPrompter() => buttonPrompter.gameObject.SetActive(true);
        public void DisableButtonPrompter() => buttonPrompter.gameObject.SetActive(false);

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

        public TextMeshPro getPrompter => buttonPrompter;
    }
}