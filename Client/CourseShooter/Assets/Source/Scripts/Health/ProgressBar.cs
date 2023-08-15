using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _progress;

    private void Awake()
    {
        SetVisiable(true);
    }

    public void SetValue(float progress, bool canHide = false)
    {
        if(canHide == true)
        {
            if (progress >= 1)
                SetVisiable(false);
            else
                SetVisiable(true);
        }

        _progress.fillAmount = progress;
    }

    public void SetColor(Color color)
    {
        _progress.color = color;
    }

    private void SetVisiable(bool state)
    {
        _progress.enabled = state;
        _background.enabled = state;
    }
}
