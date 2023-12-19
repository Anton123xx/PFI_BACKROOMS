using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoomComponent : MonoBehaviour
{
    public GameObject MenuWin;
    public GameObject Player;
    public GameObject Cam;
    private void Start()
    {
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Time.timeScale = 0;
            Partage.stopTimer();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            MenuWin.SetActive(true);
            Cam.SetActive(true);
            Player.SetActive(false);
            SceneManager.LoadScene("WinScene");
            //Partage.winSceneScript.faire();
        }
        //afficher menu mort
    }

    public void restart()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene("WinScene");
        SceneManager.LoadScene("Procedural");
    }
}
