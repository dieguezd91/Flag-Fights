using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Base
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().hasFlag && GameManager.instance.currentTime <= GameManager.instance.lossTimer)
        {
            if (GameManager.instance.points < GameManager.instance.totalPoints)
                GameManager.instance.WinRound();
            else GameManager.instance.Win();
        }
    }
}
