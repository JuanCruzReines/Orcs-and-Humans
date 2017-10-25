using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour {

    public Transform[] points;
    private int destPoint = 0;

    public RacesEnum enemyTag;

    private NavMeshAgent agent;
    private Animator animController;
    private UnitMeleeAttack _attack;

    private GameObject target;

    private bool isPursuing;
    private bool isIdle;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        animController = GetComponent<Animator>();
        _attack = GetComponent<UnitMeleeAttack>();

        isPursuing = false;
        isIdle = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = destPoint + 1;
    }

    void OnEnable()
    {
        destPoint = 0;
    }


    void Update()
    {

        if (isPursuing)
        {
            if (target != null)
            {
                agent.destination = target.transform.position;
                agent.Resume();
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    agent.Stop();
                    faceTarget();
                    _attack.attack(target);
                }
            }
            else
            {
                isPursuing = false;
                agent.destination = points[destPoint].position;
                animController.SetTrigger("run");
                agent.Resume();
            }
        }
        else
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                //If we are close to the last point
                if (points.Length <= destPoint)
                {
                    agent.Stop();
                    if (!isIdle)
                    {
                        animController.SetTrigger("idle");
                        isIdle = true;
                    }
                }
                else
                {
                    GotoNextPoint();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag.ToString()) && !isPursuing)
        {
            target = other.gameObject;
            isPursuing = true;
            agent.destination = target.transform.position;
        }
    }

    void faceTarget()
    {
        Vector3 point = target.transform.position;
        point.y = 0;
        gameObject.transform.LookAt(point);
        
    }

    public void setWalkingPoints(Transform[] walkingPoints)
    {
        points = walkingPoints;
    }


}

