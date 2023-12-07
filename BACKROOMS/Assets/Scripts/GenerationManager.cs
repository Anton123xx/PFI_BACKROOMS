using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GenerationState
{
    Idle,
    GeneratingRooms,
    GeneratingLighting,
    GeneratingSpawn,
    GeneratingExit,
    GeneratingBarrier
}


public class GenerationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform WorldGrid;

    [SerializeField] List<GameObject> RoomTypes;

    [SerializeField] List<GameObject> LightTypes;

    [SerializeField] int mapSize = 16; //the square root needs to be int

    [SerializeField] Slider MapSizeSlider, EmptinessSlider, BrightnessSlider;

    [SerializeField] Button GenerateButton;

    [SerializeField] GameObject E_Room; //empty room

    [SerializeField] GameObject B_Room; //barrier

    [SerializeField] GameObject SpawnRoom, ExitRoom;

    public List<GameObject> GeneratedRooms;

    [SerializeField] GameObject PlayerObject, MainCameraObject;


    [Header("Settings")]
    public int mapEmptiness; //chance of empty room spawning in

    public int mapBrightness; //chance of empty room spawning in

    private int mapSizeSquare;

    private float currentPosX, currentPosZ, currentPosTracker, currentRoom;

    public float roomSize = 7;

    private Vector3 currentPos;

    public GenerationState currentState;



    private void Update()
    {
        mapSize = (int)Mathf.Pow(MapSizeSlider.value, 4);

        mapSizeSquare = (int)Mathf.Sqrt(mapSize);

        mapEmptiness = (int)EmptinessSlider.value;

        mapBrightness = (int)BrightnessSlider.value;
    }
    public void ReloadWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void GenerateWorld()
    {

        for (int i = 0; i < mapEmptiness; i++)
        {
            RoomTypes.Add(E_Room);
        }

        GenerateButton.interactable = false;

        for (int state = 0; state < 6; state++)
        {
            for (int i = 0; i < mapSize; i++)
            {

                if (currentPosTracker == mapSizeSquare)
                {
                    if(currentState == GenerationState.GeneratingBarrier)//right of map
                    {
                        GenerateBarrier();
                    }
                    currentPosX = 0;
                    currentPosTracker = 0;

                    currentPosZ += roomSize;
                    if (currentState == GenerationState.GeneratingBarrier)//left of map
                    {
                        GenerateBarrier();
                    }
                }

                currentPos = new(currentPosX, 0, currentPosZ);

                switch (currentState)
                {
                    case GenerationState.GeneratingRooms:
                        GeneratedRooms.Add(Instantiate(RoomTypes[Random.Range(0, RoomTypes.Count)], currentPos, Quaternion.identity, WorldGrid));
                        break;
                    case GenerationState.GeneratingLighting:

                        int lightSpawnChance = Random.Range(-1, mapBrightness);

                        if (lightSpawnChance == 0)
                        {
                            Instantiate(LightTypes[Random.Range(0, LightTypes.Count)], currentPos, Quaternion.identity, WorldGrid);
                        }
                        break;
                    case GenerationState.GeneratingBarrier:
                
                        if(currentRoom <= mapSizeSquare && currentRoom >= 0)
                        {
                            GenerateBarrier(); //bottom
                        }

                        if(currentRoom <= mapSize && currentRoom >= mapSize - mapSizeSquare)
                        {
                            GenerateBarrier(); //top
                        }
                        break;

                }


                currentRoom++;
                currentPosTracker++;
                currentPosX += roomSize;
            }
            NextState();

            switch(currentState)
            {
                case GenerationState.GeneratingSpawn:

                    int _roomToReplace = Random.Range(0, GeneratedRooms.Count);


                    spawnRoom = Instantiate(SpawnRoom, GeneratedRooms[_roomToReplace].transform.position, Quaternion.identity, WorldGrid);

                    Destroy(GeneratedRooms[_roomToReplace]);

                    GeneratedRooms[_roomToReplace] = spawnRoom;


                    break;
                case GenerationState.GeneratingExit:

                    int roomToReplace = Random.Range(0, GeneratedRooms.Count);


                    GameObject exitRoom = Instantiate(ExitRoom, GeneratedRooms[roomToReplace].transform.position, Quaternion.identity, WorldGrid);

                    Destroy(GeneratedRooms[roomToReplace]);

                    GeneratedRooms[roomToReplace] = exitRoom;
                    break;
            }
        }



    }
    public GameObject spawnRoom;
    public void SpawnPlayer()
    {

        PlayerObject.SetActive(false);

        PlayerObject.transform.position = new Vector3(spawnRoom.transform.position.x, 1.8f, spawnRoom.transform.position.z);

        PlayerObject.SetActive(true);
        MainCameraObject.SetActive(false);  
    }

    public void NextState()
    {
        currentState++;//nextstate

        //resets
        currentPosX = 0;
        currentPosZ = 0;
        currentPosTracker = 0;
        currentPos = Vector3.zero;
        currentRoom = 0;
    }



    public void WinGame()
    {
        MainCameraObject.SetActive(true);
        PlayerObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("WINNNN");
    }


    public void GenerateBarrier()
    {
        currentPos = new(currentPosX, 0, currentPosZ);

        Instantiate(B_Room, currentPos, Quaternion.identity, WorldGrid);

        
    }
}
