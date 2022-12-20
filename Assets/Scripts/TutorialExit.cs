using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialExit : MonoBehaviour
{
    GameManager gameManager;
    GameObject slime;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        slime = GameObject.Find("Slime");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && slime.GetComponent<Mob>().hp <= 0)
        {
            gameManager.UpdateLevel(Level.Hallway);
            SceneManager.LoadScene("Addition Hallway");
        }
    }
}
