using UnityEngine;

public class UIOverlapDetector : MonoBehaviour
{
    [SerializeField] private RectTransform UItarget;
    [SerializeField] private RectTransform UItargeted;

    private void OnDrawGizmos()
    {
        if(UItarget != null)
            DrawRect(UItarget, Color.red);

        if(UItargeted != null)
            DrawRect(UItargeted, Color.green);

    }
private void DrawRect(RectTransform rt, Color color)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Gizmos.color = color;

        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);
    } 


    public bool IsOverlap(RectTransform a, RectTransform b)
    {
        if(a == null || b == null)
            return false;

    Vector3[] cornersA = new Vector3[4];
    Vector3[] cornersB = new Vector3[4];

    a.GetWorldCorners(cornersA);
    b.GetWorldCorners(cornersB);

    Rect rectA = new Rect(
        cornersA[0].x,
        cornersA[0].y,
        cornersA[2].x - cornersA[0].x,
        cornersA[2].y - cornersA[0].y);


    Rect rectB = new Rect(
        cornersB[0].x,
        cornersB[0].y,
        cornersB[2].x - cornersB[0].x,
        cornersB[2].y - cornersB[0].y);

    return rectA.Overlaps(rectB);


    


    }
}