public class EventService
{
    public EventController<TileController> OnTileClick;
    public EventService()
    {
        OnTileClick = new EventController<TileController>();
    }
}