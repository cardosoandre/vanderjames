using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using VDJ.BuilderGame.GameState;
using VDJ.BuilderGame.Objects.Buildings;

namespace VDJ.BuilderGame.GameState
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance { get; private set; }
        public float RemainingTime { get { return Timer; } }
        public bool TimerStarted { get { return timerStarted; } }

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

        
        //public List<Transform> spawningSpots;


        public List<Transform> stoneSpawns;

        public List<Transform> woodSpawns;

        private bool timerStarted;
        private float Timer;
        private int remainingSpotsCount;

        public float score;
        private bool allSpotsBuilt = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ListenToBuildingSpots();

            Debug.Log("Starting Game Coroutine");

            if (GameStateManager.Instance != null)
                PlayerManager.Instance.SetPlayerData(GameStateManager.Instance.data);

            StartCoroutine(GameCoroutine());
        }

        private void ListenToBuildingSpots()
        {
            remainingSpotsCount = buildingSpots.Count;

            foreach (var spot in buildingSpots)
            {
                spot.BuiltEvent+= OnBuildingBuilt;
            }
        }

        private void OnBuildingBuilt()
        {
            remainingSpotsCount--;
            if(remainingSpotsCount <= 0)
            {
                OnAllSpotsBuilt();
            }
        }

        [Button("Fake Ending")]
        private void OnAllSpotsBuilt()
        {
            allSpotsBuilt = true;
        }

        public void Update()
        {
            Timer -= Time.deltaTime;
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

            StartTimer();

            yield return new WaitUntil(HasEndCondition);



            PlayerManager.Instance.StopAllPlayers();


            if(allSpotsBuilt)
                GameUI.Instance.ShowVictory();
            

            yield return new WaitForSeconds(6);

            GameStateManager.Instance.GoToMenu();
        }


        private void StartTimer()
        {
            timerStarted = true;
            Timer = settings.totalTime;
        }
        private bool HasEndCondition()
        {
            return allSpotsBuilt || (timerStarted && Timer < 0);
        }

        private IEnumerator SpawnPeriodicalResources()
        {
            SpawnResources(settings.startingResourceSpawn);

            while (true)
            {
                yield return new WaitForSeconds(settings.resourceSpawnWait);
                SpawnResources(settings.resourceSpawnsPerTime);
            }
        }

        private void SpawnResources(int count)
        {
            SpawnResources(stoneSpawns, settings.stonePrefab, count);

            SpawnResources(woodSpawns, settings.woodPrefab, count);
        }

        private void SpawnResources(List<Transform> spots, GameObject prefab, int count)
        {
            var randomSort = spots.OrderBy(x => UnityEngine.Random.value).Take(count).ToArray();

            for (int i = 0; i < randomSort.Length; i++)
            {
                var circle = UnityEngine.Random.insideUnitCircle;
                
                var spawnPoint = randomSort[i].transform.position + new Vector3(circle.x, 0, circle.y) * settings.spawnRange;
                
                Spawn(prefab, spawnPoint);
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
