using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnapObject : MonoBehaviour
{
    [SerializeField] private bool isSnappable = false;
    [SerializeField] List<GameObject> sideColliders = new List<GameObject>();
    public bool IsSnappable
    {
        get { return isSnappable; }
        set { isSnappable = value; }
    }
    public List<GameObject> SideColliders
    {
        get { return sideColliders; }
        set { sideColliders = value; }
    }

    private GameObject colliderHit;
    public int transitionStateConnectionCount;
    [SerializeField] List<GameObject> connectedObjects;
    [SerializeField] private bool foundTarget;
    [SerializeField] private bool connectedCheck = false;
    [SerializeField] private GameObject outColliderObject;

    public bool ConnectedCheck
    {
        get { return connectedCheck; }
        set { connectedCheck = value; }
    }
    private void Start()
    {
        connectedCheck = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            sideColliders.Add(transform.GetChild(i).gameObject);

        }
    }
    private void Update()
    {
        if (GetComponent<DFAObject>() != null) connectedObjects = GetComponent<DFAObject>().ConnectedObjects;
        else if (GetComponent<DFATransition>() != null) connectedObjects = GetComponent<DFATransition>().ConnectedObjects;
        foreach (GameObject collider in sideColliders)
        {
            if (collider != null && collider.GetComponent<SideConnectRestrictions>() != null && collider.GetComponent<SideConnectRestrictions>().ThisColliderHit)
            {
                colliderHit = collider;
            }

        }
    }
    private void FixedUpdate()
    {
        if (connectedCheck)
        {

            if (connectedObjects == null) { Destroy(gameObject); }
            else
            {
                if (!foundTarget)
                {
                    foreach (GameObject obj in connectedObjects)
                    {
                        if (obj != null)
                        {

                            if (obj.name == "OutCollider")
                            {
                                foundTarget = true;
                                outColliderObject = obj;
                                break;
                            }
                        }
                    }
                }
                if (!foundTarget || outColliderObject == null)
                {
                    Destroy(gameObject);
                }

            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isSnappable)
        {

            if (colliderHit.transform.parent.CompareTag("Transition"))
            {

                if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject != null &&
                    transform.Find("OutCollider").GetComponent<SideConnectRestrictions>().OtherObject != null)
                {
                    if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent.name.StartsWith("State") &&
                    transform.Find("OutCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent.CompareTag("State"))
                    {
                        CheckProperRotationBeforeSnap(other);
                    }
                    else Destroy(this.gameObject);
                }
                else if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject != null)
                {
                    if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent.CompareTag("State"))
                    {

                        CheckProperRotationBeforeSnap(other);
                    }
                    else Destroy(this.gameObject);

                }
                else Destroy(this.gameObject);
            }
            else if (colliderHit.transform.parent.CompareTag("TransitionExtension"))
            {
                var baseCollider = transform.Find("BaseCollider");

                if (baseCollider != null)
                {
                    var sideConnectRestrictions = baseCollider.GetComponent<SideConnectRestrictions>();

                    if (sideConnectRestrictions != null)
                    {
                        if(sideConnectRestrictions.OtherObject != null) { 
                        var otherObjectTransform = sideConnectRestrictions.OtherObject?.transform;

                        if (otherObjectTransform != null && otherObjectTransform.parent != null)
                        {
                            if (otherObjectTransform.parent.name == "TransitionExtensionStart")
                            {
                                Destroy(gameObject);
                            }
                        }
                        }
                    }
                }

                if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject != null &&
                    transform.Find("OutCollider").GetComponent<SideConnectRestrictions>().OtherObject != null)
                {
                    if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent.name.StartsWith("Transition") &&
                        transform.Find("OutCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent.CompareTag("State"))
                    {
                        CheckProperRotationBeforeSnap(other);
                    }
                    else Destroy(this.gameObject);
                }
                else if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject != null)
                {

                    if (transform.Find("BaseCollider").GetComponent<SideConnectRestrictions>().OtherObject.transform.parent.name.StartsWith("Transition"))
                    {
                        CheckProperRotationBeforeSnap(other);
                    }
                    else Destroy(this.gameObject);

                }
                else Destroy(this.gameObject);
            }
            else if (colliderHit.transform.parent.CompareTag("State"))
            {
                if (other.transform.parent.CompareTag("Transition") || other.transform.parent.CompareTag("TransitionExtension"))
                {
                    CheckProperRotationBeforeSnap(other);
                }
                else Destroy(this.gameObject);

            }
            else if (colliderHit.transform.parent.CompareTag("Loop"))
            {
                if (other.transform.parent.CompareTag("State"))
                {
                    CheckProperRotationBeforeSnap(other);
                }
                else Destroy(this.gameObject);

            }
        }
        
    }

    private void CheckProperRotationBeforeSnap(Collider2D other)
    {
        if (ShouldDestroyGameObject("RightCollider", "LeftCollider")) Destroy(gameObject);
        if (ShouldDestroyGameObject("LeftCollider", "RightCollider")) Destroy(gameObject);
        if (ShouldDestroyGameObject("TopCollider", "BotCollider")) Destroy(gameObject);
        if (ShouldDestroyGameObject("BotCollider", "TopCollider")) Destroy(gameObject);
        Vector2 snapPosition = Vector2.zero;
        if (other.CompareTag("RightCollider")) { snapPosition = (Vector2)other.transform.position + Vector2.right * 1.75f; }
        else if (other.CompareTag("LeftCollider")) { snapPosition = (Vector2)other.transform.position + Vector2.right * -1.75f; }
        else if (other.CompareTag("TopCollider")) { snapPosition = (Vector2)other.transform.position + Vector2.up * 1.75f; }
        else if (other.CompareTag("BotCollider")) { snapPosition = (Vector2)other.transform.position + Vector2.up * -1.75f; }
        transform.position = snapPosition;
        isSnappable = false;
    }

    private bool ShouldDestroyGameObject(string colliderTagToFind, string otherColliderTag)
    {
        Transform foundChild = FindChildByTag(colliderTagToFind);
        if (foundChild != null)
        {
            SideConnectRestrictions scr = foundChild.GetComponent<SideConnectRestrictions>();
            if (scr != null && scr.OtherObject != null)
            {
                if (!scr.OtherObject.CompareTag(otherColliderTag))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Transform FindChildByTag(string tag)
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return null;
    }
}