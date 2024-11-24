using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using GameMain;


namespace Game.Scripts.Runtime.Input
{
    public class InputComponent : UnityGameFramework.Runtime.GameFrameworkComponent
    {
        public MainInputs InputMap { get; private set; }
        private MainInputs.GamePlayActions _gameplayActions { get; set; }
        private MainInputs.UIActions _uiActions { get; set; }

        private Vector2 m_LastFrameMoveInput;
        private Dictionary<InputEvent, InputAction> eventActionDict;

        protected override void Awake()
        {
            InputMap = new MainInputs();
            InputMap.Enable();
            _gameplayActions = InputMap.GamePlay;
            _uiActions = InputMap.UI;

            eventActionDict = new()
            {
                { InputEvent.Jump, _gameplayActions.Jump },
                { InputEvent.Interact, _gameplayActions.Interactive },
                { InputEvent.UIReturn, _uiActions.Return }
            };
        }

        private void Update()
        {
            m_LastFrameMoveInput = InputData.MoveInput;
            InputData.Clear();
            CheckEventStart();
            CheckHasEvent();

            void CheckEventStart()
            {
                if (m_LastFrameMoveInput == Vector2.zero && _gameplayActions.Move.ReadValue<Vector2>() != Vector2.zero)
                {
                    InputData.AddEventStart(InputEvent.Move);
                }

                foreach (var pair in eventActionDict)
                {
                    if (pair.Value.triggered)
                    {
                        InputData.AddEventStart(pair.Key);
                    }
                }
            }

            void CheckHasEvent()
            {
                //Move
                if (_gameplayActions.Move.ReadValue<Vector2>() != Vector2.zero)
                {
                    InputData.AddEvent(InputEvent.Move);
                    InputData.MoveInput = _gameplayActions.Move.ReadValue<Vector2>();
                }

                foreach (var pair in eventActionDict)
                {
                    if (pair.Value.phase == InputActionPhase.Performed ||
                        pair.Value.phase == InputActionPhase.Started)
                    {
                        InputData.AddEvent(pair.Key);
                    }
                }
            }
        }

        #region Rebind

        public void StartRebind(string actionName, int bindingIndex,
            Text statusText, bool excludeMouse)
        {
            InputAction action = InputMap.asset.FindAction(actionName);
            if (action == null || action.bindings.Count <= bindingIndex)
            {
                Debug.Log("Couldn't find action or binding");
                return;
            }

            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                    DoRebind(action, bindingIndex, statusText, true, excludeMouse);
            }
            else
                DoRebind(action, bindingIndex, statusText, false, excludeMouse);
        }

        private void DoRebind(InputAction actionToRebind, int bindingIndex, Text statusText,
            bool allCompositeParts, bool excludeMouse)
        {
            if (actionToRebind == null || bindingIndex < 0)
                return;

            statusText.text = $"Press a {actionToRebind.expectedControlType}";

            actionToRebind.Disable();

            var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

            rebind.OnComplete(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose();

                if (allCompositeParts)
                {
                    var nextBindingIndex = bindingIndex + 1;
                    if (nextBindingIndex < actionToRebind.bindings.Count &&
                        actionToRebind.bindings[nextBindingIndex].isComposite)
                        DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
                }

                SaveBindingOverride(actionToRebind);
                // rebindComplete?.Invoke();
            });

            rebind.OnCancel(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose();

                // rebindCanceled?.Invoke();
            });

            rebind.WithCancelingThrough("<Keyboard>/escape");

            if (excludeMouse)
                rebind.WithControlsExcluding("Mouse");

            // rebindStarted?.Invoke(actionToRebind, bindingIndex);
            rebind.Start(); //actually starts the rebinding process
        }

        public string GetBindingName(string actionName, int bindingIndex)
        {
            InputAction action = InputMap.asset.FindAction(actionName);
            return action.GetBindingDisplayString(bindingIndex);
        }

        private void SaveBindingOverride(InputAction action)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
            }
        }

        public void LoadBindingOverride(string actionName)
        {
            InputAction action = InputMap.asset.FindAction(actionName);

            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                    action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
            }
        }

        public void ResetBinding(string actionName, int bindingIndex)
        {
            InputAction action = InputMap.asset.FindAction(actionName);

            if (action == null || action.bindings.Count <= bindingIndex)
            {
                Debug.Log("Could not find action or binding");
                return;
            }

            if (action.bindings[bindingIndex].isComposite)
            {
                for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                    action.RemoveBindingOverride(i);
            }
            else
                action.RemoveBindingOverride(bindingIndex);

            SaveBindingOverride(action);
        }

        #endregion
    }
}