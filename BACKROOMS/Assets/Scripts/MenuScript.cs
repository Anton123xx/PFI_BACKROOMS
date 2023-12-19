using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{

    [SerializeField] GameObject playMenuGeneration;

    private void Start()
    {
        playMenuGeneration.SetActive(false);
    }

    public void Play()
    {
        playMenuGeneration.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
