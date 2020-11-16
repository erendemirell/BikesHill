using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBot : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float dashSpeed;
    [SerializeField] Transform wheelFront;

    bool isFlying;
    float angle;
    float score = 0;
    float fixRate = 1f;

    Quaternion rotCur;
    Quaternion maxAngle = Quaternion.Euler(65f, 0, 0);
    Vector3 control_point;

    Rigidbody rigidbody;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!gameManager.isGameStarted)
        {
            return;
        }

        if (!isFlying)
        {
            rigidbody.velocity = transform.forward * speed;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.fixedDeltaTime * 10f);

            if (transform.rotation.x > 0)
            {
                speed += Time.fixedDeltaTime * 5f;
            }
        }
        else
        {
            Dash();
        }
    }

    private void Dash()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, maxAngle, 0.25f * Time.fixedDeltaTime);
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(wheelFront.position, -wheelFront.up, out hit, 1.3f))
        {
            Debug.DrawRay(wheelFront.position, -wheelFront.up, Color.blue);

            rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            isFlying = false;
        }
        else if (Physics.Raycast(wheelFront.position, wheelFront.forward, out hit, 1.75f))
        {
            angle = Vector3.Angle(hit.normal, wheelFront.forward);
            Debug.DrawRay(hit.point, hit.normal * 10, Color.yellow, 0);

            rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            isFlying = false;
        }
        else
        {
            isFlying = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            control_point.z = other.gameObject.transform.position.z;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            score = transform.position.z - control_point.z;
            rigidbody.velocity = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), fixRate);
            rigidbody.isKinematic = true;
            isFlying = false;

            Debug.Log("bot: " + (int)score);
        }
    }
}
