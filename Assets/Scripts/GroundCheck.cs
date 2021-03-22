using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

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