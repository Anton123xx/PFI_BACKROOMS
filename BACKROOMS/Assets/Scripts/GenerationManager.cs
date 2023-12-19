using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GenerationState
{
    Idle,
    GeneratingRooms,
    GeneratingLighting,
    GeneratingSpawn,
    GeneratingExit,
    GeneratingBarrier,
    GeneratingPatrolRooms
}


public class GenerationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform WorldGrid;

    [SerializeField] List<GameObject> RoomTypes;

    [SerializeField] List<GameObject> LightTypes;

    [SerializeField] List<GameObject> patrolRooms;

    [SerializeField] int mapSize = 16; //the square root needs to be int

    [SerializeField] Slider MapSizeSlider, EmptinessSlider, BrightnessSlider;

    [SerializeField] Button GenerateButton;

    [SerializeField] GameObject E_Room; //empty room

    [SerializeField] GameObject B_Room; //barrier

    [SerializeField] GameObject SpawnRoom, ExitRoom;

    public List<GameObject> GeneratedRooms;

    [SerializeField] GameObject PlayerObject, playerCanvas, MainCameraObject, MonsterObject;

    [SerializeField] GameObject surface;

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

        for (int state = 0; state < 7; state++)
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

                    SpawnMonster(exitRoom);
                    break;
                case GenerationState.GeneratingPatrolRooms:


                    foreach(GameObject room in patrolRooms)
                    {
                        bool foundValidRoom = false;
                        int __roomToReplace;
                        do
                        {
                            __roomToReplace = Random.Range(0, GeneratedRooms.Count);
                            if (GeneratedRooms[__roomToReplace].tag != "Exit" && GeneratedRooms[__roomToReplace].tag != "Spawn")
                            {

                                foundValidRoom = true;
                            }
                        }
                        while (!foundValidRoom);
                        
                        GameObject patrolRoom = Instantiate(room, GeneratedRooms[__roomToReplace].transform.position, Quaternion.identity, WorldGrid);

                        Destroy(GeneratedRooms[__roomToReplace]);

                        GeneratedRooms[__roomToReplace] = patrolRoom;
                    }

                  
                    break;
            }
        }

        ///BuildNavMesh();

    }
    public GameObject spawnRoom;
    public GameObject monsterSpawnRoom;//exit
    public void SpawnPlayer()
    {

       

        PlayerObject.SetActive(false);

        PlayerObject.transform.position = new Vector3(spawnRoom.transform.position.x, 1.8f, spawnRoom.transform.position.z);
        playerCanvas.SetActive(true);
        PlayerObject.SetActive(true);
        MainCameraObject.SetActive(false);



        //spawn de monster
        //SpawnMonster();
        //BuildNavMesh();
        //Activates monster
        MonsterObject.SetActive(true);
    }

    public void BuildNavMesh()
    {

        surface.transform.position = MonsterObject.transform.position;
        surface.SetActive(true);
        surface.GetComponent<NavMeshSurface>().BuildNavMesh();

    }


    public void SpawnMonster(GameObject exitRoom)
    {

        //MonsterObject.SetActive(false);
        Debug.Log(exitRoom.transform.position.x);
      
        MonsterObject.transform.position = new Vector3(exitRoom.transform.position.x, 1.8f, exitRoom.transform.position.z);


        ////buids navmesh for monster
        BuildNavMesh();

       
 
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
