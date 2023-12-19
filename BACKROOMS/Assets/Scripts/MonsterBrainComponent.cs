using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBrainComponent : MonoBehaviour
{



    [SerializeField] GameObject player;

    [SerializeField] Animator animator;


    [Header("Movement")]
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] float speed;
    [SerializeField] public float walkSpeed;


    [Header("Patrol nodes")]
    [SerializeField] GameObject[] nodes;
    [SerializeField] GameObject eyes;
    public Ray ray;
    public float maxDistance = 15;
    public LayerMask layerMask;
    int index = 0;
    float guardTime = 3f;
    bool guard = false;


    [Header("Audio")]
    [SerializeField] AudioSource aS;
    [SerializeField] AudioClip startRun, runLoop;

    //[SerializeField] GameObject player;
    //[SerializeField] Transform playerPosition;
    //[SerializeField] PlayerHealthComponent playerHealth;



    private void Awake()
    {

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //playerHealth = player.GetComponent<PlayerHealthComponent>();

        navMeshAgent.speed = speed;
    }



    private void Start()
    {

        index = 0;
        nodes = new GameObject[4];
        nodes[0] = GameObject.FindGameObjectWithTag("N0");
        nodes[1] = GameObject.FindGameObjectWithTag("N1");
        nodes[2] = GameObject.FindGameObjectWithTag("N2");
        nodes[3] = GameObject.FindGameObjectWithTag("N3");

        animator.SetTrigger("MOVE");


    }

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            if (CanSeePlayer())
            {
                Charge();
            }

            if (animator.GetBool("AGRO"))
            {
                navMeshAgent.destination = player.transform.position;


                if (!aS.isPlaying)
                {
                    aS.clip = runLoop;
                    aS.Play();
                }
                else
                    {
                    animator.SetTrigger("CHASE");
                }
            }
            else if (DestinationComplete())
            {
                GoToNextNode();
            }
        }
    }


    public bool DestinationComplete()
    {

        if (navMeshAgent.destination == null)
            return true;
        if(Vector3.Distance(transform.position, navMeshAgent.destination) <= 3)
            return true;
        else return false;
    }




    private void OnTriggerEnter(Collider collider)
    {
        switch (collider.tag)
        {
            case "Player":
                Debug.Log("GameOver");
                break;
            case "N0":
                guard = true;
                StartCoroutine(Wait());
                collider.gameObject.SetActive(false);
                break;
            case "N1":
                guard = true;
                StartCoroutine(Wait());
                collider.gameObject.SetActive(false);
                break;
            case "N2":
                guard = true;
                StartCoroutine(Wait());
                collider.gameObject.SetActive(false);
                break;
            case "N3":
                guard = true;
                StartCoroutine(Wait());
                collider.gameObject.SetActive(false);
                break;
            default: break;
        }
    }



    private void Charge()
    {
        animator.SetBool("AGRO", true);

        StopAllCoroutines();


    }

    //private void CheckRange()
    //{
    //    if ((playerPosition.position - agent.transform.position).magnitude <= 1)
    //    {
    //        //Attack();
    //        animator.SetBool("INRANGE", true);
    //        animator.SetTrigger("ATTACK");
    //    }
    //    else
    //    {
    //        animator.SetBool("INRANGE", false);
    //    }
    //}

    //public void Attack()
    //{

    //    playerHealth.Hit();
    //}

    private bool CanSeePlayer()
    {
        //transform.LookAt(targetTransform);/////////
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * maxDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
        {
            //Debug.Log(hit.collider.tag);
            return hit.collider.CompareTag("Player");
        }

        return false;
    }



    IEnumerator Wait()
    {
        do
        {
            animator.SetTrigger("WAIT");
            navMeshAgent.destination = this.transform.position;
            Debug.Log("WAITING");

            guard = false;
            yield return new WaitForSeconds(guardTime);
        }
        while (guard);

        if (!guard)
        {
            animator.SetTrigger("MOVE");
            GoToNextNode();
            StopCoroutine(Wait());


        }



    }
    private void GoToNextNode()
    {
        Debug.Log("MOVING");

        navMeshAgent.destination = nodes[index].transform.position;
        if (index == 3)
        {
            ResetNodes();
            index = 0;
        }
        else
        {
            index++;
        }

    }

    private void ResetNodes()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            nodes[i].gameObject.SetActive(true);
        }
    }
}

