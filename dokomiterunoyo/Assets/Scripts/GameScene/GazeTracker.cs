using System.Collections.Generic;
using UnityEngine;

// 1回ぶんの「見た」記録
[System.Serializable]
public class GazeRecord
{
    public BodyPart part;
    public float duration;

    public GazeRecord(BodyPart part, float duration)
    {
        this.part = part;
        this.duration = duration;
    }
}

// 今の場面で、どこを何秒見たかを記録する係
public class GazeTracker : MonoBehaviour
{
    [SerializeField] private List<GazeRecord> history = new List<GazeRecord>();

    public bool IsLooking { get; private set; }
    public BodyPart CurrentPart { get; private set; }
    public float CurrentDuration { get; private set; }
    public IReadOnlyList<GazeRecord> History => history;

    private readonly List<OverlapTimer> watchingTimers = new List<OverlapTimer>();

    // 新しい立ち絵を渡されたら、記録をリセットして、その当たり判定を見張り始める
    public void Begin(GameObject portrait)
    {
        foreach (var timer in watchingTimers)
        {
            if (timer != null)
            {
                timer.OnEnter -= HandleEnter;
                timer.OnExit -= HandleExit;
            }
        }
        watchingTimers.Clear();

        history.Clear();
        IsLooking = false;
        CurrentDuration = 0f;

        foreach (var timer in portrait.GetComponentsInChildren<OverlapTimer>())
        {
            timer.OnEnter += HandleEnter;
            timer.OnExit += HandleExit;
            watchingTimers.Add(timer);
        }
    }

    private void Update()
    {
        if (IsLooking)
        {
            CurrentDuration += Time.deltaTime;
        }
    }

    private void HandleEnter(BodyPart part)
    {
        IsLooking = true;
        CurrentPart = part;
        CurrentDuration = 0f;
    }

    private void HandleExit(BodyPart part)
    {
        if (!IsLooking || part != CurrentPart) return;

        history.Add(new GazeRecord(part, CurrentDuration));
        Debug.Log($"{part}を{CurrentDuration:F2}秒見た（{history.Count}件目）");

        IsLooking = false;
        CurrentDuration = 0f;
    }
}