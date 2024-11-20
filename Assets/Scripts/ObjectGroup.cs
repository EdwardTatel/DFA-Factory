using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DFATransition;

public class ObjectGroup : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabsButton;

    private void Start()
    {
        tabsButton[0].SetActive(true);
        tabsButton[1].SetActive(false);
        tabsButton[2].SetActive(false);
    }
    void OnMouseDown()
    {
        if(this.gameObject.name == "StateImage")
        {
            tabsButton[0].SetActive(true);
            tabsButton[1].SetActive(false);
            tabsButton[2].SetActive(false);
        }
        else if (this.gameObject.name == "TransitionImage")
        {
            tabsButton[0].SetActive(false);
            tabsButton[1].SetActive(true);
            tabsButton[2].SetActive(false);
        }
        else if(this.gameObject.name == "TransitionExtensionImage")
        {
            tabsButton[0].SetActive(false);
            tabsButton[1].SetActive(false);
            tabsButton[2].SetActive(true);
        }
    }

}
