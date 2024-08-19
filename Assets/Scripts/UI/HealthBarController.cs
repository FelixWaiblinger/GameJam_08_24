using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HealthBarController : MonoBehaviour {

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private float _playerHealth;
    [SerializeField] private float _playerMaxHealth;
    [SerializeField] private IntEventChannel _playerHealthIntEventChannel;
    [SerializeField] private IntEventChannel _playerMaxHealthIntEventChannel;

    private void OnEnable() {
        _playerHealthIntEventChannel.OnIntEventRaised += PlayerHealthChange;
        _playerMaxHealthIntEventChannel.OnIntEventRaised += PlayerMaxHealthChange;
    }

    private void PlayerMaxHealthChange(int arg0) {
        _playerMaxHealth = arg0;
        UpdateHealthSlider();
        UpdateHealthText();
    }

    private void PlayerHealthChange(int arg0) {
        _playerHealth = arg0;
        UpdateHealthSlider();
        UpdateHealthText();
    }

    private void UpdateHealthSlider() {
        _slider.value = CalculateHealthPercentage();
    }

    private void UpdateHealthText() {
        _healthText.text = _playerHealth + " / " + _playerMaxHealth;
    }

    private float CalculateHealthPercentage() {
        float newpercent = (float)_playerHealth / (float)_playerMaxHealth;
        return newpercent;
    }


}
