using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public BottomBarView bottomBar;
    public TextMeshProUGUI panelStatusText;

    [Header("Win Screen")]
    [SerializeField] private GameObject winScreen;

    void Start()
    {
        bottomBar.ContentActivated += OnContentOpened;
        bottomBar.Closed += OnContentClosed;
        OnContentOpened("Home");

        if (winScreen != null)
            winScreen.SetActive(false);
    }

    void OnContentOpened(string contentId)
    {
        switch (contentId)
        {
            case "Map":
                ShowMapPanel();
                break;
            case "Home":
                ShowHomePanel();
                break;
            case "Shop":
                ShowShopPanel();
                break;
        }
    }

    void OnContentClosed()
    {
        HideAllPanels();
    }

    void ShowMapPanel() => panelStatusText.text = "Map Panel Shown";
    void ShowHomePanel() => panelStatusText.text = "Home Panel Shown";
    void ShowShopPanel() => panelStatusText.text = "Shop Panel Shown";
    void HideAllPanels() => panelStatusText.text = "All Panels Hidden";

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void HideWinScreen()
    {
        if (winScreen != null)
            winScreen.SetActive(false);
    }
}
