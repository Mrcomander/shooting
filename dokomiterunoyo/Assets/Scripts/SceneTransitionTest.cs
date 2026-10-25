using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransitionTest : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string targetSceneName = "ResultScene";

    [Header("Input Settings")]
    [SerializeField] private KeyCode mainKey = KeyCode.R;
    [SerializeField] private bool requireShift = true;
    [SerializeField] private bool requireCtrl = false;
    [SerializeField] private bool requireAlt = false;

    [Header("On-Screen Instructions")]
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private string instructionPrefix = "結果画面へ移動";

    private void Start()
    {
        UpdateInstructionText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(mainKey) && IsModifierCombinationPressed())
        {
            LoadTargetScene();
        }
    }

    private bool IsModifierCombinationPressed()
    {
        bool shiftPressed =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        bool ctrlPressed =
            Input.GetKey(KeyCode.LeftControl) ||
            Input.GetKey(KeyCode.RightControl);

        bool altPressed =
            Input.GetKey(KeyCode.LeftAlt) ||
            Input.GetKey(KeyCode.RightAlt);

        return shiftPressed == requireShift
            && ctrlPressed == requireCtrl
            && altPressed == requireAlt;
    }

    private void LoadTargetScene()
    {
        if (string.IsNullOrWhiteSpace(targetSceneName))
        {
            Debug.LogError("移行先のシーン名が設定されていません。", this);
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(targetSceneName))
        {
            Debug.LogError(
                $"シーン '{targetSceneName}' を読み込めません。" +
                "シーン名とビルド設定を確認してください。",
                this
            );
            return;
        }

        SceneManager.LoadScene(targetSceneName);
    }

    private void UpdateInstructionText()
    {
        if (instructionText == null)
        {
            return;
        }

        string keyCombination = "";

        if (requireCtrl)
        {
            keyCombination += "Ctrl + ";
        }

        if (requireAlt)
        {
            keyCombination += "Alt + ";
        }

        if (requireShift)
        {
            keyCombination += "Shift + ";
        }

        keyCombination += mainKey;

        instructionText.text =
            $"{instructionPrefix}：{keyCombination}";
    }

    private void OnValidate()
    {
        UpdateInstructionText();
    }
}