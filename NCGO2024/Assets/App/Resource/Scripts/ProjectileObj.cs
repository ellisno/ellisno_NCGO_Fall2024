using App.Resources.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace App.Resource.Scripts
{
    public class ProjectileObj : NetworkBehaviour
    {
        [SerializeField] float _speed = 40f;
        [SerializeField] private float _damage;
        [SerializeField] private float destructTime;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            GetComponent<Rigidbody>().velocity = this.transform.forward * _speed;
            StartCoroutine(routine: autoDestruct());
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Equals("Player") && other.gameObject.GetComponent<NetworkObject>().OwnerClientId != this.OwnerClientId)
            {
                other.gameObject.GetComponent<HealthNetScript>().DamageObjRpc(_damage);
            }
        }

        private IEnumerator autoDestruct()
        {
            yield return new WaitForSeconds(destructTime);
            this.NetworkObject.Despawn();
        }
    }
}
