using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IndexButtonPress", menuName = "UI/IndexButtonPress")]
public class SO_IndexButtonPress : ScriptableObject {

    public int index;
    public UnityAction OnIndexButtonEventRaised;

    public void RaiseDataEvent(int buttonIndex) {
        index = buttonIndex;
        OnIndexButtonEventRaised?.Invoke();
    }
}