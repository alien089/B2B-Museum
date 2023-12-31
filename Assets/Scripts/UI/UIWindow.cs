using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of generic UI window
/// </summary>
public class UIWindow : MonoBehaviour
{
    protected UIManager m_UIManager;
    protected Animator m_Animator;
    [HideInInspector]
    public Window m_WindowType;

    protected virtual void Awake()
    {
        m_UIManager = GetComponentInParent<UIManager>();
        TryGetComponent(out m_Animator);
    }

    protected void CloseWindow() => gameObject.SetActive(false);

    protected void ButtonSound() => SoundManager.Play("Button");

}
