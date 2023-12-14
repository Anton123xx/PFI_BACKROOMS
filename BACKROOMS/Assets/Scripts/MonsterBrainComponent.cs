using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBrainComponent : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed;
    //[SerializeField] float time;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator animator;


    private void Start()
    {
        //navMeshAgent.destination = player.transform.position;

        navMeshAgent.speed = speed;
    }

    private void Update()
    {

        if(animator.GetBool("CHASE"))
        {
            navMeshAgent.destination = player.transform.position;
        }
        




    }

}
