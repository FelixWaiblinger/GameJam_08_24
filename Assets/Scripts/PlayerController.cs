using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D _rb;
    CircleCollider2D _hitbox;

    [SerializeField] float _moveSpeed;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    [SerializeField] float _dashCooldown;
    [SerializeField] float _attackDuration;
    [SerializeField] float _attackCooldown;
    Vector2 _moveDirection = Vector2.zero;
    Vector2 _targetPosition = Vector2.zero;
    float _currentSpeed = 0;

    // timer
    float _dashTimer = -10;
    float _attackTimer = -10;
    // requests
    bool _dashRequested = false;
    bool _attackRequested = false;
    // state
    bool _dashing = false;
    bool _dashOnCD = false;
    bool _attacking = false;
    bool _prevAttacking = false;
    bool _attackOnCD = false;

    void OnEnable()
    {
        InputReader.MoveEvent += Move;
        InputReader.AimEvent += Aim;
        InputReader.DashEvent += Dash;
        InputReader.AttackEvent += Attack;
    }

    void OnDisable()
    {
        InputReader.MoveEvent -= Move;
        InputReader.AimEvent -= Aim;
        InputReader.DashEvent -= Dash;
        InputReader.AttackEvent -= Attack;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hitbox = GetComponent<CircleCollider2D>();
    }
    
    void Move(Vector2 direction) { _moveDirection = direction.normalized; }
    void Aim(Vector2 position) { _targetPosition = position; }
    void Dash() { _dashRequested = true; }
    void Attack() { _attackRequested = true; }

    void FixedUpdate()
    {
        // process input
        StartActions();

        // timers, state, requests
        UpdateState();
        
        // move, act, use
        Execute();
    }

    void StartActions()
    {
        var currentTime = Time.time;
        var dash = _dashRequested && !_dashing && !_dashOnCD;
        var attack = _attackRequested && !_attacking && !_dashing && !_attackOnCD;

        if (dash) { _dashTimer = currentTime; }
        else if (attack) { _attackTimer = currentTime; }
    }

    void UpdateState()
    {
        var currentTime = Time.time;
        var fxdDelta = Time.fixedDeltaTime;

        // state of previous update
        _prevAttacking = _attacking;

        // new state
        _dashing = (_dashTimer + _dashDuration) >= currentTime;
        _dashOnCD = (_dashTimer + _dashCooldown) >= currentTime;
        _attacking = (_attackTimer + _attackDuration) >= currentTime;
        _attackOnCD = (_attackTimer + _attackCooldown) >= currentTime;
        
        // request timeouts
        // _attackRequested -= _attackRequested > 0 ? fxdDelta : 0;

        // execute / cancel
        _dashRequested = false;
        _currentSpeed = _dashing ? _dashSpeed : (_attacking ? 0 : _moveSpeed);
        _hitbox.excludeLayers = LayerMask.NameToLayer(_dashing ? "Enemy" : "Nothing");
        if (_dashing && _attacking)
        { 
            _attackTimer = -10;
            _attacking = false;
        }
    }

    void Execute()
    {
        // move
        _rb.velocity = _moveDirection * _currentSpeed;

        // attack
        if (_prevAttacking && !_attacking)
        {
            _attackRequested = false;
            
            // TODO attack
        }
    }
}
