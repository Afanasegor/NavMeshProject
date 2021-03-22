using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float force;
    private bool isActive = true;
    private Vector3 startPosition; // field to know where the bullet started from

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
            enemy.MakePhysical();
            Vector3 direction = FindDirection(collision.transform.position, startPosition);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction*force);            
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Method to FindDirection, where force the enemy
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private Vector3 FindDirection(Vector3 a, Vector3 b)
    {
        // we need constraint, to not depend on distance
        float constraintMin = -1;
        float constraintMax = 1;

        Vector3 direction = new Vector3(a.x - b.x, constraintMax, a.z - b.z); // direction.y is always 1;

        if (direction.x > constraintMax)
            direction.x = constraintMax;
        else if (direction.x < constraintMin)
            direction.x = constraintMin;        

        if (direction.z > constraintMax)
            direction.z = constraintMax;
        else if (direction.z < constraintMin)
            direction.z = constraintMin;
        return direction;
    }
}