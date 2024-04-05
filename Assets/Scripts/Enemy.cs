using System;
using DefaultNamespace;
using NavMeshPlus.Editors.Extensions;
using Unity.Mathematics;
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
    public float shootCooldownSeconds;
    private Timer _shootTimer;
    public GameObject bullet;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = origin.speed;
        _health = origin.health;
        _spriteTransform = transform.GetChild(0);
        _animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        print($"transform.GetChild(0): {_spriteTransform.gameObject}"); //EZT NE TÖRÖLD MÉG KI
        _shootTimer = new Timer(shootCooldownSeconds);
    }

    private void Update()
    {
        if (IsAlive() is false) return;
        _animator.SetFloat("horizontal", GetBlendtreePositions().x);
        _animator.SetFloat("vertical", GetBlendtreePositions().y);
        if (_isDetected is false) return;
        _agent.SetDestination(PlayerPosition());
        ShootTowardsEnemy();
    }

    private void LateUpdate()
    {
        if (_isDetected) return;
        if (IsPlayerWithinRadius() is false) return;
        if (IsInConeOfVision() is false) return;
        if (IsPlayerInSight() is false) return;
        _isDetected = _isDetected is false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet")) return;
        Damage(1);
    }

    public void Heal(int addition)
    {
        _health += addition;
    }

    public void Damage(int damage)
    {
        _health -= damage;
        if (IsAlive() is false)
        {
            _agent.speed = 0;
            enabled = false; 
        }
        if (_isDetected) return;
        _isDetected = _isDetected is false;
    }

    private void ShootTowardsEnemy()
    {
        if (_shootTimer.IsCooldown())
        {
            _shootTimer.DecreaseTimer(Time.deltaTime);
            return;
        }

        if (IsPlayerInSight() is false) return;

        var shot = Instantiate(bullet, GetPositionVector2() + PlayerDirection(), quaternion.identity);
        shot.transform.localEulerAngles =
            new Vector3(0, 0, Vector2.SignedAngle(ObjectSpinner.DirectionVector(
                    GetPositionVector2(),
                    GetPositionVector2() + Vector2.up),
                PlayerDirection()));

        _shootTimer.ResetTimer();
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
    private bool IsAlive() => _health > 0;

    public bool IsPlayerInSight() =>
        Physics2D.Raycast(GetPositionVector2(), PlayerDirection(), origin.distance).collider is not null &&
        Physics2D.Raycast(GetPositionVector2(), PlayerDirection(), origin.distance).collider.gameObject.layer.Equals(6);

    private float GetPlayerDistance() => Vector2.Distance(_player.transform.position, transform.position);
    private Vector2 PlayerDirection() => ObjectSpinner.DirectionVector(GetPositionVector2(), PlayerPosition());
    private Vector2 AgentDirection() => ObjectSpinner.DirectionVector(GetPositionVector2(), AgentPosition());
    private Vector2 PlayerPosition() => _player.transform.position;
    private Vector2 AgentPosition() => _agent.nextPosition;
    private Vector2 GetPositionVector2() => transform.position;
    private Vector2 GetBlendtreePositions() => _isDetected is false ? Vector2.zero : PlayerDirection();
}