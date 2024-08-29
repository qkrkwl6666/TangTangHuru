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
    private static readonly string waveCollectionAddString = "��ü ���̺� �߰�";

    private static readonly string waveIdString = "���̺� ���̵�";
    // private static readonly string waveAddString = "���� ���̺� �߰�";
    // private static readonly string monsterSelectString = "���� ����";

    private bool WaveCreateButtonShow = false;

    private WaveCollection waveCollection = new WaveCollection();

    // ���̺� ���� ����
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
