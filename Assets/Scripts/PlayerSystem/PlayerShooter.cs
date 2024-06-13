using System.Collections.Generic;
using System.Linq;
using ManagerSystem;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class PlayerShooter : MonoBehaviour
    {
        public Gun _currentGun;
        private string _gunName;
        private int _ammunition;
        private int _gunIdx;
        private bool _singleGun;
        public List<Gun> _gunInventory;
        private IngameManager _ingameManager;

        private Timer _fireRate;

        //
        public InputAction shoot;
        public InputAction reload;
        public InputAction aim;
        public InputAction changeWeapon;
        public GameObject bullet;

        public TMPro.TextMeshPro buttonPrompter;
        private GameObject _arrow;
        private bool _isArrowEnabled = false;
        private Transform target;
        private Timer weaponChange;

        private void OnEnable()
        {
            shoot.Enable();
            reload.Enable();
            aim.Enable();
            changeWeapon.Enable();
        }

        private void OnDisable()
        {
            shoot.Disable();
            reload.Disable();
            aim.Disable();
            changeWeapon.Disable();
        }

        private void Start()
        {
            var inventoryState = PlayerPrefs.GetString(TagManager.Instance.InventoryStateSaveTag).Split(';');
            var currentGunState = inventoryState[0].Split("##");
            _currentGun = Gun.CreateGun(currentGunState[0], int.Parse(currentGunState[1]),
                int.Parse(currentGunState[2]),
                int.Parse(currentGunState[3]));
            _gunInventory = new List<Gun>();
            //runs 6 times.
            if (inventoryState.Length > 1)
            {
                List<string> gunNames = new List<string>();
                for (int i = 0; i < inventoryState.Length; i++) //because of the extra ';'
                {
                    var currentIterationOfGun = inventoryState[i].Split("##");
                    var __currentGun = Gun.CreateGun(
                        currentIterationOfGun[0].ToString(),
                        int.Parse(currentIterationOfGun[1]),
                        int.Parse(currentIterationOfGun[2]),
                        int.Parse(currentIterationOfGun[3]));

                    if (gunNames.Contains(__currentGun.name) is false)
                    {
                        _gunInventory.Add(__currentGun);
                        gunNames.Add(__currentGun.name);
                    }
                }
            }
            else
            {
                _gunInventory.Add(_currentGun);
            }

            UpdateGun();
            shoot.performed += Shoot;
            reload.performed += Reload;
            _ingameManager = GameObject.Find(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>();
            _ingameManager.DisableCursorVisibility();
            buttonPrompter = transform.GetChild(1).GetComponent<TextMeshPro>();
            buttonPrompter.gameObject.SetActive(false);
            _arrow = transform.GetChild(2).gameObject;
            _ingameManager = GameObject.Find(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>();
            weaponChange = new Timer(0.1f);
        }

        public void EquipNewGun(Gun newGun)
        {
            AddGunToInventory(newGun);
            var gunSave = PlayerPrefs.GetString(TagManager.Instance.InventoryStateSaveTag) + _gunInventory.Aggregate("",
                (current, gon) => current + $";{gon.name}##{gon.ammunition}##{gon.damage}##{gon.fireRate}");
            PlayerPrefs.SetString(TagManager.Instance.InventoryStateSaveTag, gunSave);
        }

        private void ChangeWeapon()
        {
            if (_singleGun is true) return;
            if (changeWeapon.WasPressedThisFrame() is false) return;
            if (weaponChange.IsCooldown() is true) return;
            _gunIdx = IncrementGunIndex();
            _currentGun = _gunInventory[_gunIdx];
            UpdateGun();
            weaponChange.ResetTimer();
        }

        private void Update()
        {
            _arrow.SetActive(_isArrowEnabled);
            _singleGun = _gunInventory.Capacity < 2;
            if (_isArrowEnabled)
            {
                _arrow.transform.localEulerAngles = new Vector3(0, 0, GetArrowTargetRotation());
            }

            ChangeWeapon();

            if (weaponChange.IsCooldown()) weaponChange.DecreaseTimer(Time.deltaTime);
            if (_ingameManager.IsActive is false) return;
            if (CanFireGun()) _fireRate.DecreaseTimer(Time.deltaTime);
            if (_fireRate.IsCooldown()) _fireRate.DecreaseTimer(Time.deltaTime);
        }

        private void UpdateGun()
        {
            _fireRate = new Timer(_currentGun.fireRate);
            _ammunition = _currentGun.ammunition;
        }

        private void AddGunToInventory(Gun newGun) => _gunInventory.Add(newGun);

        private void Reload(InputAction.CallbackContext obj)
        {
            _ammunition = _currentGun.ammunition;
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            if (!SceneManager.GetActiveScene().name.ToLower().Equals("echowavetown") &&
                !SceneManager.GetActiveScene().name.ToLower().Equals("howto")) return;
            if (_ingameManager.IsActive is false) return;
            if (_ammunition <= 0) return;
            if (_fireRate.IsCooldown()) return;
            //Somehow instantiating with quaternion.euler deforms the rotation.
            //Below is the post-rotation setting, which works.
            //I suspect, that Quaternion.Euler rotates a bit and it alters the rotation.
            var shot = Instantiate(bullet, GetBulletPosition(), quaternion.identity);
            shot.transform.localEulerAngles = new Vector3(0, 0, GetCrosshairRotation());
            _fireRate.ResetTimer();
            _ammunition--;
        }

        public void ShowButtonPrompter(InputAction key)
        {
            EnableButtonPrompter();
            buttonPrompter.text = $"Press '{key.GetBindingDisplayString()}'";
        }

        public void EnableArrow(Transform targetTransform)
        {
            _isArrowEnabled = true;
            target = targetTransform;
        }

        public void DisableArrow()
        {
            _isArrowEnabled = false;
            target = transform;
        }

        private void EnableButtonPrompter() => buttonPrompter.gameObject.SetActive(true);
        public void DisableButtonPrompter() => buttonPrompter.gameObject.SetActive(false);

        public Vector2 GetPositionVector2() => transform.position;

        public Vector2 GetBulletPosition() => GetPositionVector2() +
                                              ObjectSpinner.DirectionVector(GetPositionVector2(),
                                                  GetMousePositionVector2()) * 3;

        private bool CanFireGun() => _fireRate.IsCooldown() is false && _ammunition > 0;
        public Vector3 GetMousePosition() => Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        public Vector2 GetMousePositionVector2() => GetMousePosition();


        public int IncrementGunIndex()
        {
            _gunIdx++;
            return _gunIdx == _gunInventory.Count ? 0 : _gunIdx;
        }

        public float GetCrosshairRotation() =>
            Vector2.SignedAngle(
                ObjectSpinner.DirectionVector(
                    GetPositionVector2(),
                    GetPositionVector2() + Vector2.up),
                ObjectSpinner.DirectionVector(
                    GetPositionVector2(),
                    GetMousePositionVector2()));

        private float GetArrowTargetRotation() => Vector2.SignedAngle(ObjectSpinner.DirectionVector(
                GetPositionVector2(),
                GetPositionVector2() + Vector2.up),
            ObjectSpinner.DirectionVector(GetPositionVector2(), (Vector2)target.position));

        public int GetAmmunition() => _ammunition;
        public Gun GetGun() => _currentGun;
    }
}