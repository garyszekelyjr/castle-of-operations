using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 2.0f;
    private float turnSmoothTime = 0.1f;
    private GameObject BattleUI;
    private TMP_InputField input;
    private TextMeshProUGUI question;
    private Slider enemy_hp;
    private TextMeshProUGUI enemy_name_text;
    
    private TextMeshProUGUI popupText;
    public GameObject SpawnPoint;

    CharacterController controller;
    Animator animator;
    Transform main_camera;

    float turnSmoothVel;
    bool inBattle;
    MobController enemy;
    bool answered = true;
    int correctAns;
    float curVel;
    public int max_hp = 100;
    public int hp = 100;
    public int max_stamina = 100;
    public int stamina = 100;
    public int level = 1;
    Vector3 warpPosition = Vector3.zero;

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void Destroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameState state)
    {
        inBattle = state == GameState.InBattle;
        animator.ResetTrigger("getHit");
        animator.ResetTrigger("attack");
        BattleUI.SetActive(inBattle);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        main_camera = GameObject.Find("Main Camera").transform;
        popupText = GameObject.Find("PopupText").GetComponent<TextMeshProUGUI>();
        enemy_name_text = GameObject.Find("EnemyName").GetComponent<TextMeshProUGUI>();
        question = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();
        enemy_hp = GameObject.Find("EnemyHP").GetComponent<Slider>();
        input = GameObject.Find("Answer").GetComponent<TMP_InputField>();
        BattleUI = GameObject.Find("BattleUI");
    }

    void Update()
    {

        if (inBattle)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("inBattle", true);

            Vector3 mob_centroid = enemy.GetComponent<CharacterController>().bounds.center;
            Vector3 player_centroid = controller.bounds.center;
            Vector3 direction = mob_centroid - player_centroid;
            float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle_to_rotate, ref turnSmoothVel, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // HP Bar
            float currentHP = Mathf.SmoothDamp(enemy_hp.value, enemy.hp, ref curVel, 50 * Time.deltaTime);
            enemy_hp.value = currentHP;

            // Generate Question
            if (answered)
            {
                answered = false;
                correctAns = NewQuestion();
            }
            if (input.text != "" && Input.GetKeyUp(KeyCode.Return))
            {
                Answer();
            }
        }
        else
        {
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
        }
    }

    public void StartBattle(MobController newEnemy)
    {
        enemy = newEnemy;
        enemy.hp = 100;
        switch(newEnemy.mob_id){
            case 1:
                enemy_name_text.text = "Slime";
                break;
            case 2:
                enemy_name_text.text = "Spiked Shell";
                break;
            case 3:
                enemy_name_text.text = "Beholder";
                break;
            case 4:
                enemy_name_text.text = "Chest Monster";
                break;
            default:
                enemy_name_text.text = "Unknown";
                break;
        }
    }

    // Successful attack
    void SuccessfulAttack()
    {
        animator.SetTrigger("attack");
        enemy.GetHit();
    }

    // Unsuccessful attack
    public void UnsuccessfulAttack()
    {
        enemy.Attack();
        animator.SetTrigger("getHit");
        hp -= (int)Mathf.Floor(UnityEngine.Random.Range(10.0f, 20.0f)); ;
    }

    string GetOperator()
    {
        int mob_id = enemy.mob_id;
        switch (mob_id)
        {
            case 1:
                return "+";
            case 2:
                return "-";
            case 3:
                return "/";
            case 4:
                return "*";
            default:
                return "FALSE";
        }
    }

    int NewQuestion()
    {
        int num1 = (int)Mathf.Floor(UnityEngine.Random.Range(0.0f, 11.0f));
        int num2 = (int)Mathf.Floor(UnityEngine.Random.Range(0.0f, 11.0f));
        string op = GetOperator();
        string expression = num1 + op + num2;
        question.text = expression;
        ExpressionEvaluator.Evaluate(expression, out int result);
        return result;
    }

    public void Answer()
    {
        answered = true;
        if (int.Parse(input.text) == correctAns)
        {
            SuccessfulAttack();
            if (enemy.hp <= 0)
            {
                GameManager.Instance.UpdateGameState(GameState.NotInBattle);
                enemy.Die();
            }
        }
        else
        {
            UnsuccessfulAttack();
            if (hp <= 0)
            {
                Die();
            }
        }
        input.text = "";
    }

    void Die()
    {
        WarpToPosition(SpawnPoint.transform.position);
        StartCoroutine(ShowMessage("Try Again!", 2.0f));
        hp = 100;
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        popupText.text = message;
        popupText.enabled = true;
        yield return new WaitForSeconds(delay);
        popupText.enabled = false;
    }

    public void WarpToPosition(Vector3 newPosition)
    {
        warpPosition = newPosition;
    }

    void LateUpdate()
    {
        if (warpPosition != Vector3.zero)
        {
            controller.enabled = false;
            transform.position = warpPosition;
            controller.enabled = true;
            warpPosition = Vector3.zero;
            GameManager.Instance.UpdateGameState(GameState.NotInBattle);
        }
    }
}
