using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsTabHeaderColorsHandler : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI _headerText;

    [Header("Colors")]
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _pressedColor;

    private bool _isPressed;
    private bool _isPointerOver;

    private void UpdateVisual()
    {
        if (_isPressed)
        {
            _headerText.color = _pressedColor;
        }
        else if (_isPointerOver)
        {
            _headerText.color = _highlightedColor;
        }
        else
        {
            _headerText.color = _normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOver = true;
        UpdateVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOver = false;
        UpdateVisual();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        UpdateVisual();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        UpdateVisual();
    }
}