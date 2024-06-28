using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour /*, IBeginDragHandler, IDragHandler, IEndDragHandler */
{
    //public GameObject canvas;
    private bool isDragging = false;
    private bool isOverDropZone = false;
    //private GameObject startParent;
    private Vector2 startPosition;
    private GameObject currentDropZone;

    

    /*private void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
    }
    */

    void Update()
    {
        if (isDragging)
        { 
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        currentDropZone = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        isOverDropZone = false;
        currentDropZone = null;
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(currentDropZone.transform, false);
        }
        else
        {
            transform.position = startPosition;
        }
    }
    /*

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        isDragging = true;
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = eventData.position;
        transform.position = position;
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
        if (collision.gameObject.CompareTag("DropZone") || collision.gameObject.CompareTag("PlayerArea"))
        {
            currentDropZone = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DropZone") || collision.gameObject.CompareTag("PlayerArea"))
        {
            if (currentDropZone == collision.gameObject)
            {
                currentDropZone = null;
            }
        }
    }*/
}