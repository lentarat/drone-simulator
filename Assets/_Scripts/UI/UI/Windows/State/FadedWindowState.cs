using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FadedWindowState : WindowState
{
    private float _fadeTime = .25f;
    private CanvasGroup _canvasGroup;

    public FadedWindowState(BaseWindow baseWindow) : base(baseWindow)
    {
    }

    public override async UniTask HandleOpen()
    {
        if (BaseWindow.TryGetComponent<CanvasGroup>(out _canvasGroup))
        {
            _canvasGroup.alpha = 0f;
            var tweener = _canvasGroup.DOFade(1f, _fadeTime)
                .SetUpdate(true)
                .SetLink(BaseWindow.gameObject);
            await tweener.ToUniTask();
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
            var tweener = _canvasGroup.DOFade(0f, _fadeTime)
                .SetUpdate(true)
                .SetLink(BaseWindow.gameObject);
            await tweener.ToUniTask();
        }
        else
        {
            Debug.LogError(GetType() + " canvasGroup component not found");
        }
    }
}
