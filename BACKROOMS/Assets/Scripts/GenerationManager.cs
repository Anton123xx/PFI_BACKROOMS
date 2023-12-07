using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GenerationManager : MonoBehaviour
{
    [SerializeField] Transform WorldGrid;

    [SerializeField] List<GameObject> RoomTypes;

    [SerializeField] int mapSize = 16; //the square root needs to be int

    [SerializeField] Slider MapSizeSlider;

    [SerializeField] Button GenerateButton;

    [SerializeField] GameObject E_Room; //empty room

    public int mapEmptiness; //chance of empty room spawning in

    private int mapSizeSquare;

    public float roomSize = 7;

    private Vector3 currentPos;

    private float currentPosX, currentPosZ, currentPosTracker;

    


    private void Update()
    {
        mapSize = (int) Mathf.Pow(MapSizeSlider.value, 4);
        mapSizeSquare = (int)Mathf.Sqrt(mapSize);
    }
    public void ReloadWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void GenerateWorld()
    {

        for(int i=0; i< mapEmptiness; i++)
        {
            RoomTypes.Add(E_Room);
        }

        GenerateButton.interactable = false;

        for(int i = 0; i < mapSize; i++)
        {

            if(currentPosTracker == mapSizeSquare)
            {
                currentPosX = 0;
                currentPosTracker = 0;

                currentPosZ += roomSize;

            }
            currentPos = new(currentPosX, 0, currentPosZ);
            Instantiate(RoomTypes[Random.Range(0, RoomTypes.Count)],currentPos, Quaternion.identity, WorldGrid);


            currentPosTracker++;
            currentPosX += roomSize;
        }

    }




    
}
