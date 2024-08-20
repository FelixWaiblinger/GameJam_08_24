using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] FloatEventChannel _timeEvent;
    [SerializeField] BoolEventChannel _modeEvent;
    [SerializeField] IntEventChannel _healthEvent;
    [SerializeField] Image _timeBar;
    [SerializeField] TMP_Text _wave;
    [SerializeField] Image _healthBar;
    [SerializeField] TMP_Text _health;
    bool _fighting = false;
    float _fightTime = 30;
    float _chillTime = 10;
    int _waveCount = 0;
    int _currentHealth = 0;
    int _maxHealth = 0;

    void OnEnable()
    {
        _timeEvent.OnFloatEvent += UpdateTime;
        _modeEvent.OnBoolEvent += UpdateWave;
        _healthEvent.OnIntEvent += UpdateHealth;
    }

    void OnDisable()
    {
        _timeEvent.OnFloatEvent -= UpdateTime;
        _modeEvent.OnBoolEvent -= UpdateWave;
        _healthEvent.OnIntEvent -= UpdateHealth;
    }

    void Start()
    {
        StartCoroutine(StartUpCoroutine());
    }

    void UpdateTime(float remaining)
    {
        var max = _fighting ? _fightTime : _chillTime;
        _timeBar.fillAmount = remaining / max;
    }

    void UpdateWave(bool fight)
    {
        _fighting = fight;
        if (!_fighting) _waveCount++;
        _wave.text = "Wave: " + _waveCount;
    }

    void UpdateHealth(int current)
    {
        _currentHealth = current;
        if (_currentHealth > _maxHealth) _maxHealth = _currentHealth;

        _health.text = "" + _currentHealth + "/" + _maxHealth;
        _healthBar.fillAmount = _currentHealth / (float)_maxHealth;
    }

    IEnumerator StartUpCoroutine()
    {
        var time = 0f;
        
        while (time < 3f)
        {
            _healthBar.fillAmount = time / 3f;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _timeBar.fillAmount = 1;
        _healthBar.fillAmount = 1;
    }
}
