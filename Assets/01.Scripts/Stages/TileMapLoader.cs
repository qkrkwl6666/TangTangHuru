using UnityEngine;
using UnityEngine.AddressableAssets;

public class TileMapLoader : MonoBehaviour
{

    void Start()
    {
        int stageNum = GameManager.Instance.CurrentStage;
        if (stageNum == 2 || stageNum == 3)
        {
            stageNum = 1;
        }
        else if (stageNum == 5 || stageNum == 6)
        {
            stageNum = 4;
        }
        else if (stageNum == 8)
        {
            stageNum = 7;
        }
        else if (stageNum == 10)
        {
            stageNum = 9;
        }
        else if (stageNum == 12)
        {
            stageNum = 11;
        }
        else if (stageNum == 14)
        {
            stageNum = 13;
        }
        else if (stageNum == 16 || stageNum == 17)
        {
            stageNum = 15;
        }
        else if (stageNum == 19 || stageNum == 20)
        {
            stageNum = 18;
        }
        else if(stageNum == 22)
        {
            stageNum = 21;
        }
        else if (stageNum == 23 || stageNum == 24)
        {
            stageNum = 25;
        }
        else if (stageNum == 26 || stageNum == 27)
        {
            stageNum = 28;
        }
        else if (stageNum == 29)
        {
            stageNum = 30;
        }

        Addressables.LoadAssetAsync<GameObject>($"Grounds_Stage_{stageNum}").Completed +=
            (obj) =>
            {
                var currGrounds = obj.Result;
                Instantiate(currGrounds, transform);
            };

        Addressables.LoadAssetAsync<GameObject>($"Objects_Stage_{stageNum}").Completed +=
            (obj) =>
            {
                var currObjects = obj.Result;
                Instantiate(currObjects, transform);
            };
    }

}
