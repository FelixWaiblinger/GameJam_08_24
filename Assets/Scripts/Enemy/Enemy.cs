using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected VoidEventChannel _deathEvent;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] protected Item _item;
    [SerializeField] protected ItemType _itemType;
    [SerializeField] protected int _health;
    [SerializeField] protected int _strength;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _attackDuration;
    [SerializeField] protected float _attackCooldown;

    protected Rigidbody2D _rb;
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

        StartCoroutine(DamageCoroutine());

        if (_health <= 0) Death();
    }

    protected void Death()
    {
        _deathEvent.VoidEvent();

        if (Random.value < 0.4)
        {
            var item = Instantiate(_item, transform.position, Quaternion.identity);
            item.ItemType = _itemType;
        }

        Destroy(gameObject);
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
}
