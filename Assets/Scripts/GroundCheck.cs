using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Controller player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            player.isOnTower = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tower")
        {
            player.isOnTower = false;
        }
    }
}
