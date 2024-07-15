using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    private GameOverPanelController gameOverPanelController;
    private void Start()
    {
        retryButton.onClick.AddListener(OnRetryButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }
    public void Init(GameOverPanelController gameOverPanelController) => this.gameOverPanelController = gameOverPanelController;
    public void SetGameStatus(string Text) => statusText.text = Text;
    public void OnRetryButtonClick() => gameOverPanelController.OnRetryButtonClick();
    public void OnQuitButtonClick() => gameOverPanelController.OnQuitButtonClick();
}