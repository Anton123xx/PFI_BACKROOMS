using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoomComponent : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Time.timeScale = 0;
            Partage.stopTimer();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SceneManager.LoadScene("WinScene");
        }
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Procedural");
    }
}
