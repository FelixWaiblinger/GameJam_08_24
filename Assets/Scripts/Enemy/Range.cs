using System.Collections;
using UnityEngine;

public class Range : Enemy
{
    [SerializeField] GameObject _hitAreaIndicator;
    [SerializeField] Projectile _projectile;
    float _normalSpeed;

    void Update()
    {
        var direction = (_player.position - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    protected override void Attack()
    {
        // still attacking
        if (_attackTimer + _attackDuration >= Time.time) return;

        // attack on cooldown
        if (_attackTimer + _attackCooldown >= Time.time) return;

        // TODO attack
        StartCoroutine(RangeCoroutine());
        _normalSpeed = _moveSpeed;
        _moveSpeed = 0;
        _rb.velocity = Vector2.zero;
        _attackTimer = Time.time;
    }

    IEnumerator RangeCoroutine()
    {
        _hitAreaIndicator.SetActive(true);

        for (int i = 0; i < 40; i++)
        {
            _sprite.transform.localScale -= 0.01f * Vector3.one;
            yield return new WaitForEndOfFrame();
        }

        _hitAreaIndicator.SetActive(false);

        var p = Instantiate(_projectile, _hitAreaIndicator.transform.position, Quaternion.identity);
        p.Init((_player.position - p.transform.position).normalized, _strength);

        for (int i = 0; i < 10; i++)
        {
            _sprite.transform.localScale += 0.04f * Vector3.one;
            yield return new WaitForEndOfFrame();
        }

        _sprite.transform.localScale = Vector3.one;
        _moveSpeed = _normalSpeed;
    }
}
