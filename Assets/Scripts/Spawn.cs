using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] tileprefab;

    Vector3 vector1;
    Vector3 vector2;
    Vector3 vector3;

    List<string> oyuncular = new List<string>();

    private void Start()
    {
        vector1 = new Vector3(3.6f, 10, -40);
        vector2 = new Vector3(-3.6f, 10, -30);
        vector3 = new Vector3(-8f, 10, -25);
        Invoke("Timer", 1);
    }
    public void Timer()
    {
        if (!oyuncular.Contains("go"))
        {
            GameObject sa = Instantiate(tileprefab[0], vector1, Quaternion.identity);
            GameObject da = Instantiate(tileprefab[1], vector2, Quaternion.identity);
            GameObject ma = Instantiate(tileprefab[2], vector3, Quaternion.identity);
            oyuncular.Add("go");
        }
    }
}
