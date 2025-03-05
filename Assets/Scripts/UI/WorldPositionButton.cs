using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldPositionButton : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button btn;

    private Transform targetTransform;
    private Action onClickBtn;
    public void Initialized(Transform targetTransform, Action onClick)
    {
        this.targetTransform = targetTransform;
        this.onClickBtn = onClick;
    }
    private void Update()
    {
        var screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition;
        
        // var viewportPoint = Camera.main.WorldToViewportPoint(targetTransform.position);
        // var distanceFromCenter = Vector2.Distance(viewportPoint, Vector2.one * 0.5f);
        //
        // var show = distanceFromCenter < 0.3f;
        // canvasGroup.alpha = show ? 1f : 0f;
        // btn.interactable = show;
    }

    public void Show(bool show)
    {
        canvasGroup.alpha = show ? 1f : 0f;
        btn.interactable = show;
        Debug.Log("btn show" );
    }

    public void ClickBtn()
    {
        onClickBtn?.Invoke();
    }
}
