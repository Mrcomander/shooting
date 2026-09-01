using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] public int MainLoveMeter = 1000;

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
                break;

            case BodyPart.Body:
                Debug.Log("胴体の判定True");
                break;

            case BodyPart.Leg:
                Debug.Log("足の判定True");
                break;
        }
    }

}
