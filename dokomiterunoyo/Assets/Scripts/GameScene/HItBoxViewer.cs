using UnityEngine;

// 当たり判定（BoxCollider2D）の範囲を、Play画面に半透明の四角で表示するデバッグ用スクリプト
[RequireComponent(typeof(BoxCollider2D))]
public class HitBoxDebugView : MonoBehaviour
{
    [SerializeField] private bool show = true;
    [SerializeField] private Color color = new Color(1f, 0.3f, 0.5f, 0.35f);

    private BoxCollider2D box;
    private static Texture2D whiteTex;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();

        if (whiteTex == null)
        {
            whiteTex = new Texture2D(1, 1);
            whiteTex.SetPixel(0, 0, Color.white);
            whiteTex.Apply();
        }
    }

    private void OnGUI()
    {
        if (!show || !box.enabled) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Bounds b = box.bounds;
        Vector3 min = cam.WorldToScreenPoint(b.min);
        Vector3 max = cam.WorldToScreenPoint(b.max);

        // 画面座標は「下が0」、OnGUIは「上が0」なので、上下をひっくり返す
        Rect rect = Rect.MinMaxRect(
            Mathf.Min(min.x, max.x),
            Screen.height - Mathf.Max(min.y, max.y),
            Mathf.Max(min.x, max.x),
            Screen.height - Mathf.Min(min.y, max.y)
        );

        GUI.color = color;
        GUI.DrawTexture(rect, whiteTex);
        GUI.color = Color.white;
        GUI.Label(new Rect(rect.x + 4, rect.y + 2, 300, 20), name);
    }
}
