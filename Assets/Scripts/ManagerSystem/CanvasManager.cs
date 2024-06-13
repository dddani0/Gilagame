using BountySystem;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        // death panel
        public Image bloodImage;
        public GameObject deathPanelButtons;
        
        // transition
        //public Animator transition;

        void Start()
        {
            _playerShooter = GameObject.Find(TagManager.Instance.PlayerTag).GetComponent<PlayerShooter>();
            _player = _playerShooter.GetComponent<Player>();
        }

        void Update()
        {
            ammunitionText.text = _playerShooter.GetAmmunition().ToString();
            moneyText.text = $"{_player.Money.ToString()}$";
            weaponNameText.text = _playerShooter.GetGun().name;
        }

        public void SetBloodPanelStatus(int status)
        {
            //3 -> invisible
            //2 -> slightly visible
            //1 -> very visible
            //0 -> visibility = 100%
            bloodImage.color = status switch
            {
                3 => new Color(bloodImage.color.r, bloodImage.color.g, bloodImage.color.b, 0),
                2 => new Color(bloodImage.color.r, bloodImage.color.g, bloodImage.color.b, 1/4f),
                1 => new Color(bloodImage.color.r, bloodImage.color.g, bloodImage.color.b, 1/3f),
                0 => new Color(bloodImage.color.r, bloodImage.color.g, bloodImage.color.b, 1/1.5f),
                _ => bloodImage.color
            };
        }

        //public void SetTransitionState(int val) => transition.SetInteger("transition", val);

        public void EnableDeathButtons() => deathPanelButtons.SetActive(true);

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

        public void ShowBounty(Bounty bounty)
        {
            bountyElement.SetActive(true);
            name.text = bounty.Name;
            crime.text = bounty.Crime;
            bountyAmount.text = $"Reward: {bounty.Amount}";
        }
    }
}