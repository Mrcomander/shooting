using UnityEngine;

public class TouchManege : MonoBehaviour
{
    [SerializeField] private UIOverlapDetector detector;
    [SerializeField] private RectTransform UItarget;
    [SerializeField] private RectTransform UItargeted;


    // Update is called once per frame
    void Update()
    {
        bool overlap = detector.IsOverlap(UItarget,UItargeted);
        Debug.Log(overlap);
    }
}
