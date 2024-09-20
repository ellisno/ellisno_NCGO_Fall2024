using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;

namespace App.Resource.Scripts.Player
{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }

    }

}