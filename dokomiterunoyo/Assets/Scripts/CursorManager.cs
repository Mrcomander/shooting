using UnityEngine;
using UnityEngine.InputSystem;

public class UICursorFollower : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // 親の Canvas コンポーネントを取得
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Mouse.current == null || parentCanvas == null) return;

        // New Input System からマウスのスクリーン座標を取得
        Vector2 screenPos = Mouse.current.position.ReadValue();

        // スクリーン座標を Canvas のローカル座標系に変換
        RectTransform canvasRect = parentCanvas.transform as RectTransform;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera,
                out Vector2 localPos))
        {
            // 変換後の座標を anchoredPosition に代入（ScaleWithScreenSize等の影響を打ち消す）
            rectTransform.anchoredPosition = localPos;
        }
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}