using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class DFAObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> connectedObjects = new List<GameObject>();
    public List<GameObject> ConnectedObjects
    {
        get { return connectedObjects; }
        set { connectedObjects = value; }
    }
    private void Update()
    {
        connectedObjects.RemoveAll(item => item == null);
    }
    // Add parent objects to the list when their children enter trigger stay
    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject parentObject = other.gameObject;

        if (!connectedObjects.Contains(parentObject))
        {
            connectedObjects.Add(parentObject);
        }
    }

    // Remove parent objects from the list when their children exit trigger stay
    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject parentObject = other.gameObject;

        if (connectedObjects.Contains(parentObject))
        {
            connectedObjects.Remove(parentObject);
        }
    }

}