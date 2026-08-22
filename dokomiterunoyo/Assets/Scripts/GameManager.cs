using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private OverlapTimer timer;
    [SerializeField] public int MainLoveMeter = 1000;

    public bool TestOverlap{get; private set;}
    private bool previousTestOverlap = false;

    void Update()
    {
        TestOverlap = timer.EventTriggered;

        if(TestOverlap && !previousTestOverlap)
        {
             Debug.Log("ゲームマネージャーでのTrue");
        }


        previousTestOverlap = TestOverlap;
    }

}
