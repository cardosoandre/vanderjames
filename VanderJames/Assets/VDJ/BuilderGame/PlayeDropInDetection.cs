using UnityEngine;
using Rewired;
using UnityEngine.UI;
using VDJ.BuilderGame.GameState;
using System.Linq;
using System.Collections;

namespace VDJ.BuilderGame {
	public class PlayeDropInDetection : MonoBehaviour {

		public int numberOfPlayers;
		public int[] playerStatus;

        public PlayerConfig[] playerConfigs;

        private Player[] players;
		private ControllerDetection ctlrDetection;

        private bool TriggeredReady = false;

        [Space]
        public GameObject countDownText;
        public GameObject[] canvasStatusText;

		void Awake() {
			ctlrDetection = new ControllerDetection();
			ctlrDetection.connectionDelegate = IncreasePlayerCount;
			ctlrDetection.disconnectionDelegate = DecreasePlayerCount;
            

			playerStatus = new int[3];
			players = new Player[3];

			for (int i = 0; i < players.Length; i++) {
				playerStatus[i] = -1;
				players[i] = ReInput.players.GetPlayer(i);
				print("got player with id = " + players[i].id);
			}
		}

		void Start () {
			print("We have " + numberOfPlayers + " Players, 0 are ready");
		}

		void Update () {
			for (int i = 0; i < numberOfPlayers; i++) {
				if(players[i].GetButtonDown("Action")) {
					playerStatus[i] = 1;
				}
			}


			bool allOk = true;
            for (int i = 0; i < 3; i++) {
                if (playerStatus[i] == 0) {
                    allOk = false;
				}

                Text textScript;
                switch (playerStatus[i])
                {
                    case -1:
                        textScript = canvasStatusText[i].GetComponent<Text>();
                        textScript.text = "";
                        break;
                    case 0:
                        textScript = canvasStatusText[i].GetComponent<Text>();
                        textScript.text = "Player " + i + " NOT ready";
                        break;
                    case 1:
                        textScript = canvasStatusText[i].GetComponent<Text>();
                        textScript.text = "Player " + i + " ready";
                        break;
                }
			}

			if (allOk == true && numberOfPlayers > 0) {
                StartCoroutine(StartGame());
			}
		}
		
        IEnumerator StartGame() {
            countDownText.GetComponent<Text>().text = "Iniciando em 3";
            yield return new WaitForSeconds(3);
            GameStateManager.Instance.GoToBattle(playerConfigs.Take(numberOfPlayers).ToArray());
        }

		void IncreasePlayerCount (int id) {
			numberOfPlayers += 1;
			playerStatus[id] = 0;

			print("player " + id + " added");

			print(numberOfPlayers + " remaining");
		}

		void DecreasePlayerCount (int id) {
			numberOfPlayers -= 1;
			playerStatus[id] = -1;

			print("player " + id + " removed");
			print(numberOfPlayers + " remaining");
		}

		void OnPressButtonDown(InputActionEventData data) {
			print("Someone is ready: " + data.player.id);
		        if(data.GetButtonDown()) {
		        	int id = data.player.id;
		        	playerStatus[id] = 1;

		        	print("Player " + id + " IS READY!");
		        }
		}

		public class ControllerDetection {

			public delegate void ControllerConnectedDelegate(int id);
			public ControllerConnectedDelegate connectionDelegate;

			public delegate void ControllerDisconnectedDelegate(int id);
			public ControllerDisconnectedDelegate disconnectionDelegate;

			public ControllerDetection() {
				// Subscribe to events
		        ReInput.ControllerConnectedEvent += OnControllerConnected;
		        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
		        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
			}

		    // This function will be called when a controller is connected
		    // You can get information about the controller that was connected via the args parameter
		    void OnControllerConnected(ControllerStatusChangedEventArgs args) {
		        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
		        connectionDelegate(args.controllerId);
		    }

		     // This function will be called when a controller is fully disconnected
		     // You can get information about the controller that was disconnected via the args parameter
		     void OnControllerDisconnected(ControllerStatusChangedEventArgs args) {
		        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
		        disconnectionDelegate(args.controllerId);
		    }

		     // This function will be called when a controller is about to be disconnected
		     // You can get information about the controller that is being disconnected via the args parameter
		     // You can use this event to save the controller's maps before it's disconnected
		     void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args) {
		        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
		    }

		    void OnDestroy() {
		        // Unsubscribe from events
		        ReInput.ControllerConnectedEvent -= OnControllerConnected;
		        ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
		        ReInput.ControllerPreDisconnectEvent -= OnControllerPreDisconnect;
		    }
		}
	}
}