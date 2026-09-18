using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryData[] storyDatas;

    [SerializeField] private Image Background;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI characterName;

    public int storyIndex {get; private set;}
    public int textIndex {get; private set;}

    private bool finishText = false;

    private void Start()
    {
        storyText.text = "";
        characterName.text = "";
        SetStoryElement(storyIndex, textIndex);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && finishText)
        {
            textIndex++;
            storyText.text = "";
            ProgressionStory(storyIndex);
        }
    }


    private void SetStoryElement(int _storyIndex, int _textIndex)
    {
        var storyElement = storyDatas[_storyIndex].stories[_textIndex];

        Background.sprite = storyElement.Background;
        characterImage.sprite = storyElement.CharacterImage;
        
        characterName.text = storyElement.CharacterName;

        finishText = false;

        //storyText.text = storyElement.StoryText;
        StartCoroutine(TypeSentence(_storyIndex, _textIndex));
    }

    private void ProgressionStory(int _storyIndex)
    {
        if (textIndex < storyDatas[_storyIndex].stories.Count)
        {
            SetStoryElement(storyIndex, textIndex);
        }
        else
        {
            //シーン変更、選択肢を出す、別のScriptableObjectを呼ぶ
            ChangeStoryElement();
        
        }
    }

    private void ChangeStoryElement()
    {
        textIndex = 0;
        storyIndex++;
        SetStoryElement(storyIndex,textIndex);
    }

    private IEnumerator TypeSentence(int _storyIndex, int _textIndex)
    {
        foreach (var letter in storyDatas[_storyIndex].stories[_textIndex].StoryText.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        finishText = true;
    }
}