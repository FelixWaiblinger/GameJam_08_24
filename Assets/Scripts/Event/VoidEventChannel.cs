using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEvent", menuName = "Events/Void Event")]
public class VoidEventChannel : ScriptableObject
{
    public UnityAction OnVoidEvent;

    public void VoidEvent()
    {
        OnVoidEvent?.Invoke();
    }
}
