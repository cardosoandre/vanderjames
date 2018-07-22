using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VDJ.BuilderGame.GameState
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public string battleSceneName;
        public string menuSceneName;

        public PlayerConfig[] data;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }


        public void GoToBattle(PlayerConfig[] data)
        {
            SceneManager.LoadScene(battleSceneName);

            this.data = data;
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
