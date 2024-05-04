using System;
using BountySystem;
using UnityEngine;

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

        private CanvasManager _canvasManager;
        public bool isBountyInProgress = false;

        private void Start()
        {
            _canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        }

        public void GetNewBounty()
        {
            if (isBountyInProgress) return;
            var bounty = new Bounty(GetRandomName(), GetRandomCrime(), GetRandomBountyAmount());
            isBountyInProgress = true;
            _canvasManager.ShowBounty(bounty);
        }

        public void AddMoney(int amount)
            => PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + amount);

        public void SubtractMoney(int amount)
            => PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - amount);

        public void ResetBountyCoolision() => isBountyInProgress = isBountyInProgress is false;

        private string GetRandomName()
        {
            var firstName = _first[(int)RandomNumberGenerator.Instance.Generate(0, _first.Length)];
            var secondName = _second[(int)RandomNumberGenerator.Instance.Generate(0, _first.Length)];
            return $"{firstName} {secondName}";
        }

        private string GetRandomCrime() => _crime[(int)RandomNumberGenerator.Instance.Generate(0, _crime.Length)];

        private int GetRandomBountyAmount() => (int)RandomNumberGenerator.Instance.Generate(50, 100);
    }
}