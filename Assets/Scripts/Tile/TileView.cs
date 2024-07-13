using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public TileController TileController { get; private set; }
    public void Init(TileController tileController) => this.TileController = tileController;
    public void ChangeSpriteColor(Color color) => spriteRenderer.color = color;
    private void OnMouseDown() => TileController.OnTileClick();
}
