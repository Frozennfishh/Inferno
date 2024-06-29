using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
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

    void Update()
    {
        if (isDragging)
        {
            Vector2 position = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, position, canvas.GetComponent<Camera>(), out Vector2 localPoint);
            transform.position = canvas.transform.TransformPoint(localPoint);
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

    public void StartDrag()
    {
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        isDragging = true;
        transform.SetParent(canvas.transform, true);
    }

    public void EndDrag()
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
}
