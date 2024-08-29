using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveData2
{
    public string monsterId;
    public string spawnType;
    public int duration;
}

public class WaveCollection
{
    public Dictionary<int, List<List<WaveData2>>> waveData;
}

[CustomEditor(typeof(WaveManager))]
[CanEditMultipleObjects]
public class WaveEditor : Editor
{
    private static readonly string waveCollectionAddString = "전체 웨이브 추가";

    private static readonly string waveIdString = "웨이브 아이디";
    // private static readonly string waveAddString = "현재 웨이브 추가";
    // private static readonly string monsterSelectString = "몬스터 선택";

    private bool WaveCreateButtonShow = false;

    private WaveCollection waveCollection = new WaveCollection();

    // 웨이브 관련 변수
    public int waveId;

    private string[] ConditionTypeStrings = new string[]
    {
        "MonsterType1", "MonsterType2", "MonsterType3",
    };

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        waveId = EditorGUILayout.IntField(waveIdString, waveId);

        if (GUILayout.Button(waveCollectionAddString))
        {
            WaveCreateButtonShow = true;
        }

        if(WaveCreateButtonShow)
        {

        }


    }

    public void InitializeWaveVariables()
    {
        waveId = 0;
    }
}
