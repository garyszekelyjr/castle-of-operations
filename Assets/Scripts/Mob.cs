using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public bool isTutorial;
    public bool isBoss = false;
    public int hp;
    public string mobName = "Slime";

    float awareDistance;
    public float fightDistance;
    float speed = 2.5f;

    GameManager gameManager;
    GameObject player;
    CharacterController controller;
    Animator animator;
    Vector3 moveDirection;
    Vector3 targetSpot;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        moveDirection = Vector3.zero;
        awareDistance = 10f;
        fightDistance = 3f;
        hp = 100;

        if (!isTutorial && !isBoss)
        {
            StartCoroutine("RandomMove");
        }
    }

    void Update()
    {
        if (hp >= 0 && !(gameManager.state == State.InBattle))
        {
            // Find direction to player
            Vector3 playerCentroid = player.GetComponent<CharacterController>().bounds.center;
            Vector3 mobCentroid = controller.bounds.center;
            Vector3 direction = (playerCentroid - mobCentroid).normalized;

            // Check if player is visible

            if(Vector3.Distance(playerCentroid, mobCentroid) < awareDistance)
            {
                // Move mob towards player if visible

                if (Vector3.Distance(playerCentroid, mobCentroid) > fightDistance)
                {
                    moveDirection = new Vector3(direction.x, 0, direction.z);
                    controller.transform.eulerAngles = new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(direction.x, moveDirection.z), 0);
                    if(!isBoss){
                        animator.SetBool("isWalking", true);
                    }
                }
                else
                {
                    moveDirection = Vector3.zero;
                    if(!isBoss){
                        animator.SetBool("isWalking", false);
                    }
                    player.GetComponent<Player>().mob = gameObject;
                    gameManager.UpdateState(State.InBattle);
                }
            }
            else if (Vector3.Distance(targetSpot, controller.transform.position) > 0.1f)
            {
                // Move to wander location if player is not visible

                if(!isBoss){
                    animator.SetBool("isWalking", true);
                }
                Vector3 toTargetDirection = (targetSpot - controller.transform.position).normalized;
                moveDirection = new Vector3(toTargetDirection.x, 0, toTargetDirection.z);
                controller.transform.eulerAngles = new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(moveDirection.x, moveDirection.z), 0);
            }
            else
            {
                if(!isBoss){
                    animator.SetBool("isWalking", false);
                }
                moveDirection = Vector3.zero;
            }
            
            if (!isTutorial && !isBoss)
            {
                controller.Move(new Vector3(moveDirection.x, 0, moveDirection.z) * Time.deltaTime * speed);
            }
        }
    }

    // Randomly path mob around to simulate natural movement
    IEnumerator RandomMove()
    {
        for (; ; )
        {
            targetSpot = new Vector3(controller.transform.position.x + Random.Range(-5f, 5f), 0, controller.transform.position.z + Random.Range(-5f, 5f));
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
    }

    public void Die()
    {
        animator.SetTrigger("die");
        gameManager.UpdateState(State.NotInBattle);
        controller.enabled = false;
    }

    public void GetHit(){
        if(isBoss){
            hp -= Random.Range(10, 15);
        } else {
            hp -= Random.Range(25, 40);
        }
        animator.SetTrigger("getHit");
    }
}
