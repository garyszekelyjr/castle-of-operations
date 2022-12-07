using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlimeController : MonoBehaviour
{
    public int hp;
    public float turn_smooth_time = 0.1f;

    SkinnedMeshRenderer controller;
    Animator animator;
    bool in_battle;
    bool dead;

    float turn_smooth_vel;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        in_battle = false;
        hp = 100;
        dead = false;
    }

    void Update()
    {
        if (!dead)
        {
            if (in_battle)
            {
                Vector3 direction = (GameObject.Find("Player").GetComponent<CharacterController>().bounds.center - controller.bounds.center).normalized;
                float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle_to_rotate, ref turn_smooth_vel, turn_smooth_time);
                transform.rotation = Quaternion.Euler(0, angle, 0);

            }

            if (Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position) < 2)
            {
                in_battle = true;
                GameObject.Find("Player").GetComponent<PlayerController>().in_battle = true;
                GameObject.Find("Player").GetComponent<PlayerController>().enemy = gameObject;

            }
            else
            {
                in_battle = false;
            }
        }
    }

    public void Die()
    {
        animator.SetTrigger("die");
        dead = true;
    }
}
