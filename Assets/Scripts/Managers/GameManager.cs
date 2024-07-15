using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridView gridView;
    [SerializeField] private GridSO gridSO;
    [SerializeField] private CatView catView;

    private EventService eventService;
    private GridController gridController;
    private CommandInvoker commandInvoker;
    private CatController catController;
    private void Initialize()
    {
        eventService = new EventService();
        commandInvoker = new CommandInvoker();
        gridController = new GridController(gridView, gridSO, commandInvoker, eventService);
        catController = new CatController(catView, new Vector2Int(5, 5), gridController, commandInvoker, eventService);
    }
    private void Start()
    {
        Initialize();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        eventService.CheckForWinCondition.AddListener(CheckForWinCondition);
        eventService.CheckForLoseCondition.AddListener(CheckforLoseCondition);
    }

    private void CheckForWinCondition()
    {
        bool won = true;
        foreach (Vector2Int direction in catController.GetDirection())
        {
            Vector2Int tileDirection = direction + catController.CatModel.CurrentPosition;

            if (gridController.IsWithinGridBounds(tileDirection.x, tileDirection.y))
            {
                TileController tile = gridController.GetTile(tileDirection.x, tileDirection.y);

                if (tile.TileModel.TileState != TileState.FILLED)
                {
                    won = false;
                    break;
                }
            }
            else
            {
                won = false; 
                break;
            }
        }

        if (won)
        {
            Debug.Log("You Won");
        }
    }

    private void CheckforLoseCondition()
    {
        Vector2Int catPosition = catController.CatModel.CurrentPosition;
        TileController currentTile = gridController.GetTile(catPosition.x, catPosition.y);

        if (currentTile == catController.CurrentTargetTile || gridController.GetBoundaryTiles().Contains(currentTile))
        {
            Debug.Log("You Lose");
        }

    }

    private void UnsubscribeEvents()
    {
        eventService.CheckForWinCondition.RemoveListener(CheckForWinCondition);
        eventService.CheckForLoseCondition.RemoveListener(CheckforLoseCondition);
    }

    private void OnDestroy()
    {
        catController.Dispose();
        gridController.DisposeTile();
        UnsubscribeEvents();
    }
}