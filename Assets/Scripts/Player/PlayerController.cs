using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera _camera;
    Rigidbody2D _rb;

    [Header("Movement")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    [SerializeField] float _dashCooldown;
    Vector2 _moveDirection = Vector2.zero;
    float _currentSpeed = 0;

    [Header("Attack")]
    [SerializeField] Transform _attack;
    [SerializeField] int _attackDamage;
    [SerializeField] float _attackDuration;
    [SerializeField] float _attackCooldown;
    Coroutine _attackCoroutine;
    Vector3 _targetPosition = Vector3.zero;
    Vector3 ignoreZ = new Vector3(1, 1, 0);

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

    // attack hit area
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

        other.GetComponent<IDamagable>().Damage(_attackDamage);
    }

    void Start()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void Move(Vector2 direction) { if (!_dashing) _moveDirection = direction.normalized; }
    void Aim(Vector2 position) { _targetPosition = Vector3.Scale(_camera.ScreenToWorldPoint(position), ignoreZ); }
    void Dash() { _dashRequested = true; }
    void Attack() { _attackRequested = true; }

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
            _attackRequested = false;
        }
        else if (!_dashing) _currentSpeed = _moveSpeed;

        // move
        _rb.velocity = _moveDirection * _currentSpeed;
    }

    void UpdateState()
    {
        var currentTime = Time.time;

        // new state
        _dashing = (_dashTimer + _dashDuration) >= currentTime;
        _dashOnCD = (_dashTimer + _dashCooldown) >= currentTime;
        _attacking = (_attackTimer + _attackDuration) >= currentTime;
        _attackOnCD = (_attackTimer + _attackCooldown) >= currentTime;

        Physics2D.IgnoreLayerCollision(6, 7, ignore: _dashing);
        
        if (_dashing && _attacking)
        {
            _attackTimer = -10;
            StopCoroutine(_attackCoroutine);
            _attack.gameObject.SetActive(false);
            _attacking = false;
        }
    }

    IEnumerator AttackCoroutine()
    {
        var speed = 10;
        var sign = Random.value >= 0.5 ? 1 : -1;
        var direction = (_targetPosition - transform.position).normalized;
        var look = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(0, 0, -45) * direction);
        var start = new Vector3(0, 0, (look.eulerAngles.z + sign * -70) % 360);
        var end = new Vector3(0, 0, (look.eulerAngles.z + sign * 70) % 360);

        _attack.rotation = Quaternion.Euler(start);
        _attack.gameObject.SetActive(true);

        for (float i = 0; i < _attackDuration; i += Time.deltaTime)
        {
            _attack.rotation = Quaternion.RotateTowards(_attack.rotation, Quaternion.Euler(end), speed);

            yield return new WaitForEndOfFrame();
        }

        _attack.gameObject.SetActive(false);
    }
}
