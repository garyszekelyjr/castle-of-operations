using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = GameObject.Instantiate(player, new Vector3(0, 0, -5), Quaternion.identity);
        player.name = "Player";
    }

    void Update()
    {
        
    }
}
