using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridView gridView;
    [SerializeField] private GridSO gridSO;
    [SerializeField] private CatView catView;
    [SerializeField] private GameOverPanelView gameOverPanelView;
    [SerializeField] private Button ResetButton;

    private EventService eventService;
    private GridController gridController;
    private CommandInvoker commandInvoker;
    private CatController catController;
    private ButtonManager buttonManager;
    private GameOverPanelController gameOverPanelController;
    private void Initialize()
    {
        eventService = new EventService();
        commandInvoker = new CommandInvoker();
        gridController = new GridController(gridView, gridSO, commandInvoker, eventService);
        catController = new CatController(catView, new Vector2Int(5, 5), gridController, commandInvoker, eventService);
        buttonManager = new ButtonManager(eventService);
        gameOverPanelController = new GameOverPanelController(gameOverPanelView, eventService);
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
        ResetButton.onClick.AddListener(OnResetButtonClick);
    }

    private void OnResetButtonClick() => eventService.OnRetryButtonClick.Invoke();

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
            StartCoroutine(WinConditionCoroutine());
        }
    }

    private void CheckforLoseCondition()
    {
        Vector2Int catPosition = catController.CatModel.CurrentPosition;
        TileController currentTile = gridController.GetTile(catPosition.x, catPosition.y);

        if (currentTile == catController.CurrentTargetTile || gridController.GetBoundaryTiles().Contains(currentTile))
        {
            StartCoroutine(LoseConditionCoroutine());
        }

    }

    private IEnumerator LoseConditionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        eventService.OnGameLost.Invoke();
    }

    private IEnumerator WinConditionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        eventService.OnGameWon.Invoke();
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