using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitchToggle : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;

    private bool isOn;

    public void ToggleSprite()
    {
        isOn = !isOn;
        targetImage.sprite = isOn ? spriteOn : spriteOff;
    }

    public bool IsOn => isOn;
} 