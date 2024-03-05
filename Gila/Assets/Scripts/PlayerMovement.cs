using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction playerMove;

    //
    private Vector2 _input;
    //
    public float movementSpeed;

    //
    private Rigidbody _playerRigidbody;

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        FetchInput();
        _playerRigidbody.velocity = _input * (movementSpeed * (1/Time.deltaTime) * Time.deltaTime);
    }

    private void OnEnable()
    {
        playerMove.Enable();
    }

    private void OnDisable()
    {
        playerMove.Disable();
    }

    private void FetchInput()
    {
        _input = playerMove.ReadValue<Vector2>();
    }
    //
}