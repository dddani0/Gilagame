using System;
using DefaultNamespace;
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
    private Timer shootTimer;
    public GameObject bullet;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = origin.speed;
        _health = origin.health;
        _spriteTransform = transform.GetChild(0);
        print(_spriteTransform.gameObject); //EZT NE TÖRÖLD MÉG KI
        shootTimer = new Timer(shootCooldownSeconds);
    }

    private void Update()
    {
        if (IsAlive() is false) return;
        if (_isDetected is false) return;
        ObjectSpinner.SpinObject(transform, _spriteTransform,
            _agent.nextPosition);
        _agent.SetDestination(_player.transform.position);
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

    private void ShootTowardsEnemy()
    {
        if (shootTimer.IsCooldown())
        {
            shootTimer.DecreaseTimer(Time.deltaTime);
            return;
        }

        if (IsPlayerInSight() is false) return;

        var shot = Instantiate(bullet, GetPositionVector2() + PlayerDirection(), quaternion.identity);
        shot.transform.localEulerAngles =
            new Vector3(0, 0, Vector2.SignedAngle(ObjectSpinner.DirectionVector(
                    GetPositionVector2(),
                    GetPositionVector2() + Vector2.up),
                PlayerDirection()));
        
        shootTimer.ResetTimer();
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

    public bool IsPlayerInSight() =>
        Physics2D.Raycast(GetPositionVector2(), PlayerDirection(), origin.distance).collider is not null &&
        Physics2D.Raycast(GetPositionVector2(), PlayerDirection(), origin.distance).collider.gameObject.layer.Equals(6);

    private float GetPlayerDistance() => Vector2.Distance(_player.transform.position, transform.position);
    private Vector2 PlayerDirection() => ObjectSpinner.DirectionVector(GetPositionVector2(), PlayerPosition());
    private Vector2 PlayerPosition() => _player.transform.position;
    private Vector2 GetPositionVector2() => transform.position;
}