using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace VDJ.BuilderGame {
	public class PlayeDropInDetection : MonoBehaviour {

		public int numberOfPlayers;
		public int[] playerStatus;

		private Player[] players;
		private ControllerDetection ctlrDetection;

		void Awake() {
			ctlrDetection = new ControllerDetection();
			ctlrDetection.connectionDelegate = IncreasePlayerCount;
			ctlrDetection.disconnectionDelegate = DecreasePlayerCount;

			numberOfPlayers = 0;
			playerStatus = new int[4];
			players = new Player[4];

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
					// playerStatus.Insert(player.id, 1);
					// playerStatus.RemoveAt(player.id + 1);
					playerStatus[i] = 1;
				}
			}


			bool allOk = true;
			foreach (int status in playerStatus) {
				if (status == 0) {
					allOk = false;
				}
			}

			if (allOk == true && numberOfPlayers > 0) {
				//initialize game with
				print("GAME IS READY TO BEGIN!!!!");
			}
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