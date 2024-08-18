using UnityEngine;

public class Melee : Enemy
{
    protected override void Attack()
    {
        var time = Time.time;

        // still attacking
        if (_attackTimer + _attackDuration >= time) return;

        // attack on cooldown
        if (_attackTimer + _attackCooldown >= time) return;

        // TODO attack

        _attackTimer = time;
    }
}
