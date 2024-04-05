using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().hasFlag && GameManager.Instance.points < GameManager.Instance.totalPoints && GameManager.Instance.currentTime <= GameManager.Instance.lossTimer)
        {
            GameManager.Instance.WinRound();
            other.gameObject.GetComponent<PlayerController>().flag.SetActive(false);
            other.gameObject.GetComponent<PlayerController>().hasFlag = false;  
            Debug.Log("WIN ROUND");                
        }
        else if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>().hasFlag && GameManager.Instance.points >= GameManager.Instance.totalPoints && GameManager.Instance.currentTime <= GameManager.Instance.lossTimer)
        {
            GameManager.Instance.Win();
            Debug.Log("WIN");

        }
    }
}
