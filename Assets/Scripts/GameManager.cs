using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputReader _inputs;

    void Awake()
    {
        _inputs.Init();
        _inputs.EnableGameControls();
    }

    void Update()
    {
        
    }
}
