using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class ButtonManager : IDisposable
{
    private EventService eventService;
    public ButtonManager(EventService eventService)
    {
        this.eventService = eventService;
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        eventService.OnRetryButtonClick.AddListener(OnRetryButtonClick);
        eventService.OnQuitButtonClick.AddListener(OnQuitButtonClick);
    }
    private void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UnsubscribeEvents()
    {
        eventService.OnRetryButtonClick.RemoveListener(OnRetryButtonClick);
        eventService.OnQuitButtonClick.RemoveListener(OnQuitButtonClick);
    }

    public void Dispose() => UnsubscribeEvents();
}