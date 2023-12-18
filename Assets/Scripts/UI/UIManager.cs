using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIWindow m_CurrentWindow;
    private UIWindow m_PreviousWindow;
    private Dictionary<Window, UIWindow> m_WindowsList;
    private Dictionary<Overlay, UIWindow> m_OverlaysList;

    private void Awake() => InitializeManager();

    private void InitializeManager()
    {
        m_WindowsList = new Dictionary<Window, UIWindow>()
        {
            { Window.AppBoot, GetComponentInChildren<AppBoot>(true) },
            { Window.Main, GetComponentInChildren<Main>(true) },
            { Window.HUD, GetComponentInChildren<HUD>(true) },
            { Window.Tickets, GetComponentInChildren<Tickets>(true) }
        };
        m_OverlaysList = new Dictionary<Overlay, UIWindow>()
        {
            { Overlay.Questions, GetComponentInChildren<Questions>(true) },
            { Overlay.Settings, GetComponentInChildren<Settings>(true) },
            { Overlay.Profile, GetComponentInChildren<Profile>(true) }
        };

        if (m_CurrentWindow == null) m_CurrentWindow = m_WindowsList[Window.AppBoot];
        m_CurrentWindow.gameObject.SetActive(true);
    }

    public void OpenWindow(Window windowID) => m_WindowsList[windowID].gameObject.SetActive(true);
    public void OpenOverlay(Overlay overlayID)
    {
        UIWindow overlay = m_OverlaysList[overlayID];
        overlay.gameObject.SetActive(true);
        overlay.transform.SetAsLastSibling();
    }

    public void ChangeWindow(Window windowID, bool changeOnlyData = false)
    {
        if (!changeOnlyData) m_CurrentWindow.gameObject.SetActive(false);
        m_PreviousWindow = m_CurrentWindow;
        m_CurrentWindow = m_WindowsList[windowID];
        if (!changeOnlyData) m_CurrentWindow.gameObject.SetActive(true);
    }

    public void BackToPrevious(bool changeOnlyData = false)
    {
        if (!changeOnlyData)
        {
            m_CurrentWindow.gameObject.SetActive(false);
            m_PreviousWindow.gameObject.SetActive(true);
        }
        UIWindow tmp = m_CurrentWindow;
        m_CurrentWindow = m_PreviousWindow;
        m_PreviousWindow = tmp;
    }
}

public enum Window
{
    AppBoot,
    Main,
    HUD,
    Tickets
}

public enum Overlay
{
    Questions,
    Settings,
    Profile
}
