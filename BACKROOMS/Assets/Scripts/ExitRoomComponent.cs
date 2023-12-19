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
            Cam.SetActive(true);
            Player.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
            MenuWin.SetActive(true);
        }
        //afficher menu mort
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Procedural");
    }
}
