using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DFATransition : DFAObject
{
    public enum Letter { Zero, One }
    GameObject manager;
    // Private variable to hold the enum value
    [SerializeField] private Letter letter;

    // Getter and setter for the enum variable
    public Letter myLetter
    {
        get { return letter; }
        set { letter = value; }
    }
    private void Start()
    {
    }
}
