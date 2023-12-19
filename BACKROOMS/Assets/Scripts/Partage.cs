using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partage : MonoBehaviour
{
    public static PlayerController playerController;
    public static WinSceneScript winSceneScript;
    private static bool boolTimer = false;
    private static float timerTemps = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(boolTimer)
        {
            timerTemps += Time.deltaTime;
        }
    }

    public static void startTimer()
    {
        boolTimer = true;
    }
    public static float stopTimer()
    {
        boolTimer= false;
        return timerTemps;
    }
    public static float getTimer()
    {
        return timerTemps;
    }
}
