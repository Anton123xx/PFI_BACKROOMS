using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumpscare : MonoBehaviour
{
    public string scenename;
    public float jumpscareTimer;
    public AudioSource jumpScareSound;
    public GameObject jumpScareCam;
    public GameObject player;
    public Transform jumpScareCamPos;

    void Start()
    {
        jumpscareTimer = 0.9f;
        jumpScareCam.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
            StartCoroutine("JumpScarePlayer");
    }

    IEnumerator JumpScarePlayer()
    {
        jumpScareCam.transform.position = jumpScareCamPos.position + new Vector3(0.5f,0,1);
        jumpScareSound.Play();
        jumpScareCam.SetActive(true);
        player.SetActive(false);
        yield return new WaitForSeconds(jumpscareTimer);
        SceneManager.LoadScene(scenename);
    }

}
