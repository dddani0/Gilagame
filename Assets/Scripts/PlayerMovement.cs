using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("playerMove")] public InputAction move;
    private Vector2 _input;
    public float movementSpeed;
    private Rigidbody2D _playerPhysicsRigidbody;
    private Animator _animation;
    private bool _isActive = true;
    private CanvasManager cm;

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
        cm = GameObject.Find("Canvas").GetComponent<CanvasManager>();
    }

    void Update()
    {
        if (_isActive is false) return;
        FetchInput();
        _playerPhysicsRigidbody.velocity = _input * (movementSpeed * (1 / Time.deltaTime) * Time.deltaTime);
        _animation.SetFloat("horizontal", _input.x);
        _animation.SetFloat("vertical", _input.y);
    }


    private void FetchInput()
    {
        _input = move.ReadValue<Vector2>();
    }

    public void ChangeActiveState() => _isActive = _isActive is false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.name.ToLower())
        {
            case "saloontrigger":
                SceneManager.LoadScene(2);
                break;
            case "sherifftrigger":
                SceneManager.LoadScene(4);
                break;
            case "gunsmithtrigger":
                SceneManager.LoadScene(3);
                break;
            case "billboardtrigger":
                ChangeActiveState();
                break;
        }
    }
}