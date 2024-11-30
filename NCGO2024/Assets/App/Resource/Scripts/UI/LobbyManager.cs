using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.SceneManagement;
public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private Button _startBtn, _leaveBtn, _readyBtn;
    [SerializeField] private GameObject _panelPrefab;
    [SerializeField] private GameObject _ContentGO;
    [SerializeField] private TMP_Text _rdyTxt;

    [SerializeField] private NetworkedPlayerData _networkPlayers;

    private List<GameObject> _PlayerPanels = new List<GameObject>();

    private ulong _myLocalClientID;

    private bool isReady = false;


    private void Start()
    {
        _myLocalClientID = NetworkManager.ServerClientId;

        if (ServerIsHost)
        {
            _rdyTxt.text = "Waiting for Players...";
            _readyBtn.gameObject.SetActive(false);
        }
        else
        {
            _rdyTxt.text = "Not Ready";
            _readyBtn.gameObject.SetActive(true);
        }

        _networkPlayers._allConnectedPlayers.OnListChanged += NetPlayersChanged;
        _leaveBtn.onClick.AddListener(LeaveBtnClick);
        _readyBtn.onClick.AddListener(ClientRdyBtnToggle);
    }

    private void ClientRdyBtnToggle()
    {
        if (IsServer) {return;}
        
        isReady = !isReady;

        if (isReady)
        {


            _rdyTxt.text = "Ready";
        }
        else
        {
            _rdyTxt.text = "NotReady"; 

        }

        RdyBtnToggleServerRpc(isReady);
 
    }




    [Rpc(target:SendTo.Server, RequireOwnership = false)]   
    private void RdyBtnToggleServerRpc(bool readyStatus, RpcParams rpcParams = default)
    {
        Debug.Log(message: "From Ready btn RPC");
        _networkPlayers.UpdateReadyClient(rpcParams.Receive.SenderClientId, readyStatus);
    }

    private void LeaveBtnClick()
    {
        if (!IsServer)
        {

            QuitLobbyServerRpc();

        }
        else
        {
            foreach (PlayerInfoData playerdata in _networkPlayers._allConnectedPlayers)
            {

                if (playerdata._clientID != _myLocalClientID)
                {


                    KickUserBtn(playerdata._clientID);

                }
            }

            NetworkManager.Shutdown();
            SceneManager.LoadScene(0);
        }
    }


    [Rpc(target:SendTo.Server, RequireOwnership = false)]
    private void QuitLobbyServerRpc(RpcParams rpcParams=default)
    {
        KickUserBtn(rpcParams.Receive.SenderClientId);
    }

    private void NetPlayersChanged(NetworkListEvent<PlayerInfoData> changeEvent)
    {

        Debug.Log(message: "Net Players has changed event fired");
        PopulateLabels();
    }

    [ContextMenu(itemName:"PopulateLabel")]

    private void PopulateLabels()
    {
        ClearPlayerPanel();




        bool allReady = true;

        foreach (PlayerInfoData playerData in _networkPlayers._allConnectedPlayers)
        {
            GameObject newPlayerPanel = Instantiate(_panelPrefab, _ContentGO.transform);

            PlayerLabel _playerLabel = newPlayerPanel.GetComponent<PlayerLabel>();

            _playerLabel.onKickClicked += KickUserBtn;

            if (IsServer && playerData._clientID != _myLocalClientID) 
            {               
                _playerLabel.setKickActive(isOn: true);
              
            }
            else
            {
                _playerLabel.setKickActive(isOn: false);
            }

            _playerLabel.SetPlayerLabelName(playerData._clientID);
            _playerLabel.setReady(playerData._isPlayerReady);
            _playerLabel.setPlayerColor(playerData._colorID);
            _PlayerPanels.Add(newPlayerPanel);

            if(playerData._isPlayerReady == false)
            {
                allReady = false;   
            }
        }

        if (IsServer) 
        {
            if (allReady)
            {
                if (_networkPlayers._allConnectedPlayers.Count > 1)
                {
                    _rdyTxt.text = "Ready to start";
                    _startBtn.gameObject.SetActive(true);  
                }
                else
                {
                    _rdyTxt.text = "Empty Lobby";
                }

            }
            else
            {
                _rdyTxt.text = "Waiting for Ready Players";
            }

        }
    }

    private void KickUserBtn(ulong kickTarget)
    {
        if(!IsServer || !IsHost) return;

        foreach (PlayerInfoData playerData in _networkPlayers._allConnectedPlayers)
        {
            if (playerData._clientID == kickTarget)
            {

                _networkPlayers._allConnectedPlayers.Remove(playerData);

                KickedClientRPC(RpcTarget.Single(kickTarget, RpcTargetUse.Temp));

                NetworkManager.Singleton.DisconnectClient(kickTarget);
            }
        }
    }


    [Rpc(target:SendTo.SpecifiedInParams)]
    private void KickedClientRPC(RpcParams rpcParams)
    {
        SceneManager.LoadScene(0);
    }

    private void ClearPlayerPanel()
    {
        foreach (GameObject panel in _PlayerPanels) 
        { 
            Destroy(panel);
        }

        _PlayerPanels.Clear();
    }









}
