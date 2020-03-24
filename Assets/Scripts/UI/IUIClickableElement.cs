using System;
using UnityEngine.EventSystems;

public abstract class UIClickableElement<TData> : MonoBehaviorHost, IPointerClickHandler
{
    public Action<UIClickableElement<TData>> OnClick { get; set; }

    public TData Data { get; protected set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(OnClick != null)
            OnClick(this);
    }
}