using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] private float speed, rotSpeed;

    private Vector3 targetPosition;
    private Vector3 lookAtTarget;
    private Quaternion playerRot;
    private bool isMoving;

    private void Awake()
    {
        targetPosition = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
        }
        if (isMoving)
        {
            Move();
        }
    }

    private void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, 1000f))
        {
            targetPosition = new Vector3(raycastHit.point.x, raycastHit.point.y+0.5f, raycastHit.point.z);
            lookAtTarget = new Vector3(
                targetPosition.x - transform.position.x,
                transform.position.y,
                targetPosition.z - transform.position.z
                );
            playerRot = Quaternion.LookRotation(lookAtTarget);
            isMoving = true;
        }
    }

    private void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, rotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }
}
