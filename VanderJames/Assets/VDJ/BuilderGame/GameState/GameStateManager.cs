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

        public string sceneName;

        public PlayerConfig[] data;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }


        public void GoToBattle(PlayerConfig[] data)
        {
            SceneManager.LoadScene(sceneName);

            this.data = data;
        }
    }
}
