using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.GameState
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        public IEnumerable<PlayerConfig> PlayerData { get { return data.AsEnumerable(); } }
        public PlayerConfig[] data;

        public List<Transform> spawnPoints;


        public GameObject playerPrefab;

        public List<PlayerController> players = new List<PlayerController>();

        private void Awake()
        {
            Debug.Assert(Instance == null);
            Instance = this;
        }

        public void SetPlayerData(PlayerConfig[] data)
        {
            this.data = data;
        }

        public void SpawnPlayers()
        {
            foreach (var data in PlayerData)
            {
                SpawnPlayerWith(data);
            }
        }

        public void ActivatePlayers()
        {
            foreach (var player in players)
            {
                player.Activate();
            }
        }

        private void SpawnPlayerWith(PlayerConfig data)
        {
            var player = Instantiate(playerPrefab).GetComponent<PlayerController>();

            player.transform.position = spawnPoints[data.controllerIndex].position;
            player.GoToInactive();

            players.Add(player);

            player.Init(data);
            player.gameObject.SetActive(true);
        }

        public void StopAllPlayers()
        {
            foreach (var player in players)
            {
                player.GoToInactive();
            }
        }
    }


    [Serializable]
    public class PlayerConfig
    {
        public int controllerIndex;

    }
}
