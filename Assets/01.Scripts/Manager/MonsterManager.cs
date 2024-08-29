using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MonsterManager : MonoBehaviour
{
    private List<GameObject> textObjects = new List<GameObject>();
    private GameObject textObject;

    private static MonsterManager instance = null;
    public static MonsterManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>("DamageText").Completed += OnDamageTextLoaded;
    }

    private void OnDamageTextLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            textObject = handle.Result;
        }
        else
        {
            //Debug.LogError("Failed to load DamageText prefab.");
        }
    }

    public void ShowDamage(int damage, Vector3 targetPos, bool isCritical = false)
    {
        if (textObjects.Count < 100)
        {
            var newText = Instantiate(textObject, targetPos, Quaternion.identity);
            newText.GetComponent<TextMeshPro>().text = (damage / 1).ToString();
            if (isCritical)
            {
                newText.GetComponent<TextMeshPro>().color = Color.red;
            }
            else
            {
                newText.GetComponent<TextMeshPro>().color = Color.yellow;
            }
            textObjects.Add(newText);
            return;
        }

        foreach (var text in textObjects)
        {
            if (!text.activeSelf)
            {
                text.SetActive(true);
                text.transform.position = targetPos;
                text.GetComponent<TextMeshPro>().text = (damage / 1).ToString();
                if (isCritical)
                {
                    text.GetComponent<TextMeshPro>().color = Color.red;
                }
                else
                {
                    text.GetComponent<TextMeshPro>().color = Color.yellow;
                }
                return;
            }
        }
    }

}
