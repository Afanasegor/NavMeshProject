using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Vector3 targetPosition;

    [SerializeField] private Rigidbody[] allRigidbodies;

    private Animator animator;

    private GameMode gameMode;


    [SerializeField] private Transform armTransform;
    [SerializeField] private Vector3 offset = new Vector3(84.34f, 21.6f, -2.38f);
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform gunTransform;

    [SerializeField] private BulletSpawner bulletSpawner;

    private void Awake()
    {
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = true;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameMode = GameMode.walking;
    }

    private void Update()
    {
        if (gameMode == GameMode.walking)
        {
            if (Input.GetMouseButton(0))
            {                
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    agent.SetDestination(hit.point);
                }
                if (hit.distance != 0)
                {
                    animator.SetBool("Walk", true);
                }
                targetPosition = hit.point;
            }

            if (agent.velocity == Vector3.zero)
            {
                animator.SetBool("Walk", false);
            }
        }

        if (gameMode == GameMode.shooting)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    headTransform.LookAt(hit.point);
                    armTransform.LookAt(hit.point);
                    armTransform.rotation = armTransform.rotation * Quaternion.Euler(offset);
                    gunTransform.LookAt(hit.point);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                bulletSpawner.Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameMode = GameMode.shooting;
            animator.enabled = false;
        }
    }

    private void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);
    }

    private void MakePhysical()
    {
        animator.enabled = false;
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = false;
        }
    }

    enum GameMode : byte
    {
        walking,
        shooting
    }
}
