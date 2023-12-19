using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;


    public void Quit()
    {
        menu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
