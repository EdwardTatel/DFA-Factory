using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderRotationManager : MonoBehaviour
{
    private Transform top;
    private Transform bot;
    private Transform left;
    private Transform right;

    [SerializeField] private float rotation;

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "TopCollider") top = child;
            else if (child.tag == "BotCollider") bot = child;
            else if (child.tag == "LeftCollider") left = child;
            else if (child.tag == "RightCollider") right = child;
        }
    }
    private void Update()
    {

        rotation = transform.localEulerAngles.z;

            if (rotation == 0)
            {
                if(top != null) top.tag = "TopCollider";
                if (bot != null) bot.tag = "BotCollider";
                if (right != null) right.tag = "RightCollider";
                if (left != null) left.tag = "LeftCollider";
            }
            else if (rotation == 90)
            {
                if (top != null) top.tag = "LeftCollider";
                if (bot != null) bot.tag = "RightCollider";
                if (right != null) right.tag = "TopCollider";
                if (left != null) left.tag = "BotCollider";
            }
            else if (rotation == 180)
            {
                if (top != null) top.tag = "BotCollider";
                if (bot != null) bot.tag = "TopCollider";
                if (right != null) right.tag = "LeftCollider";
                if (left != null) left.tag = "RightCollider";
            }
            else if (rotation == 270)
            {
                if (top != null) top.tag = "RightCollider";
                if (bot != null) bot.tag = "LeftCollider";
                if (right != null) right.tag = "BotCollider";
                if (left != null) left.tag = "TopCollider";
            }
    }
}
