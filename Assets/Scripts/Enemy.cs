using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEntity
{
    public Entity origin;

    //
    private int _health;
    private NavMeshAgent _agent;
    private GameObject _player;

    private Transform _spriteTransform;
    //

    private bool _isDetected = false;
    private LayerMask _playerLayer = 6;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = origin.speed;
        _health = origin.health;
        _spriteTransform = transform.GetChild(0);
        print(_spriteTransform.gameObject);
    }

    private void Update()
    {
        if (IsAlive() is false) return;
        if (_isDetected is false) return;
        ObjectSpinner.SpinObject(transform, _spriteTransform,
            _agent.nextPosition);
        _agent.SetDestination(_player.transform.position);
    }

    private void LateUpdate()
    {
        if (_isDetected) return;
        if (IsPlayerWithinRadius() is false) return;
        if (IsInConeOfVision() is false) return;
        if (IsPlayerInRay() is false) return;
        _isDetected = _isDetected is false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet")) return;
        var bullet = other.GetComponent<Bullet>();
        Damage(1);
    }

    public void Heal(int addition)
    {
        _health += addition;
    }

    public void Damage(int damage)
    {
        _health -= damage;
        if (_isDetected) return;
        _isDetected = _isDetected is false;
    }

    private bool IsInConeOfVision() =>
        PlayerEnemyAngle() is >= (float)-Entity.VisionConeDegree and <= (float)Entity.VisionConeDegree;

    private float PlayerEnemyAngle() =>
        Vector2.SignedAngle(
            ObjectSpinner.DirectionVector(
                GetPositionVector2(),
                GetPositionVector2() + Vector2.up),
            PlayerDirection());

    private bool IsPlayerWithinRadius() => GetPlayerDistance() < origin.distance;
    public bool IsAlive() => _health > 0;

    public bool IsPlayerInRay() => Physics2D.Raycast(transform.position, PlayerDirection(), origin.distance).collider.gameObject.layer.Equals(6);

    private float GetPlayerDistance() => Vector2.Distance(_player.transform.position, transform.position);
    private Vector2 PlayerDirection() => ObjectSpinner.DirectionVector(GetPositionVector2(), PlayerPosition());
    private Vector2 PlayerPosition() => _player.transform.position;
    private Vector2 GetPositionVector2() => transform.position;
}