using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject LevelSelect;
    [SerializeField] private GameObject Level1;
    [SerializeField] private GameObject Level2;
    [SerializeField] private GameObject Level3;
    [SerializeField] private GameObject LevelClear;
    [SerializeField] private GameObject ItemMenu;
    [SerializeField] private GameObject Background;
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject ItemButton;
    [SerializeField] private float menuPosition;
    [SerializeField] private float UIBackPosition = -3.931196f;

    public bool LetUIAppear
    {
        set { letButtonsAppear = value; }
        get { return letButtonsAppear; }
    }
    private enum UICombination{
        menuToLevelSelect, levelSelectToLevel1, levelSelectToLevel2, levelSelectToLevel3, levelToLevelSelect, endMenu, toMenu, levelClear
    }
    public float positionAX = 11.11973f; // X-coordinate of the first position
    public float positionBX = 5.508046f; // X-coordinate of the second position
    public float moveSpeed = 30f; // Speed of movement

    private bool isMovingToA = true; 
    private bool letButtonsAppear = false;

    [SerializeField]
    private RectTransform[] targetRectTransforms; 

    public Vector2 targetPositionsA = new Vector2(0, -145f);
    public Vector2 targetPositionsB = new Vector2(0, -350f); // Second positions for each UI element

    private bool[] isMoving; 
    [SerializeField] private Vector2[] targetPositions;
    private bool stopDialog = false;

    void Start()
    {
        // Initialize positions arrays
        int count = targetRectTransforms.Length;
        isMoving = new bool[count];
        targetPositions = new Vector2[count];
        UIBackPosition = -3.931196f;


        // Initialize the UI elements' anchored positions to position A
        for (int i = 0; i < count; i++)
        {
            targetPositions[i] = targetPositionsB;
            targetRectTransforms[i].anchoredPosition = targetPositionsB;
        }

        ItemMenu.transform.position = new Vector3(positionAX, ItemMenu.transform.position.y, ItemMenu.transform.position.z);
    }

    void Update()
    {

            StartButton.transform.position = Vector3.Lerp(StartButton.transform.position, new Vector3(StartButton.transform.position.x, UIBackPosition, StartButton.transform.position.z), 5 * Time.deltaTime);
            if (GetComponent<StringManager>().TestCaseIterator == 0) ItemButton.transform.position = Vector3.Lerp(ItemButton.transform.position, new Vector3(ItemButton.transform.position.x, UIBackPosition, ItemButton.transform.position.z), 5f * Time.deltaTime);

        if (!GetComponent<TraverseDFA>().TraverseStart) {

            if (!stopDialog)
            {
                for (int i = 0; i < targetRectTransforms.Length; i++)
                {
                    
                    // Check if moving is required for this UI element
                    if (isMoving[i])
                    {
                        // Move the UI element towards the target position
                        targetRectTransforms[i].anchoredPosition = Vector2.Lerp(targetRectTransforms[i].anchoredPosition, targetPositions[i], moveSpeed * Time.deltaTime);
                        
                        

                        // Check if the UI element has reached close enough to the target position
                        if (Vector2.Distance(targetRectTransforms[i].anchoredPosition, targetPositions[i]) < .05f)
                        {
                            if (targetPositions[i] == new Vector2(0, -145f))StartCoroutine(MoveToNextPositionAfterDelay(i));

                            isMoving[i] = false;
                        }
                        
                    }
                }

            }

            if (isMovingToA && !Mathf.Approximately(ItemMenu.transform.position.x, positionAX))
            {
                ItemMenu.transform.position = Vector3.Lerp(ItemMenu.transform.position, new Vector3(positionAX, ItemMenu.transform.position.y, ItemMenu.transform.position.z), moveSpeed * Time.deltaTime);
            }
            else if (!isMovingToA && !Mathf.Approximately(ItemMenu.transform.position.x, positionBX))
            {
                ItemMenu.transform.position = Vector3.Lerp(ItemMenu.transform.position, new Vector3(positionBX, ItemMenu.transform.position.y, ItemMenu.transform.position.z), moveSpeed * Time.deltaTime);
            }
        }
        else if(GetComponent<TraverseDFA>().TraverseStart)
        {
            isMovingToA = true;
            UIBackPosition = -6f;
            ItemMenu.transform.position = Vector3.Lerp(ItemMenu.transform.position, new Vector3(positionAX, ItemMenu.transform.position.y, ItemMenu.transform.position.z), moveSpeed * Time.deltaTime);

        }
    }
    public void ResetButtonUIPosition()
    {
        for (int i = 0; i < targetPositions.Length; i++)
        {
            targetPositions[i] = targetPositionsB;  
        }
        UIBackPosition = -3.931196f;
    }
    public void StartLevel(int levelNumber)
    {
        Debug.Log("level start" + levelNumber);
        GetComponent<StringManager>().LevelNumber = levelNumber;
    }
    public void MoveToNextPage(int combo)
    {
        UICombination combination = (UICombination)combo;
        if (combination == UICombination.menuToLevelSelect)
        {
            Menu.SetActive(false);
            LevelSelect.SetActive(true);
        }
        else if (combination == UICombination.levelSelectToLevel1)
        {
            LevelSelect.SetActive(false);
            Level1.SetActive(true);
        }
        else if (combination == UICombination.levelSelectToLevel2)
        {
            LevelSelect.SetActive(false);
            Level2.SetActive(true);
        }
        else if (combination == UICombination.levelSelectToLevel3)
        {
            LevelSelect.SetActive(false);
            Level3.SetActive(true);
        }
        else if (combination == UICombination.levelToLevelSelect)
        {
            Level1.SetActive(false);
            Level2.SetActive(false);
            Level3.SetActive(false);
            LevelClear.SetActive(false);
            LevelSelect.SetActive(true);
        }
        else if (combination == UICombination.endMenu)
        {
            Level1.SetActive(false);
            Level2.SetActive(false);
            Level3.SetActive(false);
            Background.SetActive(false);
        }
        else if (combination == UICombination.toMenu)
        {
            LevelClear.SetActive(false);
            LevelSelect.SetActive(false);
            Menu.SetActive(true);
        }
        else if (combination == UICombination.levelClear)
        {
            stopDialog = true;
            LevelClear.SetActive(true);
            Background.SetActive(true);
        }

    }
    public void MoveToPosition(int index)
    {
        stopDialog = false;
        UIBackPosition = -6f;
        isMoving[index] = true;
        targetPositions[index] = targetPositionsA;
    }

    // move UI element with index to the other position after a delay
    private IEnumerator MoveToNextPositionAfterDelay(int index)
    {
        yield return new WaitForSeconds(1f);
        targetPositions[index] = targetPositionsB;
        isMoving[index] = true;

        yield return new WaitForSeconds(1.5f);
        UIBackPosition = -3.931196f;
    }
    public void TogglePosition()
    {
        isMovingToA = !isMovingToA;
    }
}
