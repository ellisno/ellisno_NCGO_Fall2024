using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ParrelSync.NonCore;

namespace App.Resource.Scripts.Obj {
    public class AmmoPickup : NetworkBehaviour
    {
        // Start is called before the first frame update
        private void OnCollisionEnter(Collision other)
        {
            if (!IsServer) return;
            if (other.gameObject.tag == "Player" )
            {

                other.gameObject.GetComponent<BulletSpawner>().ammo.Value++;

                Destroy( gameObject );
            };
        }
    }
}