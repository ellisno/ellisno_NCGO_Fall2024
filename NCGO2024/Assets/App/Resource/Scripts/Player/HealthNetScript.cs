using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace App.Resources.Scripts.Player
{
    public class HealthNetScript : NetworkBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] public float _startingHealth = 100f;
        [SerializeField] public float cooldown = 1.5f;
        [SerializeField] private bool _canDamage = true;
        [SerializeField] private NetworkVariable<float> _Health = new NetworkVariable<float>(100);
        public Image _healthBar;

        private GameScript _gameScript;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _Health.Value = _startingHealth;

            _Health.OnValueChanged += UpdateHealth;
        }

        // Update is called once per frame
        private void UpdateHealth(float previousValue, float newValue)
        {
            if (_healthBar != null)
            {
                _healthBar.fillAmount = newValue / _startingHealth;
            }

            if (IsOwner)
            {

                if (newValue < 0f)
                {
                    FindObjectOfType<GameScript>().PlayerDeathRpc();
                    HasDiedRpc();
                }
            }
        }

        [Rpc(target: SendTo.Server)]

        public void HasDiedRpc()
        {
            NetworkObject.Despawn();
        }

        [Rpc(target: SendTo.Server, RequireOwnership = false)]

        public void DamageObjRpc(float dmg)
        {
            if (!_canDamage) return;
            _Health.Value -= dmg;
            StartCoroutine(nameof(DamageCooldown));
            Debug.Log(message: $"Damage received : {dmg}");
        }

        private IEnumerable DamageCooldown()
        {
            _canDamage = false;
            yield return new WaitForSeconds(cooldown);
            _canDamage = true;
        }
    }
}