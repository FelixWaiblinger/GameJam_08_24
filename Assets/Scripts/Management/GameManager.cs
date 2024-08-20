using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputReader _inputs;
    [SerializeField] GameData _data;
    [SerializeField] FloatEventChannel _timeEvent;
    [SerializeField] BoolEventChannel _modeEvent;
    [SerializeField] VoidEventChannel _playerDeathEvent;
    [SerializeField] VoidEventChannel _enemyDeathEvent;
    [SerializeField] float _fightTime;
    [SerializeField] float _chillTime;
    bool _fighting = false;
    float _currentTime = 3;
    int _enemiesKilled = 0;
    int _wavesSurvived = 0;

    void Awake()
    {
        _inputs.Init();
        _inputs.EnableGameControls();
    }

    void OnEnable()
    {
        _playerDeathEvent.OnVoidEvent += GameOver;
        _enemyDeathEvent.OnVoidEvent += CountEnemy;
    }

    void OnDisable()
    {
        _playerDeathEvent.OnVoidEvent -= GameOver;
        _enemyDeathEvent.OnVoidEvent -= CountEnemy;
    }

    void Update()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        _timeEvent?.FloatEvent(_currentTime);

        if (_currentTime > 0) _currentTime -= Time.deltaTime;
        else
        {
            _fighting = !_fighting;
            _currentTime = _fighting ? _fightTime : _chillTime;
            _modeEvent?.BoolEvent(_fighting);

            // statistics
            if (!_fighting) _wavesSurvived++;
        }
    }

    void GameOver()
    {
        _inputs.DisableGameControls();

        _data.Waves = _wavesSurvived;
        _data.Enemies = _enemiesKilled;

#if !UNITY_WEBGL
        ReaderWriterJSON.SaveToJSON(_data);
#endif

        StartCoroutine(ReturnCoroutine());
    }

    void CountEnemy()
    {
        // statistic
        _enemiesKilled++;
    }

    IEnumerator ReturnCoroutine()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("Menu");
    }
}
