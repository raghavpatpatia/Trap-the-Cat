using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public TileController TileController { get; private set; }
    private EventService eventService;
    public void Init(TileController tileController, EventService eventService)
    {
        this.TileController = tileController;
        this.eventService = eventService;
    }
    public void ChangeSpriteColor(Color color) => spriteRenderer.color = color;
    public Vector2 GetTileCenter() => (Vector2)spriteRenderer.bounds.center;
    private void OnMouseDown() => eventService.OnTileClick.Invoke(TileController);
}
