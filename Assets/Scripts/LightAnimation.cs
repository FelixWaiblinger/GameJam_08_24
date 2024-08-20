using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAnimation : MonoBehaviour
{
    [SerializeField] float _intensityMean;
    Light2D _light;

    void Start()
    {
        _light = GetComponent<Light2D>();
    }

    void Update()
    {
        _light.intensity = _intensityMean + Mathf.Sin(Time.time) * 2;
    }
}
