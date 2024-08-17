using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Event/InputReader")]
public class InputReader : ScriptableObject, Controls.IGameActions
{
    public static UnityAction<Vector2> MoveEvent;
    public static UnityAction<Vector2> AimEvent;
    public static UnityAction DashEvent;
    public static UnityAction AttackEvent;
    public static UnityAction MenuEvent;
    public static UnityAction PauseEvent;

    private static Controls _controls;

    #region CONTROL

    public void Init()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Game.SetCallbacks(this);
        }
    }

    public void EnableGameControls()
    {
        _controls.Game.Enable();
    }

    public void DisableGameControls()
    {
        _controls.Game.Disable();
    }

    public void EnableUIControls()
    {
        // _controls.UI.Enable();
    }

    public void DisableUIControls()
    {
        // _controls.UI.Disable();
    }

    #endregion

    #region INPUTS

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started) DashEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) AttackEvent?.Invoke();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.started) MenuEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started) PauseEvent?.Invoke();
    }

    #endregion
}
