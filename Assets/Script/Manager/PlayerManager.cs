using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] HandCardManager handCardManager;

    [SerializeField] Vector3 mousePos;
    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;

    }
}
