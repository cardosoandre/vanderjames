using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.GameState;

namespace VDJ.BuilderGame.GameState
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance { get; private set; }

        

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Debug.Log("Starting Game Coroutine");

            if (GameStateManager.Instance != null)
                PlayerManager.Instance.SetPlayerData(GameStateManager.Instance.data);

            StartCoroutine(GameCoroutine());
        }

        private IEnumerator GameCoroutine()
        {
            Debug.Log("Spawned Players");
            PlayerManager.Instance.SpawnPlayers();

            yield return GameStartCoroutine();

            Debug.Log("Activating Players");
            PlayerManager.Instance.ActivatePlayers();
        }


        private IEnumerator GameStartCoroutine()
        {
            
            Debug.Log("Starting Game Begin Coroutine");
            GameUI.Instance.EnableCountdown();
            GameUI.Instance.SetCountdown(3);
            yield return new WaitForSeconds(1.0f);
            GameUI.Instance.SetCountdown(2);
            yield return new WaitForSeconds(1.0f);
            GameUI.Instance.SetCountdown(1);
            yield return new WaitForSeconds(1.0f);
            GameUI.Instance.DisableCountdown();
        }
    }
}
