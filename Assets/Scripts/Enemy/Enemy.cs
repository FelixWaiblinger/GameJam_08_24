using UnityEngine;

public enum ScaleType
{
    Melee, Range
}

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected ScaleType _scaleType;
    [SerializeField] protected int _health;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _attackDuration;
    [SerializeField] protected float _attackCooldown;

    Rigidbody2D _rb;
    protected Transform _player;
    protected float _attackTimer;

    protected void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void FixedUpdate()
    {
        var distance = Vector3.Distance(transform.position, _player.position);

        if (distance > _attackRange)
            Move();
        else
            Attack();
    }

    protected void Move()
    {
        var direction = (_player.position - transform.position).normalized;
        _rb.velocity = direction * _moveSpeed;
    }

    protected abstract void Attack();

    public void Damage(int amount)
    {
        _health -= amount;

        if (_health <= 0) Death();
    }

    protected void Death()
    {
        // TODO spawn scale

        // TODO called twice on the same object (when entering chill time)... investigate
        Destroy(gameObject);
    }
}
