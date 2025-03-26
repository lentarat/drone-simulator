using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FadedWindowState : WindowState
{
    private CanvasGroup _canvasGroup;

    public FadedWindowState(BaseWindow baseWindow) : base(baseWindow)
    {
    }

    public override void HandleOpen()
    {
        if (BaseWindow.TryGetComponent<CanvasGroup>(out _canvasGroup))
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(1f, 1f).SetLink(BaseWindow.gameObject);
        }
        else
        {
            Debug.LogError(GetType() + " canvasGroup component not found");
        }
    }

    public override async UniTask HandleClose()
    {
        if (BaseWindow.TryGetComponent<CanvasGroup>(out _canvasGroup))
        {
            _canvasGroup.alpha = 1f;
            await _canvasGroup.DOFade(0f, 1f).SetLink(BaseWindow.gameObject).ToUniTask();
        }
        else
        {
            Debug.LogError(GetType() + " canvasGroup component not found");
        }
    }
}
