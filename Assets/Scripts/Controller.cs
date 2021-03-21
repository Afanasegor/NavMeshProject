using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    public bool isOnTower = false;
    public bool autoAim;

    private Camera mainCamera;
    private NavMeshAgent agent;
    private Vector3 targetPosition;

    [SerializeField] private Rigidbody[] allRigidbodies;
    [SerializeField] private Collider[] allColliders;

    private Animator animator;

    private GameMode gameMode;

    [SerializeField] private Transform enemyTarget;
    [SerializeField] private Transform armTransform;
    [SerializeField] private Vector3 offset = new Vector3(84.34f, 21.6f, -2.38f);
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform gunTransform;

    [SerializeField] private BulletSpawner bulletSpawner;

    [SerializeField] private float timeOut = 0.2f;
    private float curTimeout;

    private RaycastHit hit; 

    private void Awake()
    {
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = true;
            allColliders[i].enabled = false;
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
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    agent.SetDestination(hit.point);
                }
                //if (hit.distance != 0)
                //{
                //    animator.SetBool("Walk", true);
                //}
                targetPosition = hit.point;
            }

            if (agent.velocity == Vector3.zero)
            {
                animator.SetBool("Walk", false);
            }
            else
            {
                animator.SetBool("Walk", true);
            }

            if (isOnTower && !animator.GetBool("Walk") && CheckToSwitchGameMode())
            {
                Debug.Log("Переключаю режим");
                gameMode = GameMode.switchingMode;
                StartCoroutine(SetGameMode(GameMode.shooting));
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

                curTimeout += Time.deltaTime;
                if (curTimeout > timeOut)
                {
                    curTimeout = 0;
                    bulletSpawner.Shoot();
                }
            }
            else
            {
                curTimeout = timeOut + 1;
            }

            //if (Input.GetMouseButtonDown(0))
            //{
            //    bulletSpawner.Shoot();
            //}
        }

        // TODO: почистить
        Debug.DrawRay(armTransform.position, FindDirectionRay(armTransform.position, enemyTarget.position), Color.red);
        
        // TODO: почистить
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (CheckToSwitchGameMode())
            {
                gameMode = GameMode.shooting;
                animator.enabled = false;
                
                headTransform.LookAt(enemyTarget.position);
                armTransform.LookAt(enemyTarget.position);
                armTransform.rotation = armTransform.rotation * Quaternion.Euler(offset);
                gunTransform.LookAt(enemyTarget.position);
            }
        }
    }    

    private void MakePhysical()
    {
        animator.enabled = false;
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = false;
        }
    }

    private Vector3 FindDirectionRay(Vector3 a, Vector3 b)
    {
        Vector3 direction = new Vector3(b.x - a.x, b.y - a.y, b.z - a.z);
        return direction;
    }

    private bool CheckToSwitchGameMode()
    {
        RaycastHit infoHit;
        if (Physics.Raycast(armTransform.position, FindDirectionRay(armTransform.position, enemyTarget.position), out infoHit))
        {
            EnemyController info = infoHit.collider.gameObject.GetComponentInParent<EnemyController>();
            if (info != null)
                return true;
            else
                return false;
        }        
        else
            return false;
    }

    IEnumerator SetGameMode(GameMode mode)
    {
        yield return new WaitForSeconds(0.5f);
        gunTransform.gameObject.SetActive(true);
        animator.enabled = false;
        gameMode = mode;

        if (autoAim)
        {
            headTransform.LookAt(enemyTarget.position);
            armTransform.LookAt(enemyTarget.position);
            armTransform.rotation = armTransform.rotation * Quaternion.Euler(offset);
            gunTransform.LookAt(enemyTarget.position);
        }
    }
    

    enum GameMode : byte
    {
        walking,
        shooting,
        switchingMode
    }
}
