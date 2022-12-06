using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    private float speed;
    private float aware_distance;
    private float gravity;
    private Vector3 targetSpot;
    private Vector3 moveDirection;
    private float wanderRange;
    private float fightDistance;
    private Animator animation_controller;
    private CharacterController controller;
    private float movementTime;
    
    private bool isBattle;

    public int mob_id;

    void Awake(){
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void Destroy(){
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state){
        isBattle = state == GameState.InBattle;
    }

    void Start() {
        speed = 2.5f;
        aware_distance = 10.0f;
        gravity = .01f;
        wanderRange = 5.0f;
        fightDistance = 3.0f;
        movementTime = UnityEngine.Random.Range(5.0f, 10.0f);
        animation_controller = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
        StartCoroutine("RandomMove");
    }

    void Update() {
        // Gravity
        moveDirection.y -= gravity;
        if(controller.isGrounded){
            moveDirection.y = 0;
        }

        if(!isBattle){
            // Find direction to player
            GameObject player = GameObject.Find("Player");
            Vector3 player_centroid = player.GetComponent<CharacterController>().bounds.center;
            Vector3 mob_centroid = controller.bounds.center;
            Vector3 direction = player_centroid - mob_centroid;
            direction.Normalize();

            // Check if player is visible
            RaycastHit hit;

            if (Physics.Raycast( mob_centroid, direction, out hit, aware_distance) && (hit.collider.gameObject == player)) {

                // Move mob towards player if visible

                if(Vector3.Distance(player_centroid,mob_centroid) > fightDistance){
                    moveDirection.x = direction.x;
                    moveDirection.z = direction.z;
                    float angle_to_rotate = Mathf.Rad2Deg * Mathf.Atan2(direction.x, moveDirection.z);
                    controller.transform.eulerAngles = new Vector3(0.0f, angle_to_rotate, 0.0f);
                    animation_controller.SetBool("isWalking", true);
                } else {
                    moveDirection.x = 0;
                    moveDirection.z = 0;
                    animation_controller.SetBool("isWalking", false);

                    // Start Battle
                    player.GetComponent<Player>().StartBattle(this);
                    GameManager.Instance.UpdateGameState(GameState.InBattle);
                }

            } else if (Vector3.Distance(targetSpot,controller.transform.position) > 0.1f){

                // Move to wander location if player is not visible

                animation_controller.SetBool("isWalking", true);
                Vector3 toTargetDirection = targetSpot - controller.transform.position;
                toTargetDirection.Normalize(); 
                moveDirection.x = toTargetDirection.x;
                moveDirection.z = toTargetDirection.z;
                float angle_to_rotate = Mathf.Rad2Deg * Mathf.Atan2(moveDirection.x, moveDirection.z);
                controller.transform.eulerAngles = new Vector3(0.0f, angle_to_rotate, 0.0f);

            } else {
                animation_controller.SetBool("isWalking", false);
                moveDirection.x = 0;
                moveDirection.z = 0;
            }

            controller.Move(moveDirection * Time.deltaTime * speed);
        } else {
            animation_controller.SetBool("isWalking", false);
        }
    }

    // Randomly path mob around to simulate natural movement
    IEnumerator RandomMove() {
        for(;;){
            targetSpot = new Vector3(controller.transform.position.x + UnityEngine.Random.Range(-wanderRange, wanderRange), controller.transform.position.y, controller.transform.position.z + UnityEngine.Random.Range(-wanderRange, wanderRange));
            yield return new WaitForSeconds(movementTime);
        }
    }

    public void GetHit(){
        animation_controller.SetTrigger("getHit");
    }

    public void Attack(){
        animation_controller.SetTrigger("attack");
    }
}
