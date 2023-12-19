using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoomComponent : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //afficher menu mort
    }

    public void restart()
    {
        SceneManager.LoadScene("Procedural");
    }
}
