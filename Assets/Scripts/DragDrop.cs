using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject canvas;
    private bool isDragging = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject currentDropZone;

    private void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        isDragging = true;
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.GetComponent<Camera>(),
            out position
        );
        transform.localPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (currentDropZone != null)
        {
            transform.SetParent(currentDropZone.transform, false);
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DropArea") || collision.gameObject.CompareTag("PlayerArea"))
        {
            currentDropZone = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentDropZone)
        {
            currentDropZone = null;
        }
    }
}
