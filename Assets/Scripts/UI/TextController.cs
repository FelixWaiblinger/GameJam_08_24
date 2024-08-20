using System;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _scalesCount;
    [SerializeField] private TextMeshProUGUI _healthCount;

    [SerializeField] private IntEventChannel _scalesCountEventChannel;
    [SerializeField] private IntEventChannel _healthCountEventChannel;

    private void OnEnable() {
        _scalesCountEventChannel.OnIntEvent += ChangeScalesCount;
        _healthCountEventChannel.OnIntEvent += ChangeHealthCount;

    }
    private void OnDisable() {
        _scalesCountEventChannel.OnIntEvent -= ChangeScalesCount;
        _healthCountEventChannel.OnIntEvent -= ChangeHealthCount;
    }

    private void ChangeHealthCount(int arg0) {
        _healthCount.text = "HP: " + arg0;
    }

    private void ChangeScalesCount(int arg0) {
        _scalesCount.text = "Scales: " + arg0;
    }
}
