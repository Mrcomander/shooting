using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private OverlapTimer timer;
    [SerializeField] int MainLoveMeter = 1000;
    [SerializeField] int MainHp = 100;

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
