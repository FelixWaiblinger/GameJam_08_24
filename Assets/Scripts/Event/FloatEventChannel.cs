using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEvent", menuName = "Events/Float Event")]
public class FloatEventChannel : ScriptableObject
{
    public UnityAction<float> OnFloatEvent;

    public void FloatEvent(float arg)
    {
        OnFloatEvent?.Invoke(arg);
    }
}