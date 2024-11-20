using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefabToInstantiate;

    private GameObject draggedObject;


    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObject = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
        draggedObject.GetComponent<SnapObject>().ConnectedCheck = false;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        draggedObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;
            draggedObject.GetComponent<SnapObject>().ConnectedCheck = false;
            mousePosition.z = 0f;
            draggedObject.transform.position = mousePosition;
        }
    }
    void Update()
    {
        if (draggedObject != null && Input.GetMouseButtonDown(1) && !draggedObject.CompareTag("State"))
        {
            RotateObjectWithRightClick(draggedObject);
        }
    }

    void RotateObjectWithRightClick(GameObject obj)
    {
        obj.transform.Rotate(Vector3.forward, -90f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            if (draggedObject.CompareTag("State") || draggedObject.CompareTag("TransitionExtension"))
            {
                if (draggedObject.GetComponent<DFAObject>().ConnectedObjects.Count == 0) Destroy(draggedObject);
            }
            else
            {
                if (draggedObject.GetComponent<DFATransition>().ConnectedObjects.Count == 0) Destroy(draggedObject);
            }
            draggedObject.GetComponent<SnapObject>().ConnectedCheck = true;
            draggedObject.GetComponent<SnapObject>().IsSnappable = true;
            draggedObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            if (draggedObject.transform.Find("Text") != null) draggedObject.transform.Find("Text").GetComponent<TextMeshPro>().sortingOrder = -1;
            draggedObject = null;
        }
    }
}