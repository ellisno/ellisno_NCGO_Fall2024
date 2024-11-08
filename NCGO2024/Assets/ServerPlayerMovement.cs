using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class ServerPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _pSpeed;
    [SerializeField] private Transform _pTransform;
    [SerializeField] private NetworkAnimator _myNetworkAnimator;
    [SerializeField] private Animator _myAnimator;

    public CharacterController _CC;

    private MyPlayerInputActions _playerInput;
    // Start is called before the first frame update
    void Start()
    {
        if (_myAnimator == null)
        {
            _myAnimator = gameObject.GetComponent<Animator>();
        }

        if (_myNetworkAnimator == null)
        {
            _myNetworkAnimator = gameObject.GetComponent<NetworkAnimator>();
        }
        _playerInput = new MyPlayerInputActions();
        _playerInput.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!IsOwner) return;
        Vector2 moveInput = _playerInput.Player.Movement.ReadValue<Vector2>();

        bool isJumping = _playerInput.Player.Jumping.triggered;
        bool isPunching = _playerInput.Player.Punching.triggered;
        bool isSprinting = _playerInput.Player.Sprinting.triggered;
       

        //determine if server or player 
        if (IsServer)
        {
            //Move if server
            Move(moveInput, isPunching, isSprinting, isJumping);

        }else if(IsClient)
        {
            //Send a move request rpc to move the player
            MoveServerRPc(moveInput, isPunching, isSprinting, isJumping);
        }

    }

    private void Move(Vector2 _input, bool isPunching, bool isSprinting, bool isJumping)
    {
        Vector3 _moveDirection = _input.x * _pTransform.right + _input.y * _pTransform.up;

        _myAnimator.SetBool(name: "IsWalking", value: _input.x != 0 || _input.y != 0);

        if (isJumping) _myNetworkAnimator.SetTrigger("JumpTrigger");


        if (isPunching) _myNetworkAnimator.SetTrigger("PunchTrigger");


        _myAnimator.SetBool(name: "IsSprinting", isSprinting);

        if (isSprinting)
        {
            _CC.Move(motion: _moveDirection * (_pSpeed * 1.3f) * Time.deltaTime);
        }
        else
        {
            _CC.Move(motion: _moveDirection * _pSpeed * Time.deltaTime);
        }
    }

    [Rpc(target:SendTo.Server)]
    private void MoveServerRPc(Vector2 _input, bool isPunching, bool isSprinting, bool isJumping)
    {
        Move(_input, isPunching, isSprinting, isJumping);
    }
}
