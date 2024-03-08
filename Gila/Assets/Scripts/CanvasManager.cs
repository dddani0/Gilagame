using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private PlayerShooter _playerShooter;

    private Player _player;

    //
    public TMPro.TextMeshProUGUI _ammunitionText;
    public TMPro.TextMeshProUGUI _healthText;

    void Start()
    {
        _playerShooter = GameObject.Find("Player").GetComponent<PlayerShooter>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _ammunitionText.text = $"Ammunition: {_playerShooter.GetAmmunition()}";
        _healthText.text = $"Health: {_player.GetHealth()}";
    }
}