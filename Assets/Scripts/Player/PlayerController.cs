using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable, ICollector
{
    [SerializeField] VoidEventChannel _deathEvent;
    [SerializeField] IntEventChannel _healthEvent;
    Camera _camera;

    [Header("Movement")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    [SerializeField] float _dashCooldown;
    Rigidbody2D _rb;
    Vector2 _moveDirection = Vector2.zero;
    float _currentSpeed = 0;

    [Header("Attack")]
    [SerializeField] GameObject _attackVisual;
    [SerializeField] Vector2 _attackAreaSize;
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackDuration;
    [SerializeField] float _attackCooldown;
    Coroutine _attackCoroutine;
    Vector3 _targetPosition = Vector3.zero;
    Vector3 ignoreZ = new Vector3(1, 1, 0);
    float _attackSign = 1;

    [Header("Health")]
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] int _health;
    [SerializeField] float _immunityDuration;

    [Header("Upgrades")]
    [SerializeField] Inventory _inventory;

    // timer
    float _dashTimer = -10;
    float _attackTimer = -10;
    float _immunityTimer = -10;
    // requests
    bool _dashRequested = false;
    bool _attackRequested = false;
    // state
    bool _dashing = false;
    bool _dashOnCD = false;
    bool _attacking = false;
    bool _attackOnCD = false;
    bool _immunity = false;

    void OnEnable()
    {
        InputReader.MoveEvent += Move;
        InputReader.AimEvent += Aim;
        InputReader.DashEvent += Dash;
        InputReader.AttackEvent += Attack;
        _healthEvent.OnIntEvent += Health;
    }

    void OnDisable()
    {
        InputReader.MoveEvent -= Move;
        InputReader.AimEvent -= Aim;
        InputReader.DashEvent -= Dash;
        InputReader.AttackEvent -= Attack;
        _healthEvent.OnIntEvent += Health;
    }

    void Start()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();

        _healthEvent.IntEvent(_health);
    }
    
    void Move(Vector2 direction) { if (!_dashing) _moveDirection = direction.normalized; }
    void Aim(Vector2 position) { _targetPosition = Vector3.Scale(_camera.ScreenToWorldPoint(position), ignoreZ); }
    void Dash() { _dashRequested = true; }
    void Attack() { _attackRequested = true; }
    void Health(int health) { _health = health; }

    void FixedUpdate()
    {
        // process input and start actions
        StartActions();

        // timers, state, requests
        UpdateState();
    }

    void StartActions()
    {
        var currentTime = Time.time;
        var dash = _dashRequested && !_dashing && !_dashOnCD;
        var attack = _attackRequested && !_attacking && !_dashing && !_attackOnCD;

        if (dash)
        {
            _currentSpeed =_dashSpeed;
            _dashTimer = currentTime;
            _dashRequested = false;
        }
        else if (attack)
        {
            _currentSpeed = 0;
            _attackTimer = currentTime;
            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }
        else if (!_dashing) _currentSpeed = _moveSpeed;

        // move
        _rb.velocity = _moveDirection * _currentSpeed;
        
        // do not save this input action
        _attackRequested = false;
    }

    void UpdateState()
    {
        var currentTime = Time.time;
        var atkMultiplier = GetAttackSpeedMultiplier();

        // new state
        _dashing = (_dashTimer + _dashDuration) >= currentTime;
        _dashOnCD = (_dashTimer + _dashCooldown) >= currentTime;
        _attacking = (_attackTimer + _attackDuration * atkMultiplier) >= currentTime;
        _attackOnCD = (_attackTimer + _attackCooldown * atkMultiplier) >= currentTime;
        _immunity = (_immunityTimer + _immunityDuration) >= currentTime;

        Physics2D.IgnoreLayerCollision(6, 7, ignore: _dashing);
        Physics2D.IgnoreLayerCollision(6, 8, ignore: _dashing);
        
        if (_dashing && _attacking)
        {
            _attackTimer = -10;
            StopCoroutine(_attackCoroutine);
            _attackVisual.SetActive(false);
            _attacking = false;
        }
    }

    float GetDamageMultiplier()
    {
        var multiplier = 1f;
        foreach (var item in _inventory.Items)
        {
            if (item.Name == "MeleeScale") multiplier *= item.Multiplier;
        }
        return multiplier;
    }

    float GetAttackSpeedMultiplier()
    {
        var multiplier = 1f;
        foreach (var item in _inventory.Items)
        {
            if (item.Name == "RangeScale") multiplier *= item.Multiplier;
        }
        return multiplier;
    }

    #region INTERFACE

    public void Damage(int amount)
    {
        if (_immunity) return;

        _health -= amount;
        _immunityTimer = Time.time;
        StartCoroutine(DamageCoroutine());
        _healthEvent.IntEvent(_health);

        // TODO game over
        if (_health <= 0) _deathEvent.VoidEvent();
    }

    public bool Collect(ItemType item)
    {
        return _inventory.Add(item);
    }

    #endregion

    #region COROUTINES

    IEnumerator AttackCoroutine()
    {
        var damage = (int)(_attackDamage * GetDamageMultiplier()) + 1;
        var speed = _attackDuration * GetAttackSpeedMultiplier();

        var direction = (_targetPosition - transform.position).normalized;
        var look = Quaternion.LookRotation(
            Vector3.forward,
            Quaternion.Euler(0, 0, -45) * direction
        );
        var start = new Vector3(0, 0, (look.eulerAngles.z + _attackSign * -70) % 360);
        var end = new Vector3(0, 0, (look.eulerAngles.z + _attackSign * 70) % 360);
        List<int> damageApplied = new();

        _attackVisual.transform.rotation = Quaternion.Euler(start);
        _attackVisual.SetActive(true);

        for (float i = 0; i < speed; i += Time.deltaTime)
        {
            // find all enemies in attack area
            var enemies = Physics2D.BoxCastAll(
                _attackVisual.transform.GetChild(0).position,
                _attackAreaSize,
                _attackVisual.transform.rotation.z,
                Vector2.up,
                0.01f,
                1 << LayerMask.NameToLayer("Enemy")
            );

            // damage these enemies
            foreach (var enemy in enemies)
            {
                if (damageApplied.Contains(enemy.collider.GetInstanceID())) continue;
                enemy.collider.GetComponent<IDamagable>().Damage(damage);
                damageApplied.Add(enemy.collider.GetInstanceID());
            }

            _attackVisual.transform.rotation = Quaternion.RotateTowards(
                _attackVisual.transform.rotation,
                Quaternion.Euler(end),
                10
            );

            yield return new WaitForEndOfFrame();
        }

        _attackSign *= -1;
        _attackVisual.SetActive(false);
    }

    IEnumerator DamageCoroutine()
    {
        var normalColor = _sprite.color;

        for (int i = 0; i < 10; i++)
        {
            _sprite.color = i % 2 == 0 ? Color.red : normalColor;
            yield return new WaitForEndOfFrame();
        }

        _sprite.color = normalColor;
    }

    #endregion
}
