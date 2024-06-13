using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ManagerSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    public InputAction readSign;
    public string title;
    public string bodyContent;
    private bool _isInPrompt;
    private PlayerShooter _playerShooter;
    private CanvasManager _canvasManager;
    private IngameManager _ingameManager;

    private void Start()
    {
        _playerShooter = GameObject.FindGameObjectWithTag(TagManager.Instance.PlayerTag).GetComponent<PlayerShooter>();
        _canvasManager = GameObject.FindGameObjectWithTag(TagManager.Instance.CanvasManagerTag)
            .GetComponent<CanvasManager>();
        _ingameManager = GameObject.FindGameObjectWithTag(TagManager.Instance.IngameManagerTag)
            .GetComponent<IngameManager>();
    }

    private void Update()
    {
        if (_ingameManager.isBountyInProgress) return;
        if (_ingameManager.IsActive is false) return;
        if (_isInPrompt is false) return;
        if (readSign.WasPressedThisFrame() is false) return;
        _canvasManager.ShowInfoPanel(title,bodyContent);
        _ingameManager.DisablePlayerActiveState();
        _ingameManager.EnableCursorVisibility();
    }

    private void OnEnable()
    {
        readSign.Enable();
    }

    private void OnDisable()
    {
        readSign.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(TagManager.Instance.PlayerTag)) return;
        _playerShooter.ShowButtonPrompter(readSign);
        _isInPrompt = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _playerShooter.DisableButtonPrompter();
        _isInPrompt = false;
    }
}