using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    public float turn_smooth_time = 0.1f;
    public bool in_battle;
    public GameObject enemy;
    public int hp;

    CharacterController controller;
    Animator animator;
    Transform main_camera;
    GameObject battle_ui;

    float turn_smooth_vel;
    float curr_vel;

    string question;
    int answer;
    bool answered;

    void Start()
    {
        hp = 100;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        main_camera = GameObject.Find("Main Camera").transform;
        battle_ui = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        in_battle = false;
        answered = true;
    }

    void Update()
    {
        animator.SetBool("inBattle", in_battle);
        battle_ui.SetActive(in_battle);

        if (in_battle)
        {
            animator.SetBool("isWalking", false);

            Vector3 direction = (enemy.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().bounds.center - controller.bounds.center).normalized;
            float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle_to_rotate, ref turn_smooth_vel, turn_smooth_time);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            if (answered)
            {
                answered = false;
                Question();
            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                Answer();
            }
        }
        else
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (direction.magnitude > 0)
            {

                float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + main_camera.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.eulerAngles.y, angle_to_rotate, ref turn_smooth_vel, turn_smooth_time), 0);

                controller.Move(Quaternion.Euler(0, angle_to_rotate, 0) * Vector3.forward * speed * Time.deltaTime);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    void Question()
    {
        question = (int)Mathf.Floor(UnityEngine.Random.Range(0, 11)) + " + " + (int)Mathf.Floor(UnityEngine.Random.Range(0, 11));
        battle_ui.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = question;
        ExpressionEvaluator.Evaluate(question, out answer);
    }

    void Answer()
    {
        answered = true;
        if (int.Parse(battle_ui.transform.GetChild(1).GetComponent<TMP_InputField>().text) == answer)
        {
            animator.SetTrigger("attack");
            enemy.GetComponent<Animator>().SetTrigger("getHit");
            enemy.GetComponent<TutorialSlimeController>().hp -= (int)Mathf.Floor(UnityEngine.Random.Range(25f, 40f));
            if (enemy.GetComponent<TutorialSlimeController>().hp <= 0)
            {
                in_battle = false;
                enemy.GetComponent<TutorialSlimeController>().Die();
            }
        }
        else
        {
            enemy.GetComponent<Animator>().SetTrigger("attack");
            animator.SetTrigger("getHit");
            hp -= (int)Mathf.Floor(UnityEngine.Random.Range(10f, 20f));
            if (hp <= 0)
            {
                Die();
            }
        }
        battle_ui.transform.GetChild(1).GetComponent<TMP_InputField>().text = "";
    }

    void Die()
    {

    }
}
