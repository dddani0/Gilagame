using BountySystem;
using DefaultNamespace;
using UnityEngine;

namespace ManagerSystem
{
    public class CanvasManager : MonoBehaviour
    {
        private PlayerShooter _playerShooter;

        private Player _player;

        //
        public TMPro.TextMeshProUGUI ammunitionText;
        public TMPro.TextMeshProUGUI moneyText;
        //bounty
        public GameObject bountyElement;
        public TMPro.TextMeshProUGUI name;
        public TMPro.TextMeshProUGUI crime;
        public TMPro.TextMeshProUGUI bountyAmount; 

        void Start()
        {
            _playerShooter = GameObject.Find("Player").GetComponent<PlayerShooter>();
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        void Update()
        {
            ammunitionText.text = _playerShooter.GetAmmunition().ToString();
            moneyText.text = _player.Money.ToString();
        }

        public void ShowBounty(Bounty bounty)
        {
            bountyElement.SetActive(true);
            name.text = bounty.Name;
            crime.text = bounty.Crime;
            bountyAmount.text = bounty.Amount.ToString();
        }
    }
}