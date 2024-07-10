using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
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
            DontDestroyOnLoad(gameObject);
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
            Debug.LogError("Failed to load DamageText prefab.");
        }
    }

    public void ShowDamage(float damage, Vector3 targetPos)
    {
        if (textObjects.Count < 10)
        {
            var newText = Instantiate(textObject, targetPos, Quaternion.identity);
            newText.GetComponent<TextMeshPro>().text = (damage / 1).ToString();
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
                return;
            }
        }


    }

}
