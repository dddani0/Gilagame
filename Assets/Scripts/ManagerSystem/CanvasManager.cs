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

        public TMPro.TextMeshProUGUI weaponNameText;

        //bounty
        public GameObject bountyElement;
        public TMPro.TextMeshProUGUI name;
        public TMPro.TextMeshProUGUI crime;

        public TMPro.TextMeshProUGUI bountyAmount;

        // shop
        public GameObject shopElement;
        
        //info panel
        public GameObject infoPanelElement;
        public TMPro.TextMeshProUGUI infoPanelTitle;
        public TMPro.TextMeshProUGUI infoPanelBody;

        void Start()
        {
            _playerShooter = GameObject.Find(TagManager.Instance.PlayerTag).GetComponent<PlayerShooter>();
            _player = _playerShooter.GetComponent<Player>();
        }

        void Update()
        {
            ammunitionText.text = _playerShooter.GetAmmunition().ToString();
            moneyText.text = $"{_player.Money.ToString()}$";
            weaponNameText.text = _playerShooter.gunOrigin.name;
        }

        public void ShowShop()
        {
            shopElement.SetActive(true);
        }

        public void DisableShop()
        {
            shopElement.SetActive(false);
        }

        public void ShowInfoPanel(string title, string body)
        {
            infoPanelElement.SetActive(true);
            infoPanelTitle.text = title;
            infoPanelBody.text = body;
        }

        public void DisableInfoPanel()
        {
            infoPanelElement.SetActive(false);
        }

        public void ShowBounty(Bounty bounty)
        {
            bountyElement.SetActive(true);
            name.text = bounty.Name;
            crime.text = bounty.Crime;
            bountyAmount.text = $"Reward: {bounty.Amount}";
            //insert icon
        }
    }
}