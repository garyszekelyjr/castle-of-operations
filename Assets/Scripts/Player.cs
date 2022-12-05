using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animation_controller;

    public Transform cam;

    public float speed = 2.0f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVel;
    
    private bool inBattle;
    private MobController enemy;

    void Awake(){
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void Destroy(){
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state){
        inBattle = state == GameState.InBattle;
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
}
