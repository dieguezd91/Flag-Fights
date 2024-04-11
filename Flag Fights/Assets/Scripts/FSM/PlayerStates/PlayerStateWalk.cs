using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWalk<T> : State<T>
{
    float horizontalInput;
    float forwardInput;
    [SerializeField] float speed = 2;
    [SerializeField] float turnSpeed = 100;
    Transform _transform;

    T _input;

    public PlayerStateWalk(Transform transform, T input)
    {
        _transform = transform;
        _input = input;
    }

    public override void Execute()
    {
        base.Execute();

        //3D MOVEMENT
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        _transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        _transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);


        //2D MOVEMENT
        //Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //transform.Translate(new Vector3(input.x, 0f, input.y) * speed * Time.deltaTime);


        if (horizontalInput == 0 && forwardInput == 0) _fsm.Transition(_input);
    }
}
