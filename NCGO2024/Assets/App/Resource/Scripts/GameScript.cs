using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class GameScript : NetworkBehaviour
{

    [SerializeField] private List<ulong> _currentPlayers;

    public TMP_Text myWinText;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        myWinText.gameObject.SetActive(false);
    }
    [Rpc(target:SendTo.Server)]

    public void AddPlayerRpc(RpcParams rpcParams = default)
    {
        _currentPlayers.Add(rpcParams.Receive.SenderClientId);
    }

    [Rpc(target: SendTo.Server)]

    public void PlayerDeathRpc(RpcParams rpcParams = default) { 
    
    
        _currentPlayers.Remove(rpcParams.Receive.SenderClientId);

        YouLoseRpc( RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));

        if(_currentPlayers.Count == 1)
        {
            YouWinRpc( RpcTarget.Single(_currentPlayers[0], RpcTargetUse.Temp));
        }
    }

    [Rpc(target: SendTo.SpecifiedInParams)]

    public void YouLoseRpc(RpcParams rpcParams = default) {


        myWinText.text = "You Lose!";
        myWinText.gameObject.SetActive(true);
    
    }

    [Rpc(target: SendTo.SpecifiedInParams)]

    public void YouWinRpc(RpcParams rpcParams = default)
    {


        myWinText.text = "You Win!";
        myWinText.gameObject.SetActive(true);

    }
}
