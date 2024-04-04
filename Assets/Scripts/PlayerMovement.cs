using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("playerMove")] public InputAction move;
    private Vector2 _input;
    public float movementSpeed;
    private Rigidbody2D _playerPhysicsRigidbody;
    private Animator _animation;

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
        _animation = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        FetchInput();
        _playerPhysicsRigidbody.velocity = _input * (movementSpeed * (1 / Time.deltaTime) * Time.deltaTime);
        _animation.SetFloat("horizontal", _input.x);
        _animation.SetFloat("vertical", _input.y);
    }


    private void FetchInput()
    {
        _input = move.ReadValue<Vector2>();
    }
}