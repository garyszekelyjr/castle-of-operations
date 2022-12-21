using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    GameManager gameManager;
    GameObject slime;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        slime = GameObject.Find("Slime");
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (slime.GetComponent<Mob>().hp <= 0)
            {
                GetComponent<BoxCollider>().isTrigger = true;
            }
            else
            {
                GetComponent<BoxCollider>().isTrigger = false;
            }
        }
        else
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            gameManager.UpdateLevel(Level.Hallway);
            switch (SceneManager.GetActiveScene().name)
            {
                case "Tutorial":
                    SceneManager.LoadScene("Addition Hallway");
                    break;
                case "Addition Hallway":
                    SceneManager.LoadScene("Addition Arena");
                    break;
                case "Subtraction Hallway":
                    SceneManager.LoadScene("Subtraction Arena");
                    break;
                case "Multiplcation Hallway":
                    SceneManager.LoadScene("Multiplcation Arena");
                    break;
                case "Division Hallway":
                    SceneManager.LoadScene("Division Arena");
                    break;
            }

        }
    }
}
