using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public Vector3 moveVector { get; set; }

    // Update is called once per frame
    public void UpdateBodyPart()
    {
        transform.Translate(moveVector);
    }
}
