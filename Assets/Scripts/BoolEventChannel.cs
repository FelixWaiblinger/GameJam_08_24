using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolEventChannel", menuName = "Events/Bool Event Channel")]
public class BoolEventChannel : ScriptableObject {

    public UnityAction<bool> OnBoolEventRaised;

    public void RaiseBoolEvent(bool arg) {
        OnBoolEventRaised?.Invoke(arg);
    }

}
