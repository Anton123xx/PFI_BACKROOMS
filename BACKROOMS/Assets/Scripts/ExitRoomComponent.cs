using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoomComponent : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GenerationManager generationManager = FindObjectOfType<GenerationManager>();
    }
}
