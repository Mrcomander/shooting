using UnityEngine;
using UnityEngine.InputSystem;

public class UICursorFollower : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        // 強制的にスクリーン座標をワールド座標（UI座標）に変換して適用
        transform.position = new Vector3(mousePos.x, mousePos.y, 0f);

        // デバッグ用：座標変更を強制的にログ出力
        Debug.Log($"UIカーソル移動成功: {transform.position}");
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}