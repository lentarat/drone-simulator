using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ApplyChangesButtonHighlighter
{
    private bool _isHighlightningApplyButton;
    private float _highlightShowTime = 0.3f;
    private float _highlightScaleMultiplier = 1.1f;
    private Color _highlightColor = Color.grey;
    private Button _applyChangesButton;

    public ApplyChangesButtonHighlighter(Button applyChangesButton)
    {
        _applyChangesButton = applyChangesButton;
    }

    public void HighlightButton()
    {
        HighlightButtonAsync().Forget();
    }

    private async UniTask HighlightButtonAsync()
    {
        if (_isHighlightningApplyButton)
            return;

        try
        {
            _isHighlightningApplyButton = true;

            Image applyButtonImage = _applyChangesButton.GetComponent<Image>();
            if (applyButtonImage)
            {
                RectTransform applyButtonRectTransform = applyButtonImage.GetComponent<RectTransform>();
                float duration = _highlightShowTime * 0.5f;
                Color defaultColor = applyButtonImage.color;

                var colorTween = applyButtonImage.DOColor(_highlightColor, duration);
                var scaleTween = applyButtonRectTransform.DOScale(_highlightScaleMultiplier, duration);

                await UniTask.WhenAll(colorTween.ToUniTask(), scaleTween.ToUniTask());

                colorTween = applyButtonImage.DOColor(defaultColor, duration);
                scaleTween = applyButtonRectTransform.DOScale(1f, duration);

                await UniTask.WhenAll(colorTween.ToUniTask(), scaleTween.ToUniTask());
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        finally
        {
            _isHighlightningApplyButton = false;
        }
    }
}
