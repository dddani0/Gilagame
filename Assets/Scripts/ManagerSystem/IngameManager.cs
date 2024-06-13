using System;
using BountySystem;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ManagerSystem
{
    public class IngameManager : MonoBehaviour
    {
        //thanks to: gandamir, from: https://forum.unity.com/threads/would-love-a-bit-of-help-creating-a-name-generator.517108/
        private string[] _first =
        {
            "Ald", "Alf", "Ash", "Barn", "Blan", "Brack", "Brad", "Brain", "Brom", "Bur", "Cas", "Chelm", "Clere",
            "Cook", "Dart", "Dur", "Edg", "Eg", "El", "Elm", "En", "Farn", "Flit", "Hart", "Horn", "Hors", "Hurst",
            "Kings", "Leather", "Maiden",
            "Marl", "Mel", "Nort", "Pem", "Pen", "Prest", "Rock", "Shaft", "Shriv", "Sod", "South", "Staf", "Stain",
            "Stap", "Sud", "Sun", "Walt",
            "Watch", "Wen", "Wet", "Whit", "Win", "Wy", "Wych"
        };

        private string[] _second =
        {
            "Abb", "Bass", "Booth", "Both", "Burr", "Camb", "Camm", "Cann", "Chedd", "Chill", "Chipp", "Cir",
            "Dribb", "Egg", "Ell", "Emm", "End", "Fald", "Full", "Hamm", "Hamp", "Hann", "Kett", "Mill", "Pend", "Redd",
            "Ribb", "Roth", "Sir",
            "Skell", "Sodd", "Sudd", "Sund", "Tipp", "Todd", "Warr", "Wolv", "Worr"
        };

        private string[] _crime =
        {
            "Treason", "Affiliated with the 'Yellow-hats'", "Manslaughter", "loitering", "Murder", "Attempted murder"
        };

        public Player player;
        private PlayerShooter _playerShooter;

        public Enemy[] enemies;
        public Entity[] enemyEntities;
        public Transform[] _spawns;
        private CanvasManager _canvasManager;
        private Bounty _currentBounty;
        public bool isBountyInProgress = false;
        private Crosshair _crosshair;
        private bool _isActive = true;

        private GameObject spawnedEnemy = null;

        //
        public InputAction exitButton;

        private void OnEnable()
        {
            exitButton.Enable();
        }

        private void OnDisable()
        {
            exitButton.Disable();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(TagManager.Instance.PlayerTag).GetComponent<Player>();
            _playerShooter = player.GetComponent<PlayerShooter>();
            _canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
            _crosshair = GameObject.Find(TagManager.Instance.CrosshairTag).GetComponent<Crosshair>();
            if (GameObject.Find("SpawnPositions") != null)
            {
                if ((GameObject.Find("SpawnPositions").GetComponentsInChildren<Transform>()) != null)
                {
                    _spawns = GameObject.Find("SpawnPositions").GetComponentsInChildren<Transform>();
                }
            }

            _crosshair.IsActive = SceneManager.GetActiveScene().name.ToLower().Equals("echowavetown") ||
                                  SceneManager.GetActiveScene().name.ToLower().Equals("howto");
        }

        private void Update()
        {
            if (exitButton.WasPressedThisFrame())
            {
                EnableCursorVisibility();
                SceneManager.LoadScene(0);
            }
        }

        public void GetNewBounty()
        {
            if (isBountyInProgress) return;
            var bounty = new Bounty(GetRandomName(), GetRandomCrime(), GetRandomBountyAmount());
            _currentBounty = bounty;
            _canvasManager.ShowBounty(bounty);
        }

        public void CompleteBounty()
        {
            player.IncrementMoney(_currentBounty.Amount);
            AddMoney(_currentBounty.Amount);
            ChangeBountyStatus();
            spawnedEnemy = null;
            _playerShooter.DisableArrow();
        }

        public void AddMoney(int amount)
            => PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + amount);

        public void SubtractMoney(int amount)
            => PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - amount);

        public void ChangeBountyStatus()
        {
            isBountyInProgress = isBountyInProgress is false;
            _playerShooter.EnableArrow(spawnedEnemy.transform);
        }

        public void EnableCursorVisibility() => Cursor.visible = true;
        public void DisableCursorVisibility() => Cursor.visible = false;

        public void EnablePlayerActiveState() => _isActive = true;
        public void DisablePlayerActiveState() => _isActive = false;

        public void SpawnEnemy()
        {
            var randomEnemy = enemies[(int)RandomNumberGenerator.Instance.Generate(0, enemies.Length - 1)].gameObject;
            var randomPosition = _spawns[(int)RandomNumberGenerator.Instance.Generate(1, _spawns.Length - 1)].position;
            spawnedEnemy = Instantiate(randomEnemy, randomPosition, Quaternion.identity);
            spawnedEnemy.GetComponent<Enemy>()
                .SetEnemy(enemyEntities[(int)RandomNumberGenerator.Instance.Generate(0, enemyEntities.Length - 1)]);
        }

        private string GetRandomName()
        {
            var firstName = _first[(int)RandomNumberGenerator.Instance.Generate(0, _first.Length - 1)];
            var secondName = _second[(int)RandomNumberGenerator.Instance.Generate(0, _first.Length - 1)];
            return $"{firstName} {secondName}";
        }

        private string GetRandomCrime() => _crime[(int)RandomNumberGenerator.Instance.Generate(0, _crime.Length - 1)];

        private int GetRandomBountyAmount() => (int)RandomNumberGenerator.Instance.Generate(50, 100);
        public bool IsActive => _isActive;
    }
}