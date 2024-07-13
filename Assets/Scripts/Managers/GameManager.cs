using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridView gridView;
    [SerializeField] private GridSO gridSO;

    private GridController gridController;
    private CommandInvoker commandInvoker;
    private void Initialize()
    {
        commandInvoker = new CommandInvoker();
        gridController = new GridController(gridView, gridSO, commandInvoker);
    }
    private void Start()
    {
        Initialize();
    }
}