using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float force = 5000f;
    private bool isActive = true;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive)
        {
            return;
        }
        isActive = false;
        GetComponent<Rigidbody>().useGravity = true;        

        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemy)
        {
            Debug.Log("Попал во врага");
            enemy.MakePhysical();
            Vector3 direction = FindDirection(collision.transform.position, startPosition);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction*force);            
        }

        Destroy(gameObject);
    }

    private Vector3 FindDirection(Vector3 a, Vector3 b)
    {
        float constraintMin = -1;
        float constraintMax = 1;
        Vector3 direction = new Vector3(a.x - b.x, constraintMax, a.z - b.z);
        if (direction.x > constraintMax)
            direction.x = constraintMax;
        else if (direction.x < constraintMin)
            direction.x = constraintMin;

        //if (direction.y > constraintMax)
        //    direction.y = constraintMax;
        //else if (direction.y < constraintMin)
        //    direction.y = constraintMin;

        if (direction.z > constraintMax)
            direction.z = constraintMax;
        else if (direction.z < constraintMin)
            direction.z = constraintMin;
        return direction;
    }
}
