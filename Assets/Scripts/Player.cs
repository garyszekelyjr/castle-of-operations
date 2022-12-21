using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject mob;

    public float speed = 2.0f;
    private float turnSmoothTime = 0.1f;
    GameObject battleMenu;
    TextMeshProUGUI question;
    TMP_InputField answer;
    Slider mobHp;
    TextMeshProUGUI mobName;
    TextMeshProUGUI popupText;

    float turnSmoothVel;
    bool answered = true;
    int correctAnswer;
    public int max_hp = 100;
    public int hp = 100;

    CharacterController controller;
    Animator animator;
    Transform main_camera;
    GameManager gameManager;

    Vector3 respawnPoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        main_camera = GameObject.Find("Main Camera").transform;

        battleMenu = GameObject.Find("BattleMenu");
        question = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();
        answer = GameObject.Find("Answer").GetComponent<TMP_InputField>();
        mobName = GameObject.Find("MobName").GetComponent<TextMeshProUGUI>();
        mobHp = GameObject.Find("MobHp").GetComponent<Slider>();
        popupText = GameObject.Find("PopupText").GetComponent<TextMeshProUGUI>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject.Find("UsernameText").GetComponent<TextMeshProUGUI>().text = gameManager.username;

        gameManager.UpdateState(State.NotInBattle);
        respawnPoint = transform.position;
    }

    void Update()
    {
        switch (gameManager.state)
        {
            case State.NotInBattle:
                battleMenu.SetActive(false);
                animator.SetBool("inBattle", false);

                Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

                if (direction.magnitude > 0)
                {
                    animator.SetBool("isWalking", true);

                    float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + main_camera.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.eulerAngles.y, angle_to_rotate, ref turnSmoothVel, turnSmoothTime), 0);

                    controller.Move(Quaternion.Euler(0, angle_to_rotate, 0) * Vector3.forward * speed * Time.deltaTime);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                }
                break;
            case State.InBattle:
                battleMenu.SetActive(true);
                animator.SetBool("isWalking", false);
                animator.SetBool("inBattle", true);

                Vector3 mob_centroid = mob.GetComponent<CharacterController>().bounds.center;
                Vector3 player_centroid = controller.bounds.center;
                Vector3 _direction = (mob_centroid - player_centroid).normalized;
                transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.eulerAngles.y, Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg, ref turnSmoothVel, turnSmoothTime), 0);

                mobName.text = mob.GetComponent<Mob>().mobName;
                mobHp.value = mob.GetComponent<Mob>().hp;

                // Generate Question
                if (answered)
                {
                    answered = false;
                    correctAnswer = Question();
                }
                if (answer.text != "" && Input.GetKeyUp(KeyCode.Return))
                {
                    Answer();
                }
                break;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
        mob.GetComponent<Mob>().GetHit();
    }

    public void Die()
    {
        StartCoroutine(ShowMessage("Try Again!", 2.0f));

        controller.enabled = false;
        transform.position = respawnPoint;
        controller.enabled = true;

        gameManager.UpdateState(State.NotInBattle);
        hp = 100;
    }

    string GetOperator()
    {
        switch (mob.GetComponent<Mob>().mobName)
        {
            case "Slime":
                return " + ";
            case "Skeletron, the Addition Operator":
                return " + ";
            case "Turtle":
                return " - ";
            case "Chest":
                return " / ";
            case "Beholder":
                return " * ";
            default:
                return " ";
        }
    }

    int Question()
    {
        string op = GetOperator();
        string expression;
        if (op != " / ")
        {
            expression = Random.Range(0, 13) + op + Random.Range(0, 13);
        }
        else
        {
            int denominator = Random.Range(0, 13);
            int numerator = denominator * Random.Range(0, 13);
            expression = numerator + " / " + denominator;
        }

        question.text = expression;
        ExpressionEvaluator.Evaluate(expression, out int answer);
        return answer;
    }

    public void Answer()
    {
        answered = true;
        if (int.Parse(answer.text) == correctAnswer)
        {
            GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("sword", typeof(AudioClip)));
            Attack();
            if (mob.GetComponent<Mob>().hp <= 0)
            {
                mob.GetComponent<Mob>().Die();
            }
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("punch", typeof(AudioClip)));
            mob.GetComponent<Mob>().Attack();
            if (!mob.GetComponent<Mob>().isTutorial)
            {
                hp -= Random.Range(10, 20);
            }
            if (hp <= 0)
            {
                Die();
            }
        }
        answer.text = "";
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        popupText.text = message;
        popupText.enabled = true;
        yield return new WaitForSeconds(delay);
        popupText.enabled = false;
    }
}
