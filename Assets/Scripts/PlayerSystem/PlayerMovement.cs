using System;
using System.Security.Cryptography;
using DefaultNamespace;
using ManagerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerShooter _playerShooter;
        public InputAction move;
        public InputAction openBillboard;
        private Vector2 _input;
        public float movementSpeed;
        private Rigidbody2D _playerPhysicsRigidbody;
        private Animator _animation;
        private IngameManager _ingameManager;
        private Collider2D _collidingObject;

        private void OnEnable()
        {
            move.Enable();
            openBillboard.Enable();
        }

        private void OnDisable()
        {
            move.Disable();
            openBillboard.Disable();
        }

        void Start()
        {
            _playerPhysicsRigidbody = GetComponent<Rigidbody2D>();
            _animation = GetComponentInChildren<Animator>();
            _ingameManager = GameObject.Find(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>();
            _playerShooter = GetComponent<PlayerShooter>();
            _animation.SetBool("isAlive", true);
            if (SceneManager.GetActiveScene().name.Equals("EchowaveTown") is false) return;
            //only set a new position, when the player actually enters the hub area.
            var rawPosition = PlayerPrefs.GetString("pos");
            transform.position = new Vector3(
                float.Parse(rawPosition.Split("|")[0]),
                float.Parse(rawPosition.Split("|")[1]),
                float.Parse(rawPosition.Split("|")[2])
            );
        }

        void Update()
        {
            if (_ingameManager.IsActive is false)
            {
                _playerPhysicsRigidbody.velocity = Vector2.zero;
                return;
            }

            EnterBillboard();

            FetchInput();
            _playerPhysicsRigidbody.velocity = _input * (movementSpeed * (1 / Time.deltaTime) * Time.deltaTime);
            _animation.SetFloat("horizontal", _input.x);
            _animation.SetFloat("vertical", _input.y);
        }


        private void FetchInput()
        {
            _input = move.ReadValue<Vector2>();
        }

        private void EnterBillboard()
        {
            if (_collidingObject == null)
            {
                _playerShooter.DisableButtonPrompter();
                return;
            }

            if (!_playerShooter.buttonPrompter.IsActive() || !openBillboard.WasPressedThisFrame() ||
                SceneManager.GetActiveScene().name.ToLower().Equals("echowavetown") is false || _collidingObject.tag.Equals(TagManager.Instance.SignTrigger)) return;
            _ingameManager.ChangePlayerActiveState();
            _ingameManager.GetNewBounty();
            _ingameManager.EnableCursorVisibility();
        }

        private void OnTriggerEnter2D(Collider2D collidingObject)
        {
            _collidingObject = collidingObject;
            const float positionOffset = 5f;
            if (collidingObject.CompareTag(TagManager.Instance.CorpseTag))
            {
                var corpse = collidingObject.gameObject;
                Destroy(corpse);
                _ingameManager.CompleteBounty();
            }

            if (IsBillboardTrigger())
            {
                _playerShooter.ShowButtonPrompter(openBillboard);
            }

            if (_ingameManager.isBountyInProgress) return;
            if (IsTriggerEnter() is false) return;


            if (IsExitTrigger())
            {
                SceneManager.LoadScene(1);
                return;
            }

            PlayerPrefs.SetString(TagManager.Instance.PlayerPositionTag, PositionSaveParser(transform.position));
            switch (collidingObject.gameObject.name.ToLower())
            {
                case "saloontrigger":
                    SceneManager.LoadScene(2);
                    break;
                case "sherifftrigger":
                    SceneManager.LoadScene(4);
                    break;
                case "gunsmithtrigger":
                    SceneManager.LoadScene(3);
                    break;
                case "churchtrigger":
                    SceneManager.LoadScene(5);
                    break;
            }

            return;

            bool IsTriggerEnter()
                => collidingObject.gameObject.name.ToLower().Contains("trigger");

            string PositionSaveParser(Vector3 position)
                => $"{position.x}|{position.y - positionOffset}|{position.z}";

            bool IsBillboardTrigger()
                => collidingObject.gameObject.name.ToLower().Contains("billboardtrigger");

            bool IsExitTrigger()
                => collidingObject.gameObject.name.ToLower().Equals("exittrigger");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _collidingObject = null;
        }

        public Animator GetAnimator() => _animation;
        public Rigidbody2D GetRigidbody() => _playerPhysicsRigidbody;
    }
}