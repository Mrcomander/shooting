using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ConditionalStart
{
    [Tooltip("Inspector上でわかりやすくするためのメモ。処理には使われない")]
    public string memo;

    [Tooltip("GameScene側で立てるフラグ名と、一字一句同じ文字列を入れる")]
    public string requiredFlag;

    public StoryData story;
}

public class StoryManager : MonoBehaviour
{
    [Header("開始分岐（上から順に判定。最初に条件を満たしたものを使う）")]
    [SerializeField] private List<ConditionalStart> conditionalStarts = new List<ConditionalStart>();

    [Header("どれにも当てはまらない場合")]
    [SerializeField] private StoryData defaultStory;

    [Header("画面のUI")]
    [SerializeField] private Image background;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("選択肢UI")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private Transform choiceParent;

    private StoryData currentStory;
    public int textIndex { get; private set; }

    private bool finishText = false;
    private Coroutine typingCoroutine;


    private void Start()
    {
        storyText.text = "";
        characterName.text = "";
        choicePanel.SetActive(false);

        currentStory = ResolveStartStory();

        if (currentStory == null)
        {
            Debug.LogError("StoryManager : 開始するStoryDataがありません（条件にもdefaultStoryにも該当なし）");
            return;
        }

        textIndex = 0;
        SetStoryElement(textIndex);
    }

    // StoryFlagStoreを見て、最初に再生するStoryDataを決める
    private StoryData ResolveStartStory()
    {
        foreach (var condition in conditionalStarts)
        {
            if (string.IsNullOrEmpty(condition.requiredFlag))
            {
                Debug.LogWarning($"StoryManager : 条件「{condition.memo}」のrequiredFlagが未設定です");
                continue;
            }

            if (!StoryFlagStore.GetFlag(condition.requiredFlag))
                continue;

            if (condition.story == null)
            {
                Debug.LogWarning($"StoryManager : 条件「{condition.memo}」のStoryDataが未設定です");
                continue;
            }

            return condition.story;
        }

        return defaultStory;
    }

    private void Update()
    {
        if (choicePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Return) && finishText)
        {
            textIndex++;
            ProgressionStory();
        }
    }

    private void SetStoryElement(int _textIndex)
    {
        var storyElement = currentStory.stories[_textIndex];

        background.sprite = storyElement.Background;
        characterImage.sprite = storyElement.CharacterImage;
        characterName.text = storyElement.CharacterName;

        finishText = false;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(_textIndex));
    }

    private void ProgressionStory()
    {
        if (textIndex < currentStory.stories.Count)
            SetStoryElement(textIndex);
        else
            EndOfStory();
    }

    private void EndOfStory()
    {
        switch (currentStory.endType)
        {
            case EndType.NextStory:
                ChangeStoryElement(currentStory.nextStory);
                break;

            case EndType.Choice:
                if (currentStory.choices.Count > 0)
                    ShowChoices();
                else
                    Debug.LogWarning($"{currentStory.name} : endTypeがChoiceですがchoicesが空です");
                break;

            case EndType.End:
                Debug.Log("会話終了");
                break;
        }
    }

    private void ChangeStoryElement(StoryData _next)
    {
        if (_next == null)
        {
            Debug.LogWarning($"{currentStory.name} : 次のStoryDataが設定されていません");
            return;
        }

        currentStory = _next;
        textIndex = 0;

        ClearChoices();
        choicePanel.SetActive(false);
        SetStoryElement(textIndex);
    }

    private void ShowChoices()
    {
        choicePanel.SetActive(true);
        ClearChoices();

        foreach (var choice in currentStory.choices)
        {
            var button = Instantiate(choiceButtonPrefab, choiceParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.ChoiceText;

            var next = choice.NextStory;
            button.onClick.AddListener(() => ChangeStoryElement(next));
        }
    }

    private void ClearChoices()
    {
        foreach (Transform child in choiceParent)
            Destroy(child.gameObject);
    }

    private IEnumerator TypeSentence(int _textIndex)
    {
        storyText.text = "";

        foreach (var letter in currentStory.stories[_textIndex].StoryText.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        finishText = true;
    }
}