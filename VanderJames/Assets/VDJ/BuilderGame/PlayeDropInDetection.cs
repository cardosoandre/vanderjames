using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace VDJ.BuilderGame {
	public class PlayeDropInDetection : MonoBehaviour {

		public int numberOfPlayers = 0;
		public List<int> playerStatus = new List<int>();

		private List<Player> players = new List<Player>();
		private ControllerDetection ctlrDetection;

		void Awake() {
			ctlrDetection = new ControllerDetection();
			ctlrDetection.connectionDelegate = IncreasePlayerCount;
			ctlrDetection.disconnectionDelegate = DecreasePlayerCount;
		}

		void Start () {
			print("We have " + numberOfPlayers + " Players, 0 are ready");
		}

		void Update () {
			
		}
		
		void IncreasePlayerCount () {
			numberOfPlayers += 1;
			playerStatus.Add(-1);

			Player newPlayer = ReInput.players.GetPlayer(numberOfPlayers - 1);
			newPlayer.AddInputEventDelegate(OnPressButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Action");
			players.Add(newPlayer);
			
			print("player added");
		}

		void DecreasePlayerCount (int id) {
			numberOfPlayers -= 1;
			playerStatus.RemoveAt(id);
		}

		void OnPressButtonDown(InputActionEventData data) {
			print("Someone is ready");
		        if(data.GetButtonDown()) {
		        	int id = data.player.id;
		        	playerStatus[id] = 1;

		        	print("Player " + id + " IS READY!");
		        }
		}

		public class ControllerDetection {

			public delegate void ControllerConnectedDelegate();
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
		        connectionDelegate();
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