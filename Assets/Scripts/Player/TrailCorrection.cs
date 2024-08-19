using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCorrection : MonoBehaviour
{
    [SerializeField] Transform _movingParent;
    TrailRenderer _renderer;
    Vector3 _lastPosition;

    void Start()
    {
        _renderer = GetComponent<TrailRenderer>();
        _lastPosition = _movingParent.position;        
    }

    void LateUpdate()
    {
        var delta = _movingParent.position - _lastPosition;
        _lastPosition = _movingParent.position;

        var positions = new Vector3[_renderer.positionCount];
        _renderer.GetPositions(positions);

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] += delta;
        }

        _renderer.SetPositions(positions);
    }
}
