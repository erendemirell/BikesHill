using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float dashSpeed;
    [SerializeField] float time;
    [SerializeField] Transform whellFront;

    bool isFlying;
    float angle;
    public float score = 0;
    float fixRate = 1f;

    Quaternion rotCur;
    Quaternion maxAngle = Quaternion.Euler(65f, 0, 0);
    Vector3 controlPoint;

    Rigidbody rigidbody;

    Finish finish;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        finish = FindObjectOfType<Finish>();
    }

    private void Start()
    {
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

            if (Input.GetMouseButton(0))
            {
                if (transform.rotation.x > 0)
                {
                    speed += Time.fixedDeltaTime * 5f;
                }
            }
        }
        else
        {
            Dash();
        }
    }

    private void Dash()
    {
        if (Input.GetMouseButton(0))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, maxAngle, dashSpeed * Time.fixedDeltaTime);
            rigidbody.velocity = transform.forward * speed;
            speed += Time.fixedDeltaTime * 1f;
            dashSpeed += Time.fixedDeltaTime * 2f;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, maxAngle, 0.25f * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        Debug.DrawRay(whellFront.position, whellFront.forward * 0.5f, Color.yellow);
        Time.timeScale = time;

        RaycastHit hit;

        if (Physics.Raycast(whellFront.position, -whellFront.up, out hit, 1.3f))
        {
            Debug.DrawRay(whellFront.position, -whellFront.up * 0.5f, Color.blue);

            rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            isFlying = false;
        }
        else if (Physics.Raycast(whellFront.position, whellFront.forward, out hit, 1.75f))
        {
            angle = Vector3.Angle(hit.normal, whellFront.forward);
            Debug.DrawRay(hit.point, hit.normal * 1, Color.yellow, 10f);
            print("açı :" + angle);
            if (angle > 170)
            {
                if (speed > 50)
                {
                    Destroy(gameObject);
                }
                else
                {
                    speed = 20f;
                    isFlying = false;
                }
            }
            else if (170 >= angle && 130 <= angle)
            {
                rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                speed = 20f;
                isFlying = false;
            }
            else
            {
                rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                isFlying = false;
            }
        }
        else
        {
            isFlying = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            controlPoint.z = other.gameObject.transform.position.z;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            score = transform.position.z - controlPoint.z;
            rigidbody.velocity = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), fixRate);
            rigidbody.isKinematic = true;
            isFlying = false;
            finish.FinishGame();

            Debug.Log("player: " + (int)score);
        }
    }
}
