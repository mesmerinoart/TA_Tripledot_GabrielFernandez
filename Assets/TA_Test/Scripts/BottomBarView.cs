using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottomBarView : MonoBehaviour
{
    [SerializeField] private Button mapButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button shopButton;

    [SerializeField] private RectTransform highlightBackground;
    [SerializeField] private float selectedScale = 1.2f;
    [SerializeField] private float iconOffsetY = 10f;
    [SerializeField] private float textOffsetY = -20f;

    [SerializeField] private float animationSpeed = 10f;
    [SerializeField] private float bounceHeight = 15f;
    [SerializeField] private float bounceDuration = 0.5f;

    public event Action<string> ContentActivated;
    public event Action Closed;

    private Button currentActive;
    private bool bIsMoving;
    private Vector2 highlightHomePos;
    private float bounceStartTime;
    private bool isBouncing;

    private class ButtonData
    {
        public RectTransform Rect;
        public Vector3 OriginalScale;
        public TextMeshProUGUI Text;
        public Vector2 TextOriginalPos;
        public RectTransform Icon;
        public Vector2 IconOriginalPos;

        public ButtonData(Button button)
        {
            Rect = button.GetComponent<RectTransform>();
            OriginalScale = Rect.localScale;
            Text = button.GetComponentInChildren<TextMeshProUGUI>();
            TextOriginalPos = Text ? Text.rectTransform.anchoredPosition : Vector2.zero;
            if (Text) Text.alpha = 0f;
            Icon = button.transform.Find("Icon")?.GetComponent<RectTransform>();
            IconOriginalPos = Icon ? Icon.anchoredPosition : Vector2.zero;
        }
    }

    private ButtonData[] buttonData;

    private void Start()
    {
        InitializeButtons();
        SetupButtonEvents();
        OnButtonClicked(homeButton, "Home");
    }

    private void InitializeButtons()
    {
        highlightHomePos = highlightBackground.anchoredPosition;

        buttonData = new[]
        {
            mapButton != null ? new ButtonData(mapButton) : null,
            homeButton != null ? new ButtonData(homeButton) : null,
            shopButton != null ? new ButtonData(shopButton) : null
        };

        bounceStartTime = 0f;
        isBouncing = false;
    }

    private void SetupButtonEvents()
    {
        if (mapButton != null)
            mapButton.onClick.AddListener(() => OnButtonClicked(mapButton, "Map"));
        if (homeButton != null)
            homeButton.onClick.AddListener(() => OnButtonClicked(homeButton, "Home"));
        if (shopButton != null)
            shopButton.onClick.AddListener(() => OnButtonClicked(shopButton, "Shop"));
    }

    private void Update()
    {
        if (bIsMoving)
        {
            UpdateHighlight();
        }
        UpdateButtons();
    }

    private void UpdateHighlight()
    {
        Vector2 targetPosition = GetTargetPosition();
        highlightBackground.anchoredPosition = Vector2.Lerp(
            highlightBackground.anchoredPosition,
            targetPosition,
            Time.deltaTime * animationSpeed
        );

        if (Vector2.Distance(highlightBackground.anchoredPosition, targetPosition) < 0.01f)
        {
            highlightBackground.anchoredPosition = targetPosition;
            bIsMoving = false;
        }
    }

    private Vector2 GetTargetPosition()
    {
        if (!currentActive) return highlightHomePos;

        Vector3 worldPos = currentActive.transform.position;
        return new Vector2(
            highlightBackground.parent.InverseTransformPoint(worldPos).x,
            highlightBackground.anchoredPosition.y
        );
    }

    private void UpdateButtons()
    {
        float deltaTime = Time.deltaTime * animationSpeed;

        for (int i = 0; i < buttonData.Length; i++)
        {
            var data = buttonData[i];
            if (data == null || data.Rect == null) continue;

            bool isSelected = data.Rect.GetComponent<Button>() == currentActive;

            UpdateButtonScale(data, isSelected, deltaTime);
            UpdateButtonText(data, isSelected, deltaTime);
        }
    }

    private void UpdateButtonScale(ButtonData data, bool isSelected, float deltaTime)
    {
        Vector3 targetScale = data.OriginalScale;
        Vector2 iconTargetPos = data.IconOriginalPos;

        if (isSelected)
        {
            targetScale *= selectedScale;
            iconTargetPos.y += iconOffsetY;

            if (isBouncing)
            {
                float elapsedTime = Time.time - bounceStartTime;
                if (elapsedTime < bounceDuration)
                {
                    float bounceProgress = elapsedTime / bounceDuration;
                    float bounce = bounceHeight * Mathf.Sin(bounceProgress * Mathf.PI);
                    iconTargetPos.y += bounce;
                }
                else
                {
                    isBouncing = false;
                }
            }
        }

        data.Rect.localScale = Vector3.Lerp(data.Rect.localScale, targetScale, deltaTime);

        if (data.Icon != null)
        {
            data.Icon.anchoredPosition = Vector2.Lerp(
                data.Icon.anchoredPosition,
                iconTargetPos,
                deltaTime
            );
        }
    }

    private void UpdateButtonText(ButtonData data, bool isSelected, float deltaTime)
    {
        if (data.Text == null) return;

        Vector2 textTargetPos = data.TextOriginalPos;
        if (isSelected) textTargetPos.y += textOffsetY;

        data.Text.rectTransform.anchoredPosition = Vector2.Lerp(
            data.Text.rectTransform.anchoredPosition,
            textTargetPos,
            deltaTime
        );

        data.Text.alpha = Mathf.Lerp(data.Text.alpha, isSelected ? 1f : 0f, deltaTime);
    }

    private void OnButtonClicked(Button clickedButton, string contentId)
    {
        if (clickedButton == currentActive)
        {
            currentActive = null;
            Closed?.Invoke();
            isBouncing = false;
        }
        else
        {
            currentActive = clickedButton;
            ContentActivated?.Invoke(contentId);
            bounceStartTime = Time.time;
            isBouncing = true;
        }

        bIsMoving = true;
        highlightBackground.gameObject.SetActive(currentActive != null);
    }
}
