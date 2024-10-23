using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace Assets.App.Resource.Scripts.Player
{
    public class PlayerMovement : NetworkBehaviour
    {

        [SerializeField] Animator _myAnimator;
        [SerializeField] private OwnerNetworkAnimator _ownerAnimator;

        // Update is called once per frame

        private void Awake()
        {
            if(_myAnimator == null)
            {
                _myAnimator = gameObject.GetComponent<Animator>();
            }

            if (_ownerAnimator == null) { 
            
               _ownerAnimator = gameObject.GetComponent<OwnerNetworkAnimator>();
            }
        }
        void FixedUpdate()
        {
            if(!IsOwner)return;

            Vector3 moveDirection = new Vector3(x: 0, y: 0, z: 0);
            if (Input.GetKey(KeyCode.W)) moveDirection.z = +1f;
            if (Input.GetKey(KeyCode.S)) moveDirection.z = -1f;
            if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;
            if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;


            if (Input.GetKey(KeyCode.Space)) _ownerAnimator.SetTrigger("JumpTrigger");


            if (Input.GetKey(KeyCode.Z)) _ownerAnimator.SetTrigger("PunchTrigger");


            if (Input.GetKey(KeyCode.LeftShift))
            {
                _myAnimator.SetBool(name:"IsSprinting", value: true);
            }
            else
            {
                _myAnimator.SetBool(name: "IsSprinting", value: false);
            }


            if (moveDirection.z != 0 || moveDirection.x != 0) {

                _myAnimator.SetBool(name: "IsWalking", value: true);
            }


            float moveSpeed = 3f;
            transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        }
    }
}