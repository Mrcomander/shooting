using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{

    [SerializeField] public int MainLoveMeter = 1000;
    public bool head = false;
    public bool body = false;
    public bool leg = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        //重複チェック
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //シーン間でGameManagerオブジェクトを保持
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //好感度履歴リストを作成
    private readonly List<int> loveMeterHistory = new List<int>();
    public IReadOnlyList<int> LoveMeterHistory => loveMeterHistory;

    public void RecordLoveMeter()
    {
        loveMeterHistory.Add(MainLoveMeter);
    }

    


    private void Start()
    {
        OverlapTimer[] timers = FindObjectsOfType<OverlapTimer>();
        foreach (var timer in timers)
        {
            timer.OnOverlapCompleted += HandleBloodEffect;
        }
    
    }

    

    private void HandleBloodEffect(BodyPart part)
    {
        switch(part)
        {
            case BodyPart.Head:
                Debug.Log("頭の判定True");
                head = true;
                break;

            case BodyPart.Body:
                Debug.Log("胴体の判定True");
                body = true;
                break;

            case BodyPart.Leg:
                Debug.Log("足の判定True");
                leg = true;
                break;
        }
    }


    //立ち絵の状態保持

    public string CurrentPortrait { get; private set; }

    
    public void SetPortraitState(
        string portrait)
    {
        CurrentPortrait = portrait;
    }
}