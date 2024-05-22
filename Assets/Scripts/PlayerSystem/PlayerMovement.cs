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
        public InputAction move;
        private Vector2 _input;
        public float movementSpeed;
        private Rigidbody2D _playerPhysicsRigidbody;
        private Animator _animation;
        private IngameManager _ingameManager;

        private void OnEnable()
        {
            move.Enable();
        }

        private void OnDisable()
        {
            move.Disable();
        }

        void Start()
        {
            _playerPhysicsRigidbody = GetComponent<Rigidbody2D>();
            _animation = GetComponentInChildren<Animator>();
            _ingameManager = GameObject.Find(TagManager.Instance.IngameManagerTag).GetComponent<IngameManager>();
            
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
            FetchInput();
            _playerPhysicsRigidbody.velocity = _input * (movementSpeed * (1 / Time.deltaTime) * Time.deltaTime);
            _animation.SetFloat("horizontal", _input.x);
            _animation.SetFloat("vertical", _input.y);
        }


        private void FetchInput()
        {
            _input = move.ReadValue<Vector2>();
        }

        private void OnTriggerEnter2D(Collider2D collidingObject)
        {
            const float positionOffset = 5f;
            if (collidingObject.CompareTag(TagManager.Instance.CorpseTag))
            {
                
                var corpse = collidingObject.gameObject;
                Destroy(corpse);
                _ingameManager.CompleteBounty();
            }
            if (_ingameManager.isBountyInProgress) return;
            if (IsTriggerEnter() is false) return;
            if (IsFallacyTrigger())
            {
                _ingameManager.ChangePlayerActiveState();
                _ingameManager.GetNewBounty();
                _ingameManager.ChangeCursorVisibility();
                return;
            }
            

            if (IsExitTrigger())
            {
                SceneManager.LoadScene(1);
                return;
            }

            PlayerPrefs.SetString("pos", PositionSaveParser(transform.position));
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
            }

            return;

            bool IsTriggerEnter()
                => collidingObject.gameObject.name.ToLower().Contains("trigger");

            string PositionSaveParser(Vector3 position)
                => $"{position.x}|{position.y - positionOffset}|{position.z}";

            bool IsFallacyTrigger()
                => collidingObject.gameObject.name.ToLower().Equals("billboardtrigger");

            bool IsExitTrigger()
                => collidingObject.gameObject.name.ToLower().Equals("exittrigger");
        }
    }
}