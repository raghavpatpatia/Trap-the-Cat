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
    }

    private void OnDestroy()
    {
        catController.Dispose();
        gridController.DisposeTile();
    }
}