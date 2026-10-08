using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale = new Vector3(2.2f, 2.2f, 2.2f);
    public float speed = 10f;

    private bool isHovered = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        Vector3 goal = isHovered ? targetScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, goal, Time.unscaledDeltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
