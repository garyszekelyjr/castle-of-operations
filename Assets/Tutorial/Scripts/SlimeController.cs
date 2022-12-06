using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position) < 2)
        {
            Debug.Log("IN ATTACK");
        }
        else
        {
            Debug.Log("NOT IN ATTACK");
        }
    }
}
