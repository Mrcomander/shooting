using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;

public enum BodyPart
{
    Head,
    Body,
    Leg
}

public class OverlapTimer : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BodyPart bodyPart; //Inspectorで部位を指定

    public BodyPart Part => bodyPart;
    
    public event Action<BodyPart> OnEnter;
    public event Action<BodyPart> OnExit;

    private bool isInside = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInside = true;
        Debug.Log($"入った：{bodyPart}");
        OnEnter?.Invoke(bodyPart);

    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Exit();
    }

    public void OnDisable()
    {
        Exit();
    }

    private void Exit()
    {
        if (!isInside) return;

        isInside = false;
        Debug.Log($"出た：{bodyPart}");
        OnExit?.Invoke(bodyPart);
    }
}