using player;
using player.settings;
using System.Collections.Generic;
using UnityEngine;
using utils;

namespace application
{
    public class PlayerSpawner : MonoBehaviour
    {
        private List<PlayerController> _activePlayers = new List<PlayerController>();
        private PoolController<PlayerController> _queuedPlayers;

        public int PlayersActive => _activePlayers.Count;

        private bool _initialized = false;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            _queuedPlayers = new PoolController<PlayerController>(1, prefab: PlayerSettingsSO.Instance.Prefab);
        }

        public void Spawn()
        {
            PlayerController newPlayer = _queuedPlayers.Dequeue();
            newPlayer.gameObject.SetActive(true);
            newPlayer.transform.SetPositionAndRotation(Vector2.zero, Quaternion.identity);

            _activePlayers.Add(newPlayer);
        }

        public void DestroyAll()
        {
            for (int i = _activePlayers.Count - 1; i >= 0; i--)
            {
                Destroy(_activePlayers[i]);
            }
        }

        public void Destroy(PlayerController player)
        {
            _activePlayers.Remove(player);
            _queuedPlayers.Enqueue(player);

            player.gameObject.SetActive(false);
        }
    }
}
