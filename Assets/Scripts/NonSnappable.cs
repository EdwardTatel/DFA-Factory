using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSnappable : MonoBehaviour
{
    void Update()
    {
        GetComponent<SnapObject>().IsSnappable = false;
    }

}
