using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetTransform;
    public float zPos = -10;
    Vector3 newPos = Vector3.zero;

    void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        newPos.x = targetTransform.position.x;
        newPos.y = targetTransform.position.y;
        newPos.z = zPos;

        transform.position = newPos;
    }
}
