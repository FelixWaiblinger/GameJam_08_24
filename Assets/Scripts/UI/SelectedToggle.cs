using UnityEngine;
using UnityEngine.UI;

public class SelectedToggle : MonoBehaviour{
    [SerializeField] private Image _isSelectedImg;

    public void SelectToggle() {

        bool arg = _isSelectedImg.enabled;
        _isSelectedImg.enabled = !arg;
    }
}
