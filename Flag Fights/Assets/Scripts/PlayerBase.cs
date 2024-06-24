using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerModel>().HasFlag && GameManager.instance.currentTime <= GameManager.instance.lossTimer)
        {
            Debug.Log("A");
            other.GetComponent<PlayerModel>().HasFlag = false;
            other.GetComponent<PlayerView>().flag.SetActive(false);
            GameManager.instance.EndRound(true);
        }
    }
}
