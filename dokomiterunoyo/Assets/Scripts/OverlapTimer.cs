using UnityEngine;
using System;
using System.Collections;

public enum BodyPart
{
    Head,
    Body,
    Leg
}

public class OverlapTimer : MonoBehaviour
{
    [SerializeField] private float requiredTime = 2.0f; // 秒数
    [SerializeField] private BodyPart bodyPart; // Inspectorで部位指定

    public event Action<BodyPart> OnOverlapCompleted;

    private Coroutine timerCoroutine;

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Entered");
        StartTimer();
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse Exited");
        StopTimer();
    }

    private void StartTimer()
    {
        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    private void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(requiredTime);
        // 指定時間経過後に合図を送る
        OnOverlapCompleted?.Invoke(bodyPart);
    }
}