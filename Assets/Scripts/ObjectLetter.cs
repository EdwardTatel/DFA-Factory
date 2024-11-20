using TMPro;
using UnityEngine;
using static DFATransition;

public class ObjectLetter : MonoBehaviour
{
    private TextMeshPro textMeshPro;
    private GameObject manager;
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = "0";
    }
    private void Start()
    {
        manager = GameObject.Find("Manager");
    }
    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }
    void OnMouseDown()
    {
        if (!manager.GetComponent<TraverseDFA>().TraverseStart && manager.GetComponent<StringManager>().TestCaseIterator == 0) {
            if (textMeshPro.text == "0")
            {
                transform.parent.gameObject.GetComponent<DFATransition>().myLetter = Letter.One;
                textMeshPro.text = "1";
            }
            else
            {
                transform.parent.gameObject.GetComponent<DFATransition>().myLetter = Letter.Zero;
                textMeshPro.text = "0";
            }
        }
    }

}
