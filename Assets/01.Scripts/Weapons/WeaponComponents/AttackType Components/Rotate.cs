using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate : MonoBehaviour
{
    public int burstCount = 0;
    public int index = 0;
    private float angle = 0f;

    float timer = 0f;
    float range = 2.5f;

    IAimer currAimer;
    Transform parentTransform;

    void Awake()
    {
        currAimer = GetComponent<IAimer>();
        parentTransform = currAimer.Player.transform;
    }

    private void OnEnable()
    {
        angle = (360f / currAimer.TotalCount) * currAimer.Index;
    }

    private void OnDisable()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (timer > currAimer.LifeTime)
        {
            gameObject.SetActive(false);
        } 
        else
        {
            float radians = angle * Mathf.Deg2Rad;
             
            float x = parentTransform.position.x + Mathf.Cos(radians) * range;
            float y = parentTransform.position.y + Mathf.Sin(radians) * range;

            Vector3 newPosition = new Vector3(x, y, 0);
            transform.position = newPosition;

            Vector3 direction = (transform.position - parentTransform.position).normalized;
            transform.up = direction;  // 오브젝트가 항상 바깥을 보도록

            angle += currAimer.Speed * Time.deltaTime;
            timer += Time.deltaTime;
        }
    }



}
