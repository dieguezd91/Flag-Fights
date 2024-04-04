using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    float horizontalInput;
    float forwardInput;
    bool hasFlag = false;


    void Update()
    {
        //3D MOVEMENT

        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);


        //2D MOVEMENT
        //Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //transform.Translate(new Vector3(input.x, 0f, input.y) * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Flag"))
        {
            collision.gameObject.transform.SetParent(gameObject.transform);
            hasFlag = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Base") && hasFlag && GameManager.Instance.points < GameManager.Instance.totalPoints && GameManager.Instance.currentTime <= GameManager.Instance.lossTimer)
        {
            GameManager.Instance.WinRound();
            Debug.Log("WIN ROUND");
        }
        else if(other.gameObject.CompareTag("Base") && hasFlag && GameManager.Instance.points >= GameManager.Instance.totalPoints && GameManager.Instance.currentTime <= GameManager.Instance.lossTimer)
        {
            GameManager.Instance.Win();
            Debug.Log("WIN");
        }
    }
}
