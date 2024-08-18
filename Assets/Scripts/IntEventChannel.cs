using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEventChannel", menuName = "Events/Int Event Channel")]
public class IntEventChannel : ScriptableObject {
    public UnityAction<int> OnIntEventRaised;

    public void RaiseIntEvent(int arg) {
        OnIntEventRaised?.Invoke(arg);
    }
}