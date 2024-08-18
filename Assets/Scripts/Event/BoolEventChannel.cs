using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolEvent", menuName = "Events/BoolEvent")]
public class BoolEventChannel : ScriptableObject
{
    public UnityAction<bool> OnBoolEvent;

    public void BoolEvent(bool arg)
    {
        OnBoolEvent?.Invoke(arg);
    }
}