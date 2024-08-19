using System.Collections;
using UnityEngine;

public class Melee : Enemy
{
    [SerializeField] GameObject _hitAreaIndicator;

    protected override void Attack()
    {
        // still attacking
        if (_attackTimer + _attackDuration >= Time.time) return;

        // attack on cooldown
        if (_attackTimer + _attackCooldown >= Time.time) return;

        // TODO attack
        StartCoroutine(MeleeCoroutine());
        _attackTimer = Time.time;
    }

    IEnumerator MeleeCoroutine()
    {
        _hitAreaIndicator.SetActive(true);

        for (int i = 0; i < 50; i++)
        {
            _sprite.transform.rotation *= Quaternion.Euler(0, 0, i * 0.2f);
            yield return new WaitForEndOfFrame();
        }

        _hitAreaIndicator.SetActive(false);
        var player = Physics2D.CircleCast(
            transform.position,
            0.7f,
            Vector2.up,
            0.01f,
            1 << LayerMask.NameToLayer("Player")
        );

        player.collider?.GetComponent<IDamagable>().Damage(_strength);

        for (int i = 0; i < 20; i++)
        {
            _sprite.transform.rotation *= Quaternion.Euler(0, 0, -i * 3);
            yield return new WaitForEndOfFrame();
        }

        _sprite.transform.rotation = Quaternion.identity;
    }
}
