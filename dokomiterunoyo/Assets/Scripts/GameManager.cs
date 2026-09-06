using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] public int MainLoveMeter = 1000;
    public bool head = false;
    public bool body = false;
    public bool leg = false;

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