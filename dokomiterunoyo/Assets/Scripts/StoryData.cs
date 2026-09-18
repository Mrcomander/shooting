using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndType
{
    NextStory, //会話終わったら次のStoryDataへ
    Choice, //選択肢
    End //終わり
}

[CreateAssetMenu(fileName="New Data" , menuName="StoryData")]

public class StoryData :ScriptableObject
{
    public List<Story> stories = new List<Story>();

    [Header("この会話が終わったときの動作")]
    public EndType endType = EndType.NextStory;

    [Header("NextStory のとき使う")]
    public StoryData nextStory;

    [Header("Choice のとき使う")]
    public List<Choice> choices = new List<Choice>();
}
[System.Serializable]
public class Story
{
    public Sprite Background;
    public Sprite CharacterImage;
    [TextArea]
    public string StoryText;
    public string CharacterName;
}

[System.Serializable]
public class Choice
{
    public string ChoiceText;     // ボタンに表示する文
    public StoryData NextStory;   // 選んだら再生するStoryData
}