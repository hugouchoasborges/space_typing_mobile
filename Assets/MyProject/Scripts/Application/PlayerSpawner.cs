using player;
using player.settings;
using System.Collections.Generic;
using UnityEngine;
using utils;

namespace application
{
    public class PlayerSpawner : MonoBehaviour
    {
        private List<PlayerController> _activePlayers;
        private PoolController<PlayerController> _queuedPlayers;

        private bool _initialized = false;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            _activePlayers = new List<PlayerController>();
            _queuedPlayers = new PoolController<PlayerController>(1, prefab: PlayerSettingsSO.Instance.Prefab);
        }

        public void Spawn()
        {
            PlayerController newPlayer = _queuedPlayers.Dequeue();
            newPlayer.gameObject.SetActive(true);
            newPlayer.transform.SetPositionAndRotation(Vector2.zero, Quaternion.identity);

            _queuedPlayers.Enqueue(newPlayer);
        }

        public void Destroy(PlayerController player)
        {
            _activePlayers.Remove(player);
            _queuedPlayers.Enqueue(player);

            player.gameObject.SetActive(false);
        }
    }
}
