using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class StringManager : MonoBehaviour
{
    // variables
    [SerializeField] private GameObject dfaHolderPrefab;
    [SerializeField] private GameObject dfaHolderStart;
    [SerializeField] private List<int[]> inputStringArray = new List<int[]>();
    [SerializeField] private int currentLetter;
    [SerializeField] private int iterator;
    Vector3 iteratePositionSpawn;
    [SerializeField] private int testCaseIterator;
    [SerializeField] private int levelNumber;
    [SerializeField] private List<GameObject> dfaHolderObjects;
    [SerializeField] int inputStringArraySerial;
    [SerializeField] GameObject AcceptedOrNotHolder;

    // getters and setters
    public int LevelNumber
    {
        set { levelNumber = value; }
        get { return levelNumber; }
    }
    public int CurrentLetter
    {
        set { currentLetter = value; }
        get { return currentLetter; }
    }
    public int Iterator
    {
        set { iterator = value; }
        get { return iterator; }
    }

    public int TestCaseIterator
    {
        set { testCaseIterator = value; }
        get { return testCaseIterator; }
    }
    public List<int[]> InputStringArray
    {
        set { inputStringArray = value; }
        get { return inputStringArray; }
    }

    public List<GameObject> DFAHolderObjects
    {
        set { dfaHolderObjects = value; }
        get { return dfaHolderObjects; }
    }

    // intialization of variables
    private void Start()
    {
        LevelNumber = 0;
    }


    private void Update()
    {
        // Sets text on the bottom right of the main game scene
        if (testCaseIterator == 1 || testCaseIterator == 3) AcceptedOrNotHolder.GetComponentInChildren<TextMeshPro>().text = "Rejected";
        else AcceptedOrNotHolder.GetComponentInChildren<TextMeshPro>().text = "Accepted";
        dfaHolderObjects.RemoveAll(item => item == null);

        //Start of level
        if (levelNumber > 0) {

            // Initialzes test cases based on selected level
            if (levelNumber == 1)
            {
                inputStringArray.Add(new int[] { 1,0,1,0,0 });
                inputStringArray.Add(new int[] { 1,1,1,1 });
                inputStringArray.Add(new int[] { 1,1,0,0,1,0 });
                inputStringArray.Add(new int[] { 0,1,0,1,0,1 });
                inputStringArray.Add(new int[] { 0,0,0 });
            }
            else if (levelNumber == 2)
            {
                inputStringArray.Add(new int[] { 0,0,1 });
                inputStringArray.Add(new int[] { 1,1,1 });
                inputStringArray.Add(new int[] { 1,0,1,1,0,1 });
                inputStringArray.Add(new int[] { 1,0,1,0 });
                inputStringArray.Add(new int[] { 1,0,1,0,1 });
            }

            else if (levelNumber == 3)
            {
                inputStringArray.Add(new int[] { 1,0,0 });
                inputStringArray.Add(new int[] { 1,1,1 });
                inputStringArray.Add(new int[] { 1,1,0,0 });
                inputStringArray.Add(new int[] { 0,0,0,1 });
                inputStringArray.Add(new int[] { 1,0,1,0,0,0 });
            }

            //Sets variables to initial values

            GetComponent<TraverseDFA>().ResetTraverse();

            iterator = 0;
            testCaseIterator = 0;
            currentLetter = inputStringArray[testCaseIterator][iterator];
            iteratePositionSpawn = dfaHolderStart.transform.position;
            foreach (int item in inputStringArray[testCaseIterator])
            {
                InstantiateAlphabetHolder(item, iteratePositionSpawn);
                iteratePositionSpawn += Vector3.down;
            }
            dfaHolderObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("dfaholder"));
            levelNumber = 0;
        }
        if (dfaHolderObjects != null && dfaHolderObjects.Count > 0)
        {
            // Make the current letter being tested in the test case to glow, if not current letter then disable glow
            if(DFAHolderObjects[iterator].GetComponent<Animator>().GetBool("Glow") == false) DFAHolderObjects[iterator].GetComponent<Animator>().SetBool("Glow", true);
            for (int i = 0; i < dfaHolderObjects.Count; i++)
            {
                // Check if the current object index is not equal to the testCaseIterator
                if (i != iterator)
                {
                    // Disable the "Glow" parameter for letter box
                    if (DFAHolderObjects[iterator].GetComponent<Animator>().GetBool("Glow") == true) dfaHolderObjects[i].GetComponent<Animator>().SetBool("Glow", false);
                }
            }
        }


    }
    // Move to next letter to be tested
    public void MoveToNextLetter(GameObject Object)
    {
        if (iterator < inputStringArray[testCaseIterator].Length - 1)
        {
            iterator++;
            currentLetter = inputStringArray[testCaseIterator][iterator];
        }
        else{

            if (Object.name == "FinalState(Clone)")
            {
                
                GetComponent<TraverseDFA>().TraverseStart = false;

                if (testCaseIterator == 1 || testCaseIterator == 3)
                {
                    StartCoroutine(DeclinedWait(2f));
                }
                else
                {
                    AcceptedOrNotHolder.GetComponent<Animator>().SetBool("Glow", true);
                    StartCoroutine(AcceptedWait(2f));
                }
            }
            else
            {

                GetComponent<TraverseDFA>().TraverseStart = false;

                if (testCaseIterator == 1 || testCaseIterator == 3)
                {
                    AcceptedOrNotHolder.GetComponent<Animator>().SetBool("Glow", true);
                    StartCoroutine(AcceptedWait(2f));
                }
                else
                {
                    StartCoroutine(DeclinedWait(2f));
                }
            }
                
        }
    }
    // Move to next test case to be tested
    public void MoveToNextTestCase()
    {

        // Iterate over each object
        foreach (GameObject obj in dfaHolderObjects)
        {
            Destroy(obj);

        }
        iteratePositionSpawn = dfaHolderStart.transform.position;
        testCaseIterator += 1;
        if (testCaseIterator == 5)
        {
            dfaHolderObjects.RemoveAll(item => item == null);
            inputStringArray.Clear();
            levelNumber = 0;
            testCaseIterator = 0;
            iterator = 0;
            GetComponent<UIManager>().MoveToNextPage(7);
            GetComponent<TraverseDFA>().DeleteDFA();
        }
        else
        {
            iteratePositionSpawn = dfaHolderStart.transform.position;
            foreach (int item in inputStringArray[testCaseIterator])
            {
                InstantiateAlphabetHolder(item, iteratePositionSpawn);

                iteratePositionSpawn += Vector3.down;
            }
            dfaHolderObjects = FindObjectsOfType<GameObject>().Where(obj => obj.name == "dfaholder(Clone)").ToList();
            dfaHolderObjects.Reverse();
        }
    }

    // Reset test case (if DFA failed test cases or initialization of level)
    public void ResetTestCase()
    {
        // Iterate over each object
        foreach (GameObject obj in dfaHolderObjects)
        {
            Destroy(obj);

        }
        dfaHolderObjects.Clear();
        levelNumber = 0;
        testCaseIterator = 0;
        iterator = 0;
        iteratePositionSpawn = dfaHolderStart.transform.position;
        foreach (int item in inputStringArray[testCaseIterator])
        {
            InstantiateAlphabetHolder(item, iteratePositionSpawn);
            iteratePositionSpawn += Vector3.down;
        }
        dfaHolderObjects = FindObjectsOfType<GameObject>().Where(obj => obj.name == "dfaholder(Clone)").ToList();
        dfaHolderObjects.Reverse();

    }
    // Makes the letter boxes/alphabet holders visible
    public void InstantiateAlphabetHolder(int letter, Vector3 positionToSpawn)
    {

        GameObject instantiatedObject = Instantiate(dfaHolderPrefab, positionToSpawn, Quaternion.identity);
        instantiatedObject.GetComponentInChildren<TextMeshPro>().text = letter.ToString();
        transform.position += new Vector3(1, 0, 0);
    }

    // Test case successful code
    IEnumerator AcceptedWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Debug.Log("ACCEPTED!!");
        GetComponent<UIManager>().MoveToPosition(1);
        MoveToNextTestCase();
        if (inputStringArray.Count > 0 && inputStringArray[0]?.Length > 0) GetComponent<TraverseDFA>().ResetTraverse();

        AcceptedOrNotHolder.GetComponent<Animator>().SetBool("Glow", false);
        GetComponent<UIManager>().LetUIAppear = true;
    }

    // Test case unsuccessful code
    IEnumerator DeclinedWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Debug.Log("DECLINED!!!");
        GetComponent<UIManager>().MoveToPosition(2);
        ResetTestCase();
        if (inputStringArray.Count > 0 && inputStringArray[0]?.Length > 0) GetComponent<TraverseDFA>().ResetTraverse();

        AcceptedOrNotHolder.GetComponent<Animator>().SetBool("Glow", false);
        GetComponent<UIManager>().LetUIAppear = true;

    }
}

