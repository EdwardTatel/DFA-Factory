using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideConnectRestrictions : MonoBehaviour
{

    [SerializeField] private bool thisColliderHit = false;
    [SerializeField] private GameObject otherObject;
    public GameObject OtherObject
    {
        get { return otherObject; }
        set { otherObject = value; }
    }
    public bool ThisColliderHit
    {
        get { return thisColliderHit; }
        set { thisColliderHit = value; }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OtherObject = other.gameObject;
        ThisColliderHit = true;
    }
    private void OnTriggerExit2D()
    {
        OtherObject = null;
        ThisColliderHit = false;
    }

    public void Update()
    {
        if (OtherObject == null)
        {
            ThisColliderHit = false;
        }
    }

}
