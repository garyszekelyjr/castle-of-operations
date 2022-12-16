using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlimeController : MonoBehaviour
{
    private Vector3 targetSpot;
    private float fightDistance;
    private Animator animation_controller;
    private CharacterController controller;
    
    private bool isBattle;

    public int hp;
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
        fightDistance = 3.0f;
        animation_controller = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        hp = 100;
    }

    void Update() {
        if(!isBattle && hp > 0){
            // Find direction to player
            GameObject player = GameObject.Find("Player");
            Vector3 player_centroid = player.GetComponent<CharacterController>().bounds.center;
            Vector3 mob_centroid = controller.bounds.center;
            Vector3 direction = player_centroid - mob_centroid;
            direction.Normalize();

            if (Vector3.Distance(targetSpot,controller.transform.position) > 0.1f) {

                float angle_to_rotate = Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z);
                controller.transform.eulerAngles = new Vector3(0.0f, angle_to_rotate, 0.0f);

                if ((Vector3.Distance(player_centroid,mob_centroid) > fightDistance) && (GameManager.Instance.state != GameState.InBattle)){
                    //player.GetComponent<Player>().StartBattle(this);
                    GameManager.Instance.UpdateGameState(GameState.InBattle);
                }
            }
        }
    }

    public void GetHit(){
        animation_controller.SetTrigger("getHit");
        hp -= (int)Mathf.Floor(UnityEngine.Random.Range(25.0f, 40.0f));;
    }

    public void Attack(){
        animation_controller.SetTrigger("attack");
    }

    public void Die(){
        animation_controller.SetTrigger("die");
    }

}
