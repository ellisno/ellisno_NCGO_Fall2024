using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLabel : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerText;
    [SerializeField] private Button _kickBtn;
    [SerializeField] private RawImage _readyStatusImg, _playerColorImg;

    public event Action<ulong> onKickClicked;
    private ulong _clientID;

    private void OnEnable()
    {
        _kickBtn.onClick.AddListener(btnKick_Clicked);
    }
    public void SetPlayerLabelName(ulong playerName)
    {
        _clientID = playerName;
        _playerText.text = "Player " + playerName.ToString();
    }

    private void btnKick_Clicked()
    {
        onKickClicked?.Invoke(_clientID);
    }

    public void setKickActive(bool isOn)
    {
        _kickBtn.gameObject.SetActive(isOn);
    }

    public void setReady(bool ready) {

        if (ready) { 
        
        
            _readyStatusImg.color = Color.green;
        
        }
        else
        {
            _readyStatusImg.color= Color.red;
        }
    
    }

    public void setPlayerColor(Color color)
    {
        _playerColorImg.color = color;
    }
}
