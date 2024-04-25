using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class CanvasManager : MonoBehaviour
{
    private PlayerShooter _playerShooter;

    private Player _player;

    //
    public TMPro.TextMeshProUGUI ammunitionText;

    void Start()
    {
        _playerShooter = GameObject.Find("Player").GetComponent<PlayerShooter>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        ammunitionText.text = _playerShooter.GetAmmunition().ToString();
    }
}