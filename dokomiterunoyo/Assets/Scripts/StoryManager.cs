using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
     [Header("最初に再生するStoryData")]
    [SerializeField] private StoryData startStory;


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
    public int textIndex {get; private set;}

    private bool finishText = false;

    private void Start()
    {
        storyText.text = "";
        characterName.text = "";
        choicePanel.SetActive(false);

        currentStory = startStory;
        textIndex = 0;
        SetStoryElement(textIndex);
    }

    private void Update()
    {
        //選択肢中はエンター判定しない
        if(choicePanel.activeSelf) return;
        
        
        if(Input.GetKeyDown(KeyCode.Return) && finishText)
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

        //storyText.text = storyElement.StoryText;
        StartCoroutine(TypeSentence(_textIndex));
    }

    private void ProgressionStory()
    {
        if (textIndex < currentStory.stories.Count)
        {
            SetStoryElement(textIndex);
        }
        else
        {
            //シーン変更、選択肢を出す、別のScriptableObjectを呼ぶ
            EndOfStory();
        
        }
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

            var next = choice.NextStory;   // ループ内で受けるのが重要
            button.onClick.AddListener(() => ChangeStoryElement(next));
        }
    }

    // 生成済みの選択肢ボタンを全部消す
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