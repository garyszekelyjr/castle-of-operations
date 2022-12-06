using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animation_controller;

    public Transform cam;

    public float speed = 2.0f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVel;
    
    public GameObject BattleUI;
    public TMP_InputField input;
    public TextMeshProUGUI question;
    private bool inBattle;
    private MobController enemy;
    private bool answered = true;
    private int correctAns;

    void Awake(){
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void Destroy(){
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state){
        inBattle = state == GameState.InBattle;
        animation_controller.SetBool("inBattle", inBattle);
        BattleUI.SetActive(inBattle);
        if(inBattle){
            animation_controller.SetBool("isWalking", false);
        }
    }

    void Start(){
        controller = GetComponent<CharacterController>();
        animation_controller = GetComponent<Animator>();
    }

    void Update()
    {
        if (inBattle) {
            Vector3 mob_centroid = enemy.GetComponent<CharacterController>().bounds.center;
            Vector3 player_centroid = controller.bounds.center;
            Vector3 direction = mob_centroid - player_centroid;
            float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle_to_rotate, ref turnSmoothVel, turnSmoothTime); 
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            // Generate Question
            if(answered){
                answered = false;
                correctAns = NewQuestion();
            }
            if(input.text != "" && Input.GetKeyUp(KeyCode.Return)){
                Answer();
            }

        } else {
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(hor, 0f, ver).normalized;

            if(direction.magnitude >= .01f){
                float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle_to_rotate, ref turnSmoothVel, turnSmoothTime); 
                transform.rotation = Quaternion.Euler(0f,angle,0f);

                Vector3 moveDir = Quaternion.Euler(0f, angle_to_rotate, 0f) * Vector3.forward;
                controller.Move(moveDir*speed*Time.deltaTime);
                animation_controller.SetBool("isWalking", true);
            } else {
                animation_controller.SetBool("isWalking", false);
            }
        }
    }

    public void StartBattle(MobController newEnemy){
        enemy = newEnemy;
    }

    // Successful attack
    private void SuccessfulAttack(){
        animation_controller.SetTrigger("attack");
        enemy.GetHit();
    }

    // Unsuccessful attack
    public void UnsuccessfulAttack(){
        enemy.Attack();
        animation_controller.SetTrigger("getHit");
    }

    private string GetOperator(){
        int mob_id = enemy.mob_id;

        switch (mob_id){
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

    private int NewQuestion(){
        int num1 = (int)Mathf.Floor(UnityEngine.Random.Range(0.0f, 11.0f));
        int num2 = (int)Mathf.Floor(UnityEngine.Random.Range(0.0f, 11.0f));
        string op = GetOperator();
        string expression = num1 + op + num2;
        question.text = expression;
        ExpressionEvaluator.Evaluate(expression, out int result);
        return result;
    }

    public void Answer(){
        answered = true;
        if(int.Parse(input.text) == correctAns){
            SuccessfulAttack();
            if(enemy.hp <= 0){
                GameManager.Instance.UpdateGameState(GameState.NotInBattle);
                enemy.Die();
            }
        } else {
            UnsuccessfulAttack();
        }
    }
}
