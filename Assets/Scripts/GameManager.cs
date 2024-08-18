using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputReader _inputs;
    [SerializeField] FloatEventChannel _timeEvent;
    [SerializeField] BoolEventChannel _modeEvent;
    [SerializeField] float _fightTime;
    [SerializeField] float _chillTime;
    float _currentTime = 3;
    bool _fighting = false;

    void Awake()
    {
        _inputs.Init();
        _inputs.EnableGameControls();
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
        }
    }
}
