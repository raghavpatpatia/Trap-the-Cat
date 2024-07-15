public class EventService
{
    public EventController<TileController> OnTileClick;
    public EventController CheckForWinCondition;
    public EventController CheckForLoseCondition;
    public EventController OnRetryButtonClick;
    public EventController OnQuitButtonClick;
    public EventController OnGameWon;
    public EventController OnGameLost;
    public EventService()
    {
        OnTileClick = new EventController<TileController>();
        CheckForWinCondition = new EventController();
        CheckForLoseCondition = new EventController();
        OnRetryButtonClick = new EventController();
        OnQuitButtonClick = new EventController();
        OnGameWon = new EventController();
        OnGameLost = new EventController();
    }
}