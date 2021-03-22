using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Settings playerSettings; // create ScriptableObject in Assets, to set up player;

    [HideInInspector] public bool isOnTower = false;

    private Camera mainCamera;
    private NavMeshAgent agent;
    private Vector3 targetPosition;

    [Space]
    // Arrays to switch off players phisics and colliders
    [SerializeField] private Rigidbody[] allRigidbodies;
    [SerializeField] private Collider[] allColliders;
    [Space]

    [Header("Fields for aim enemy")]
    [SerializeField] private Transform enemyTarget;
    [SerializeField] private Transform armTransform;
    [SerializeField] private Vector3 offset = new Vector3(84.34f, 21.6f, -2.38f); // need to normalize hand direction; default settings for this model: (84.34f, 21.6f, -2.38f)
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform gunTransform;

    // Shootings settings
    [SerializeField] private BulletSpawner bulletSpawner;
    private float timeOut;
    private float curTimeout;
    private int powerIndex = 1000;

    private Animator animator;
    private RaycastHit hit; 

    private void Awake()
    {
        // switch off players phisics and colliders
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = true;
            allColliders[i].enabled = false;
        }
    }

    private void Start()
    {
        // caching fields
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Initialize fields from ScriptableObject (playerSettings)
        agent.speed = playerSettings.playerSpeed;
        timeOut = playerSettings.shootingTimeout;
        bulletSpawner.bulletPrefab.GetComponent<Bullet>().force = playerSettings.GetFireMode(playerSettings.fireMode, powerIndex);
    }

    private void Update()
    {
        // Walking GameMode
        if (GameController.singleton.GetGameMode() == GameController.GameMode.walking)
        {
            // Find the point where to go
            if (Input.GetMouseButton(0))
            {                
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    agent.SetDestination(hit.point);
                }
                targetPosition = hit.point;
            }

            // Animation controlling in Walking GameMode
            if (agent.velocity == Vector3.zero)
            {
                animator.SetBool("Walk", false);
            }
            else
            {
                animator.SetBool("Walk", true);
            }

            // Switching GameMode if the player is on Tower and he can aim enemy
            if (isOnTower && !animator.GetBool("Walk") && CheckToSwitchGameMode())
            {
                GameController.singleton.SetGameModeMethod(GameController.GameMode.shooting);
            }
        }
        // Shooting GameMode
        else if (GameController.singleton.GetGameMode() == GameController.GameMode.shooting)
        {
            if (Input.GetMouseButton(0))
            {
                // Find shoot point
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    headTransform.LookAt(hit.point);
                    armTransform.LookAt(hit.point);
                    armTransform.rotation = armTransform.rotation * Quaternion.Euler(offset);
                    gunTransform.LookAt(hit.point);
                }

                // counting shooting Delay
                curTimeout += Time.deltaTime;
                if (curTimeout > timeOut)
                {
                    curTimeout = 0;
                    bulletSpawner.Shoot();
                }
            }
            else
            {
                curTimeout += Time.deltaTime; // counting shooting Delay
            }
        }        
    }

    private Vector3 FindDirectionRay(Vector3 a, Vector3 b)
    {
        Vector3 direction = new Vector3(b.x - a.x, b.y - a.y, b.z - a.z);
        return direction;
    }

    /// <summary>
    /// Checking "can the player aim enemy or not"
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// SetActive weapon, animator controller enable = false; If autoAim -> aim on enemy for the first shoot
    /// </summary>
    public void ReadyForShoot()
    {
        gunTransform.gameObject.SetActive(true);
        animator.enabled = false;
        
        if (GameController.singleton.GetAutoAim())
        {
            headTransform.LookAt(enemyTarget.position);
            armTransform.LookAt(enemyTarget.position);
            armTransform.rotation = armTransform.rotation * Quaternion.Euler(offset);
            gunTransform.LookAt(enemyTarget.position);
        }
    }
}