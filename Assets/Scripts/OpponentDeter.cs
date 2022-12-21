using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentDeter : MonoBehaviour
{
    public GameObject chestMonster;
    public GameObject turtleShell;
    public GameObject beholderPBR;
    public GameObject slime;
    public int opp;

    // Start is called before the first frame update
    void Start()
    {
        chestMonster.SetActive(false);
        turtleShell.SetActive(false);
        beholderPBR.SetActive(false);
        slime.SetActive(false);

        if (opp == 1){
            chestMonster.SetActive(true);
        } else if (opp == 2){
            turtleShell.SetActive(true);
        } else if (opp == 3){
            beholderPBR.SetActive(true);
        } else if (opp == 4){
            slime.SetActive(true);
        }
    }
}
