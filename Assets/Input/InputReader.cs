using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "LD53/InputReader", order = 0)]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    private GameInput _gameInput;

    private void OnEnable() {
        // tie our callbacks to Input System
        if (_gameInput == null){
            _gameInput = new GameInput();
            
            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);

            SetGameplay();
        }
    }

    // enable input mappings
    public void SetGameplay(){
        _gameInput.UI.Disable();
        _gameInput.Gameplay.Enable();
    }

    public void SetUI(){
        _gameInput.UI.Enable();
        _gameInput.Gameplay.Disable();
    }

    // define events
    public event Action<Vector2> MoveEvent;

    public event Action AbilityEvent;
    public event Action SkipIntroEvent;

    public void OnMovement(InputAction.CallbackContext context)
    {
        // Debug.Log($"Phase: {context.phase}, Value: {context.ReadValue<Vector2>()}");
        if (context.phase == InputActionPhase.Performed) {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnAbility(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            AbilityEvent?.Invoke();
            // SetUI();
        }
    }

    public void OnSkipIntro(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) {
            SkipIntroEvent?.Invoke();
            SetGameplay();
        }
    }
}
