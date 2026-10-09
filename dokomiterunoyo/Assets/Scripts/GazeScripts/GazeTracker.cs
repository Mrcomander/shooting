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

// 今の場面で、どこを・何秒・何回・どの順番で見たかを記録する係
public class GazeTracker : MonoBehaviour
{
    [SerializeField] private List<GazeRecord> history = new List<GazeRecord>();
    [SerializeField] private List<BodyPart> lookOrder = new List<BodyPart>();

    public bool IsLooking { get; private set; }
    public BodyPart CurrentPart { get; private set; }
    public float CurrentDuration { get; private set; }
    public IReadOnlyList<GazeRecord> History => history;

    private readonly List<OverlapTimer> watchingTimers = new List<OverlapTimer>();

    // 記録を全部消す（場面が変わったときに呼ぶ）
    public void ResetRecords()
    {
        history.Clear();
        lookOrder.Clear();
        IsLooking = false;
        CurrentDuration = 0f;
    }

    // 見張る当たり判定を、渡された立ち絵のものに切り替える（記録は消さない）
    public void Watch(GameObject portrait)
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

        IsLooking = false;
        CurrentDuration = 0f;

        foreach (var timer in portrait.GetComponentsInChildren<OverlapTimer>(true))
        {
            timer.OnEnter += HandleEnter;
            timer.OnExit += HandleExit;
            watchingTimers.Add(timer);
        }
    }

    // その部位を何回見たか
    public int CountOf(BodyPart part)
    {
        int count = 0;

        foreach (var p in lookOrder)
        {
            if (p == part) count++;
        }

        return count;
    }

    // 指定した順番どおりに見たか（間にほかの部位を見ていてもOK）
    public bool HasLookedInOrder(List<BodyPart> order)
    {
        if (order == null || order.Count == 0) return false;

        int next = 0;

        foreach (var p in lookOrder)
        {
            if (p == order[next])
            {
                next++;

                if (next == order.Count) return true;
            }
        }

        return false;
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
        lookOrder.Add(part);
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
