using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadedWindowState : WindowState
{
    public FadedWindowState(BaseWindow baseWindow) : base(baseWindow)
    {
    }

    public override void HandleOpen()
    {

        if (BaseWindow.TryGetComponent<Image>(out Image image))
        {
            //Color newColor = image.color;
            //newColor.a = 0.5f;
            //image.color = newColor;
            image.DOFade(1f, 1f);
        }
        else
        {
            Debug.LogError(GetType() + " image component not found");
        }
    }

    public override void HandleClose()
    {
        if (BaseWindow.TryGetComponent<Image>(out Image image))
        {
            //Color newColor = image.color;
            //newColor.a = 0.5f;
            //image.color = newColor;
            image.DOFade(0f, 1f);
        }
        else
        {
            Debug.LogError(GetType() + " image component not found");
        }
    }
}
