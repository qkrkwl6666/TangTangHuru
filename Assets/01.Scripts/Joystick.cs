using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// ��ġ ���ڽ���� �ӽñ���

public class Joystick : MonoBehaviour
{
    private Vector2 pos;
    public GameObject blackCirclePrefabs;
    private GameObject blackCircle;

    public Vector2 InputValue { get; private set; }
    private Vector3 startScreenPosition = Vector2.zero;
    private Vector3 currentScreenPosition = Vector3.zero;

    // ��ġ �ִ� ����
    private float joystickRadius = 30f;

    private void Awake()
    {
        blackCircle = Instantiate(blackCirclePrefabs);
        blackCircle.SetActive(false);
    }

    public void OnJoyStick(InputAction.CallbackContext context)
    {

        
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        Debug.Log(InputValue);

        if(Touch.activeTouches.Count <= 0)
        {
            InputValue = Vector2.zero;
            return;
        }

        foreach (var touch in Touch.activeTouches)
        {
            startScreenPosition = touch.startScreenPosition;
        }

        var primaryTouchPhase = Touch.activeTouches[0].phase;
        switch (primaryTouchPhase)
        {
            // ��ġ ����
            case TouchPhase.Began:
                blackCircle.SetActive(true);
                {
                    startScreenPosition.z = 10;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(startScreenPosition);
                    blackCircle.transform.position = worldPos;
                }
                break;
            // ��ġ ���� ���
            case TouchPhase.Canceled:
                blackCircle.SetActive(false);
                break;
            // ��ġ ���� ���
            case TouchPhase.Ended:
                blackCircle.SetActive(false);
                break;
            case TouchPhase.Moved:
                {
                    currentScreenPosition = Touch.activeTouches[0].screenPosition;
                    Vector3 worldPos;
                    var dir = currentScreenPosition - startScreenPosition;
                    float distance = (startScreenPosition - currentScreenPosition).magnitude;

                    if (distance > joystickRadius)
                    {
                        dir = dir.normalized * joystickRadius;
                        dir.z = 10;
                        worldPos = Camera.main.ScreenToWorldPoint(startScreenPosition + dir);
                        blackCircle.transform.position = worldPos;

                        InputValue = dir / joystickRadius;
                        
                        return;
                    }

                    InputValue = dir / joystickRadius;

                    currentScreenPosition.z = 10;
                    worldPos = Camera.main.ScreenToWorldPoint(currentScreenPosition);
                    blackCircle.transform.position = worldPos;
                }
                break;
        }


    }
}
