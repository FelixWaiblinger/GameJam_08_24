using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Enemy> _enemies = new();
    [SerializeField] BoolEventChannel _modeEvent;
    [SerializeField] Vector2 _areaHalfSize;
    [SerializeField] float _spawnTime;
    int _difficulty = -1;
    float _lastSpawn = -10;
    bool _spawning = false;

    void OnEnable()
    {
        _modeEvent.OnBoolEvent += ToggleSpawning;
    }

    void OnDisable()
    {
        _modeEvent.OnBoolEvent -= ToggleSpawning;
    }

    void Update()
    {
        if (!_spawning) return;

        if (Time.time - _lastSpawn < _spawnTime) return;

        var enemyType = _enemies[Random.Range(0, _enemies.Count)];
        var sign = Random.value >= 0.5 ? 1 : -1;
        Vector3 position;

        // spawn at west/east side
        if (Random.value >= 0.5)
        {
            position = new Vector3(
                _areaHalfSize.x * sign,
                Random.Range(-_areaHalfSize.y, _areaHalfSize.y),
                0
            );
        }
        // spawn at north/south side
        else
        {
            position = new Vector3(
                Random.Range(-_areaHalfSize.x, _areaHalfSize.x),
                _areaHalfSize.y * sign,
                0
            );
        }

        _lastSpawn = Time.time;
        Instantiate(enemyType, position, Quaternion.identity);
    }

    void ToggleSpawning(bool fightMode)
    {
        _spawning = fightMode;

        if (!_spawning)
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }

            var projectiles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (var projectile in projectiles)
            {
                Destroy(projectile);
            }
        }
        else
        {
            // make each fight harder
            _spawnTime = Mathf.Clamp(_spawnTime * Mathf.Pow(0.9f, ++_difficulty), 0.33f, 10);
        }
    }
}
