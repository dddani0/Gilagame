using BountySystem;
using DefaultNamespace;
using UnityEngine;
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

        public Enemy[] enemies;
        private Transform[] _spawns;
        private CanvasManager _canvasManager;
        private Bounty _currentBounty;
        public bool isBountyInProgress = false;
        private Crosshair _crosshair;
        private bool _isActive = true;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(TagManager.Instance.PlayerTag).GetComponent<Player>();
            _canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
            _crosshair = GameObject.Find(TagManager.Instance.CrosshairTag).GetComponent<Crosshair>();
            if (GameObject.Find("SpawnPositions") != null)
            {
                if ((GameObject.Find("SpawnPositions").GetComponentsInChildren<Transform>()) != null)
                {
                    _spawns = GameObject.Find("SpawnPositions").GetComponentsInChildren<Transform>();
                }
            }
            _crosshair.IsActive = SceneManager.GetActiveScene().name.Equals("EchoWaveTown");
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
        }

        public void AddMoney(int amount)
            => PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + amount);

        public void SubtractMoney(int amount)
            => PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - amount);

        public void ChangeBountyStatus() => isBountyInProgress = isBountyInProgress is false;

        public void ChangeCursorVisibility() => Cursor.visible = Cursor.visible is false;

        public void ChangePlayerActiveState() => _isActive = _isActive is false;

        public void SpawnEnemy()
        {
            var randomEnemy = enemies[(int)RandomNumberGenerator.Instance.Generate(0, enemies.Length - 1)].gameObject;
            var randomPosition = _spawns[(int)RandomNumberGenerator.Instance.Generate(1, _spawns.Length)].position;
            Instantiate(randomEnemy, randomPosition, Quaternion.identity);
        }

        private string GetRandomName()
        {
            var firstName = _first[(int)RandomNumberGenerator.Instance.Generate(0, _first.Length)];
            var secondName = _second[(int)RandomNumberGenerator.Instance.Generate(0, _first.Length)];
            return $"{firstName} {secondName}";
        }

        private string GetRandomCrime() => _crime[(int)RandomNumberGenerator.Instance.Generate(0, _crime.Length)];

        private int GetRandomBountyAmount() => (int)RandomNumberGenerator.Instance.Generate(50, 100);
        public bool IsActive => _isActive;
    }
}