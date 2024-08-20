using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEvent", menuName = "Events/Int Event")]
public class IntEventChannel : ScriptableObject
{
    public UnityAction<int> OnIntEvent;

    public void IntEvent(int arg)
    {
        OnIntEvent?.Invoke(arg);
    }
}