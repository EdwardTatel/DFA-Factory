using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseDFA : MonoBehaviour
{
    // variables
    [SerializeField]  private GameObject currentObject; 
    [SerializeField] private GameObject previousObject;
    [SerializeField] private List<GameObject> connectedTransitions = new List<GameObject>();
    [SerializeField] private GameObject transitionObject;
    [SerializeField] private int connectedLetter0;
    [SerializeField] private int connectedLetter1;
    [SerializeField] private SideConnectRestrictions stateCheck;
    [SerializeField] private bool traverse = false;
    private string[] tags = { "Transition", "Loop", "TransitionExtension", "State" };

    // setters and getters
    public GameObject CurrentObject
    {
        get { return currentObject; }
        set { currentObject = value; }
    }
    public GameObject PreviousObject
    {
        get { return previousObject; }
        set { previousObject = value; }
    }
    public bool TraverseStart
    {
        get { return traverse; }
        set { traverse = value; }
    }
    
    //initialization of variables
    void Start()
    {
        connectedLetter0 = 0;
        connectedLetter1 = 0;
    }

    private void Update()
    {

        if (currentObject != null)
        {
            connectedTransitions = currentObject.GetComponent<DFAObject>().ConnectedObjects;
        }
        // Makes the objects in the DFA light up, when traversing
        foreach (string tag in tags)
        {
            // Find all game objects with the current tag
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

            // Loop through each tagged object
            foreach (GameObject obj in taggedObjects)
            {
                Animator animator = obj.GetComponent<Animator>();
                if (obj != currentObject)
                {
                    // Disable glow animator if present

                    if (animator != null)
                    {
                        animator.SetBool("Glow", false);
                    }
                }
                else animator.SetBool("Glow", true);
            }
        }


    }

    // Code to traverse from object to object in the DFA
    public void Traverse()
    {
        // Checks first if DFA is null then show error box
        if (currentObject != null && currentObject.transform.Find("OutCollider") != null &&
         currentObject.transform.Find("OutCollider").GetComponent<SideConnectRestrictions>() != null &&
         currentObject.transform.Find("OutCollider").GetComponent<SideConnectRestrictions>().OtherObject != null &&
         currentObject.transform.Find("OutCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent != null &&
         currentObject.transform.Find("OutCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent.name.StartsWith("FinalState")
         && currentObject.name.StartsWith("TransitionExtensionStart"))
        {
            GetComponent<UIManager>().MoveToPosition(4);
        }
        else if (previousObject != null && previousObject.name == "Start" && stateCheck.ThisColliderHit == false)
        {
            GetComponent<UIManager>().MoveToPosition(3);
        }
        else if(previousObject != null && currentObject!=null)
        {
            traverse = true;


            //Check if there are more than the alphabet connected to a state, if so then show error box
            if (currentObject.CompareTag("State"))
            {
                
                foreach (GameObject obj in connectedTransitions)
                {
                    if (obj.name == "BaseCollider")
                    {
                        if ((int)obj.transform.parent.GetComponent<DFATransition>().myLetter == 0)
                        {
                            connectedLetter0++;
                        }
                        else if ((int)obj.transform.parent.GetComponent<DFATransition>().myLetter == 1)
                        {
                            connectedLetter1++;
                        }
                    }
                }
                if (connectedLetter0 == 1 && connectedLetter1 == 1)
                {
                    foreach (GameObject obj in connectedTransitions)
                    {
                        if (obj.name == "BaseCollider" && (int)obj.transform.parent.GetComponent<DFATransition>().myLetter == GetComponent<StringManager>().CurrentLetter)
                        {
                            MoveToNextObject(obj);
                            return;
                        }
                    }
                }
                else
                {
                    Debug.Log("Kulang or Sobra");
                    GetComponent<UIManager>().MoveToPosition(0);
                    ResetTraverse();
                }
            }

            // Checks if transitions are placed appropriately in the DFA
            else if (currentObject.name.StartsWith("Transition"))
            {
                if (connectedTransitions.Count == 1)
                {
                    ResetTraverse();
                    GetComponent<UIManager>().MoveToPosition(5);
                }
                else {
                    foreach (GameObject obj in connectedTransitions)
                    {
                        if (obj.name == "BaseCollider")
                        {
                            MoveToNextObject(obj);
                            return;
                        }
                        else if (obj.name == "OutCollider")
                        {
                            if (obj.transform.parent == null || obj.transform.parent.gameObject != previousObject)
                            {
                                MoveToNextObject(obj);
                                if (obj.transform.parent.CompareTag("State"))
                                {
                                    if (previousObject.name != "TransitionExtensionStart") GetComponent<StringManager>().MoveToNextLetter(obj.transform.parent.gameObject);
                                }
                                return;
                            }
                        }
                    }
                
                }
                
            }
            else if (currentObject.CompareTag("Loop"))
            {
                Debug.Log("move");
                MoveToNextObject(connectedTransitions[0]);
                if (connectedTransitions[0].transform.parent.CompareTag("State")) GetComponent<StringManager>().MoveToNextLetter(connectedTransitions[0].transform.parent.gameObject);
                return;
            }
        }
    }
    // Moves to check next object inserted in the method.
    private void MoveToNextObject(GameObject Object)
    {
            previousObject = currentObject;
            currentObject = Object.transform.parent.gameObject;
            connectedLetter1 = 0;
            connectedLetter0 = 0;
            StartCoroutine(WaitAndExecute());
        
    }

    IEnumerator WaitAndExecute()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0.5f);

        // Enables the traversal of created DFA
        if(traverse) Traverse();
    }

    // Resets all variables of DFA
    public void ResetTraverse()
    {
        connectedLetter0 = 0;
        connectedLetter1 = 0;
        traverse = false;
        currentObject = GameObject.FindGameObjectWithTag("TransitionExtension");
        previousObject = GameObject.Find("Start");
        
        GetComponent<StringManager>().Iterator = 0;
        GetComponent<StringManager>().CurrentLetter = GetComponent<StringManager>().InputStringArray[GetComponent<StringManager>().TestCaseIterator][GetComponent<StringManager>().Iterator];


        for (int i = 0; i <GetComponent<StringManager>().DFAHolderObjects.Count; i++)
        {
            Animator animator = GetComponent<StringManager>().DFAHolderObjects[i].GetComponent<Animator>();
            if (i == 0)
            {
                animator.SetBool("Glow", true);
            }
            else if (animator != null)
            {
                animator.SetBool("Glow", false);
            }
        }
    }
    // Deletes DFA created
    // Deletes the initial state of the DFA, then every object connected to it gets deleted.
    public void DeleteDFA()
    {
        Destroy(GameObject.FindGameObjectWithTag("State"));
    }
}
