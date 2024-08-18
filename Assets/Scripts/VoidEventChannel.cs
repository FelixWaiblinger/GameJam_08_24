using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Events/Void Event Channel")]
public class VoidEventChannel : ScriptableObject {
    public UnityAction OnVoidEventRaised;

    public void RaiseVoidEvent() {
        OnVoidEventRaised?.Invoke();
    }
}
