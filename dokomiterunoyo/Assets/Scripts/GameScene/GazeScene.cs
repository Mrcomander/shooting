using UnityEngine;
using System.Collections.Generic;

//1つごとの場面設定　１場面=１アセット
[CreateAssetMenu(fileName = "NewGazeScene" , menuName = "Game/Gaze Scene")]
public class GazeScene : ScriptableObject
{
    [Tooltip("この画面で表示する立ち絵（画像＋当たり判定）のPrefab")]
    public GameObject portraitPrefab;

    [Tooltip("上から順に判定。最初に条件を満たした分気が使われるよ")]
    public List<GazeBranch> branches = new List<GazeBranch>();
}