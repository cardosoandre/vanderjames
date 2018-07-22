﻿using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VDJ.BuilderGame.GameState;
using VDJ.BuilderGame.Objects.Buildings;

namespace VDJ.BuilderGame.GameState
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance { get; private set; }
        [Serializable]
        public class Settings
        {
            public int startingBuildings = 3;
            public float buildingActivationWait = 10;
            public float totalTime = 120;

            public int startingResourceSpawn = 2;
            public int resourceSpawnsPerTime = 1;
            public float resourceSpawnWait = 3.0f;
            public float spawnRange = .5f;


            public GameObject stonePrefab;
            public GameObject woodPrefab;
        }

        public Settings settings;
        [ReorderableList]
        public List<BuildingSpot> buildingSpots;

        
        public List<Transform> spawningSpots;


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


            StartCoroutine(ActiavtePeriodicalBuildings());
            StartCoroutine(SpawnPeriodicalResources());
            
        }


        private IEnumerator SpawnPeriodicalResources()
        {
            SpawnResources(settings.startingResourceSpawn);

            while (buildingSpots.Count > 0)
            {
                yield return new WaitForSeconds(settings.resourceSpawnWait);
                SpawnResources(settings.resourceSpawnsPerTime);
            }
        }

        private void SpawnResources(int spawnsPerType)
        {
            var randomSort = spawningSpots.OrderBy(x => UnityEngine.Random.value).Take(spawnsPerType * 2).ToArray();

            for (int i = 0; i < randomSort.Length; i++)
            {

                var spawnPoint = randomSort[i].transform.position + UnityEngine.Random.insideUnitSphere * settings.spawnRange;
                if (i%2 == 0)
                {
                    Spawn(settings.stonePrefab, spawnPoint);
                }else
                {
                    Spawn(settings.woodPrefab, spawnPoint);
                }
            }

        }

        private void Spawn(GameObject prefab, Vector3 spawnPoint)
        {
            Instantiate(prefab, spawnPoint, prefab.transform.rotation);
        }

        private IEnumerator ActiavtePeriodicalBuildings()
        {

            for (int i = 0; i < settings.startingBuildings; i++)
            {
                ActivateNextBuilding();
            }

            while(buildingSpots.Count > 0)
            {
                yield return new WaitForSeconds(settings.buildingActivationWait);
                ActivateNextBuilding();
            }
        }

        private void ActivateNextBuilding()
        {
            var spot = buildingSpots[0];
            buildingSpots.RemoveAt(0);
            spot.Activate();
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


        [Button]
        private void LoadAllBuildings()
        {
            buildingSpots = new List<BuildingSpot>(FindObjectsOfType<BuildingSpot>());

        }
    }
}
