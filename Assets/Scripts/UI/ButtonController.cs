using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

    [SerializeField] private VoidEventChannel _buttonPressed;

    public void ButtonPressed() {
        _buttonPressed.VoidEvent();
    }
}
