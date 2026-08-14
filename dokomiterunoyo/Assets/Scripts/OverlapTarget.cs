using UnityEngine;

public class OverlapTarget : MonoBehaviour
{
    //マウスが重なったとき、離れたときにそれぞれLOGが出ます

    public bool IsOverlapping { get; private set; }

    private void OnMouseEnter()
    {
        IsOverlapping = true;
        Debug.Log("True");
    }

    private void OnMouseExit()
    {
        IsOverlapping = false;
        Debug.Log("False");
    }
}


