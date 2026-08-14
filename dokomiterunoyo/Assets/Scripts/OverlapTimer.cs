using UnityEngine;

public class OverlapTimer : MonoBehaviour
{
    [SerializeField] private OverlapTarget target;
    // = 2.0fに任意のフレーム数で変更
    [SerializeField] private float requiredTime = 2.0f;
    private bool previousbool = false;

    private float timer = 0f;
    public bool EventTriggered{ get; private set; }


    private void Update()
    {
        if(target.IsOverlapping)
        {
            timer += Time.deltaTime;

            if(timer >= requiredTime)
            {
                EventTriggered = true;
            }
        }
        else
        {
            timer = 0f;
            EventTriggered = false;

        }
        if (EventTriggered && !previousbool)
        {
            Debug.Log("２フレーム重なりました");
        }
        previousbool = EventTriggered;
    }
}
