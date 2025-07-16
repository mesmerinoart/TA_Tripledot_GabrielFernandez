using TMPro;
using UnityEngine;

public class AnimatedCounter : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public int targetValue = 9999;
    public float baseSpeed = 50f;
    public float acceleration = 20f;

    private float currentValue = 0f;
    private float currentSpeed;
    private bool finished = false;

    void OnEnable()
    {
        currentValue = 0f;
        currentSpeed = baseSpeed;
        finished = false;

        if (textComponent != null)
            textComponent.text = "0";
    }

    void Update()
    {
        if (finished || textComponent == null) return;

        currentSpeed += acceleration * Time.deltaTime;
        currentValue += currentSpeed * Time.deltaTime;

        if (currentValue >= targetValue)
        {
            currentValue = targetValue;
            finished = true;
        }

        textComponent.text = Mathf.FloorToInt(currentValue).ToString();
    }
}
