public class EventService
{
    public EventController<TileController> OnTileClick;
    public EventController CheckForWinCondition;
    public EventController CheckForLoseCondition;
    public EventService()
    {
        OnTileClick = new EventController<TileController>();
        CheckForWinCondition = new EventController();
        CheckForLoseCondition = new EventController();
    }
}