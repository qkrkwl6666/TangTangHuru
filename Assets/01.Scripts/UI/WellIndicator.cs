using UnityEngine;

public class WellIndicator : MonoBehaviour
{
    public GameObject wellUIPrefab;
    private GameObject wellUI;
    public Transform target;
    private RectTransform wellUITransform;
    private Camera cemera;
    private float edgeBuffer = 20f;
    public RectTransform canvasRect;

    private void Awake()
    {
        cemera = Camera.main;

        wellUI = Instantiate(wellUIPrefab);

        wellUI.SetActive(false);

        canvasRect = wellUI.GetComponent<RectTransform>();

        foreach (Transform t in wellUI.transform)
        {
            wellUITransform = t.GetComponentInChildren<RectTransform>();
            break;
        }


    }

    private void Start()
    {

    }

    private void Update()
    {
        if (target == null) return;

        Vector3 targetScreenPos = cemera.WorldToScreenPoint(target.position);

        bool isOffscreen = targetScreenPos.x <= 0 || targetScreenPos.y <= 0
            || targetScreenPos.x >= Screen.width || targetScreenPos.y >= Screen.height;

        wellUI.SetActive(isOffscreen);

        if (isOffscreen)
        {
            wellUITransform.anchoredPosition = ClampToCanvas(targetScreenPos);
        }

    }

    Vector2 ClampToCanvas(Vector3 screenPosition)
    {
        Vector2 canvasPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, null, out canvasPosition);

        Vector2 viewportPosition = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
        Vector2 clampedViewportPosition = new Vector2(
            Mathf.Clamp(viewportPosition.x, edgeBuffer / canvasRect.rect.width, 1 - edgeBuffer / canvasRect.rect.width),
            Mathf.Clamp(viewportPosition.y, edgeBuffer / canvasRect.rect.height, 1 - edgeBuffer / canvasRect.rect.height)
        );

        Vector2 clampedScreenPosition = new Vector2(
            clampedViewportPosition.x * canvasRect.rect.width - canvasRect.rect.width * 0.5f,
            clampedViewportPosition.y * canvasRect.rect.height - canvasRect.rect.height * 0.5f
        );

        return clampedScreenPosition;
    }
}
