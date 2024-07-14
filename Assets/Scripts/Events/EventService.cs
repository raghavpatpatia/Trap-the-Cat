public class EventService
{
    public EventController<TileController> OnTileClick;
    public EventController OnCatMove;
    public EventService()
    {
        OnTileClick = new EventController<TileController>();
        OnCatMove = new EventController();
    }
}