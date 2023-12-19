using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class timerLabel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text txtlbl;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //https://discussions.unity.com/t/show-1-decimal-only-when-printing-a-float/31253
        float timer = Partage.getTimer();
        string temps = timer.ToString("F1");
        txtlbl.text = temps;
        Debug.Log(temps);
    }
}
