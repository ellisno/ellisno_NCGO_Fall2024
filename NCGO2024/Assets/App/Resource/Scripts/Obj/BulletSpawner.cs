using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace App.Resource.Scripts.Obj
{
    public class BulletSpawner : NetworkBehaviour
    {
        [SerializeField] public NetworkVariable<int> ammo = new NetworkVariable<int>(4);
        [SerializeField] private Transform _startingPoint;
        [SerializeField] private NetworkObject _projectilePrefab;

        [Rpc(target: SendTo.Server, RequireOwnership = false)]
        public void FireProjectileRpc(RpcParams rpcParams = default)
        {
            if (ammo.Value > 0)
            {
                NetworkObject newProjectile = NetworkManager.Instantiate(_projectilePrefab, _startingPoint.position, _startingPoint.rotation);

                newProjectile.SpawnWithOwnership(rpcParams.Receive.SenderClientId);

                ammo.Value--;
            }
        }

    }
}
