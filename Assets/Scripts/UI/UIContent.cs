using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIContent<TUIElement, TDefinition> : MonoBehaviour
    where TUIElement: UIClickableElement<TDefinition>
{
    ObjectPool<TUIElement> elementsPool;
    
    public TUIElement elementPrefab;

    public Image ActiveElementImage;

    public bool ElementsClickable;
    
    bool isInit = false;

    void Start()
    {
        if (!isInit)
            Init();
    }

    void Init()
    {
        elementsPool = new ObjectPool<TUIElement>(elementPrefab, transform);
        isInit = true;
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void SetActive(bool val)
    {
        gameObject.SetActive(val);
    }

    public void Refresh()
    {
        if (!isInit)
            Init();

        elementsPool.SetAllElementsInactive();
        
        foreach (var definition in GetAllDefinitions())
        {
            var element = elementsPool.GetElementFromPool();
            SetupElement(element, definition);

            if (ElementsClickable)
            {
                element.OnClick -= OnClick;
                element.OnClick += OnClick;
            }
        }
    }

    public virtual void OnClick(UIClickableElement<TDefinition> element)
    {
        Debug.Log("Active element is " + element.gameObject.name);
        if (ActiveElementImage != null)
        {
            var rect = element.transform as RectTransform;
            ActiveElementImage.enabled = true;
            ActiveElementImage.rectTransform.sizeDelta = rect.sizeDelta;
            ActiveElementImage.transform.position = element.transform.position;
            ActiveElementImage.transform.parent = element.transform;
        }
    }

    public virtual IEnumerable<TUIElement> GetAllElements()
    {
        return elementsPool.GetAllActiveElements();
    }

    public abstract IEnumerable<TDefinition> GetAllDefinitions();
    protected abstract void SetupElement(TUIElement element, TDefinition definition);
}
