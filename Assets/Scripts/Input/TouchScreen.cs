//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/TouchScreen.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TouchScreen: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TouchScreen()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchScreen"",
    ""maps"": [
        {
            ""name"": ""PuzzleActions"",
            ""id"": ""e1edfaa6-3765-4d65-95bf-0dfa660ac77e"",
            ""actions"": [
                {
                    ""name"": ""TouchPos"",
                    ""type"": ""Value"",
                    ""id"": ""e17834cd-b684-4532-8df3-2b3a95cdaab2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c4d9dcab-7def-4064-9c2d-39a6262f6c2d"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PuzzleActions
        m_PuzzleActions = asset.FindActionMap("PuzzleActions", throwIfNotFound: true);
        m_PuzzleActions_TouchPos = m_PuzzleActions.FindAction("TouchPos", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PuzzleActions
    private readonly InputActionMap m_PuzzleActions;
    private List<IPuzzleActionsActions> m_PuzzleActionsActionsCallbackInterfaces = new List<IPuzzleActionsActions>();
    private readonly InputAction m_PuzzleActions_TouchPos;
    public struct PuzzleActionsActions
    {
        private @TouchScreen m_Wrapper;
        public PuzzleActionsActions(@TouchScreen wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchPos => m_Wrapper.m_PuzzleActions_TouchPos;
        public InputActionMap Get() { return m_Wrapper.m_PuzzleActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PuzzleActionsActions set) { return set.Get(); }
        public void AddCallbacks(IPuzzleActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_PuzzleActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PuzzleActionsActionsCallbackInterfaces.Add(instance);
            @TouchPos.started += instance.OnTouchPos;
            @TouchPos.performed += instance.OnTouchPos;
            @TouchPos.canceled += instance.OnTouchPos;
        }

        private void UnregisterCallbacks(IPuzzleActionsActions instance)
        {
            @TouchPos.started -= instance.OnTouchPos;
            @TouchPos.performed -= instance.OnTouchPos;
            @TouchPos.canceled -= instance.OnTouchPos;
        }

        public void RemoveCallbacks(IPuzzleActionsActions instance)
        {
            if (m_Wrapper.m_PuzzleActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPuzzleActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_PuzzleActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PuzzleActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PuzzleActionsActions @PuzzleActions => new PuzzleActionsActions(this);
    public interface IPuzzleActionsActions
    {
        void OnTouchPos(InputAction.CallbackContext context);
    }
}
