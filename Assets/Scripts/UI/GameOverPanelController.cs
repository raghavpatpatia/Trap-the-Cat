using System;

public class GameOverPanelController : IDisposable
{
    private GameOverPanelView gameOverPanelView;
    private EventService eventService;
    public GameOverPanelController(GameOverPanelView gameOverPanelView, EventService eventService)
    {
        this.gameOverPanelView = gameOverPanelView;
        this.gameOverPanelView.Init(this);
        this.eventService = eventService;
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        eventService.OnGameWon.AddListener(OnGameWon);
        eventService.OnGameLost.AddListener(OnGameLose);
    }

    private void OnGameWon()
    {
        gameOverPanelView.gameObject.SetActive(true);
        gameOverPanelView.SetGameStatus("You Won!!");
    }

    private void OnGameLose()
    {
        gameOverPanelView.gameObject.SetActive(true);
        gameOverPanelView.SetGameStatus("You Lose!!");
    }

    public void OnRetryButtonClick()
    {
        gameOverPanelView.gameObject.SetActive(false);
        eventService.OnRetryButtonClick.Invoke();
    }
    public void OnQuitButtonClick() => eventService.OnQuitButtonClick.Invoke();

    private void UnsubscribeEvents()
    {
        eventService.OnGameWon.RemoveListener(OnGameWon);
        eventService.OnGameLost.RemoveListener(OnGameLose);
    }

    public void Dispose() => UnsubscribeEvents();
}