using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScore : MonoBehaviour
{
    [SerializeField]
    private Animation _animation;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private float _halfHideAlpha = 0.35f;

    public void Hide()
    {
        if (_canvasGroup.alpha > 0.0f)
        {
            _animation.Play();
        }
    }

    public void Show()
    {
        if (_animation.isPlaying)
        {
            _animation.Stop();
        }
        _canvasGroup.alpha = 1.0f;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void HalfHide()
    {
        Show();
        _canvasGroup.alpha = _halfHideAlpha;
    }
}
