using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("playerMove")] public InputAction move;
    private Vector2 _input;
    public float movementSpeed;
    private Rigidbody2D _playerPhysicsRigidbody;

    private void OnEnable()
    {
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    void Start()
    {
        _playerPhysicsRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FetchInput();
        _playerPhysicsRigidbody.velocity = _input * (movementSpeed * (1 / Time.deltaTime) * Time.deltaTime);
    }


    private void FetchInput()
    {
        _input = move.ReadValue<Vector2>();
    }
}