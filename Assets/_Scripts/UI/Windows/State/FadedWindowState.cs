using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FadedWindowState : WindowState
{
    public FadedWindowState(BaseWindow baseWindow) : base(baseWindow)
    {
    }

    public override void HandleOpen()
    {
        if (BaseWindow.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
        {
            canvasGroup.DOFade(1f, 1f);
        }
        else
        {
            Debug.LogError(GetType() + " canvasGroup component not found");
        }
    }

    public override async UniTask HandleClose()
    {
        if (BaseWindow.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
        {
            //Color newColor = image.color;
            //newColor.a = 0.5f;
            //image.color = newColor;
            await canvasGroup.DOFade(0f, 1f).ToUniTask();
        }
        else
        {
            Debug.LogError(GetType() + " canvasGroup component not found");
        }
    }
}
