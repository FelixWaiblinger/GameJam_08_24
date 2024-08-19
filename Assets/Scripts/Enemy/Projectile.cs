using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    Vector3 _direction = Vector3.zero;
    int _strength = 0;
    float _spawnTime;

    public void Init(Vector3 direction, int strength)
    {
        _direction = direction;
        _strength = strength;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _direction * _speed;

        _spawnTime = Time.time;
    }

    void FixedUpdate()
    {
        var player = Physics2D.CircleCast(
            transform.position,
            0.3f,
            Vector2.up,
            0.01f,
            1 << LayerMask.NameToLayer("Player")
        );

        if (player.collider != null)
        {
            player.collider.GetComponent<IDamagable>().Damage(_strength);
            Destroy(gameObject);
        }

        if (_spawnTime + _lifeTime > Time.time) return;

        Destroy(gameObject);
    }
}
