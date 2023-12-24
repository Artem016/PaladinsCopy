using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class PlayerMovment : NetworkBehaviour
{
    [SerializeField] Ball _prefabBall;

    private Vector3 _forward = Vector3.forward;
    private Vector3 _velocity;
    private bool _jumpPressed;

    private CharacterController _controller;

    public float PlayerSpeed = 2f;
    public Camera Camera;

    public float jumpForce = 5f;
    public float GravityValue = -9.81f;

    [Networked] private TickTimer delay { get; set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority == false)
        {
            return;
        }

        var cameraRotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        _velocity.y += GravityValue * Runner.DeltaTime;
        
        if(_jumpPressed && _controller.isGrounded)
        {
            _velocity.y += jumpForce;
        }
        _controller.Move(move + _velocity * Runner.DeltaTime);

        if(move !=  Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if(_controller.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
        {
            if (Input.GetKey(KeyCode.E))
            {
                delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                Runner.Spawn(_prefabBall,
                   transform.position + _forward, Quaternion.LookRotation(_forward),
                   Object.InputAuthority, (runner, o) =>
                   {
                       var ball = o.GetComponent<Ball>();
                       ball.transform.forward = transform.forward;
                       ball.Init();
                   });
            }
        }

        

        _jumpPressed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Debug.Log("Убит");
        }
    }
}
