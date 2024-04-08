using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().hasFlag && GameManager.instance.points < GameManager.instance.totalPoints && GameManager.instance.currentTime <= GameManager.instance.lossTimer)
        {
            GameManager.instance.WinRound();
            other.gameObject.GetComponent<PlayerController>().flag.SetActive(false);
            other.gameObject.GetComponent<PlayerController>().hasFlag = false;  
            Debug.Log("WIN ROUND");                
        }
        else if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().hasFlag && GameManager.instance.points >= GameManager.instance.totalPoints && GameManager.instance.currentTime <= GameManager.instance.lossTimer)
        {
            GameManager.instance.Win();
            Debug.Log("WIN");

        }
    }
}
