using UnityEngine;
using Rewired;
using UnityEngine.UI;
using VDJ.BuilderGame.GameState;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace VDJ.BuilderGame {
	public class PlayeDropInDetection : MonoBehaviour {
        
        public PlayerConfig[] playerConfigs;
        public int numberOfJointPlayers = 0;
        public bool[] jointPlayers = new bool[3] { false, false, false };

        private Player[] players = new Player[3];
		private ControllerDetection ctlrDetection;

        [Space]
        public GameObject countDownText;
        public GameObject[] canvasStatusText;

		void Awake() {
            ctlrDetection = new ControllerDetection(this);

            int i = 0;
            foreach (Player p in ReInput.players.Players) {
                players[i] = p;
                i++;
            }
		}

		void Start () {
			
		}

		void Update () {

            for (int i = 0; i < 3; i++) {
				if(players[i].GetButtonDown("Action")) {
                    if (jointPlayers[i]) {
                        StartCoroutine(StartGame());
                    }

                    jointPlayers[i] = true;
                    ++numberOfJointPlayers;

                    canvasStatusText[i].GetComponent<Text>().text = "Jogador " + i + " pronto!";
                }
			}

            if(numberOfJointPlayers > 0) {
                countDownText.GetComponent<Text>().text = "Pressione o botão \nnovamente para começar!";
            }
		}
		
        IEnumerator StartGame() {
            countDownText.GetComponent<Text>().text = "Iniciando em 3";
            yield return new WaitForSeconds(1);
            countDownText.GetComponent<Text>().text = "Iniciando em 2";
            yield return new WaitForSeconds(1);
            countDownText.GetComponent<Text>().text = "Iniciando em 1";
            yield return new WaitForSeconds(1);
            GameStateManager.Instance.GoToBattle(playerConfigs.Take(numberOfJointPlayers).ToArray());
        }

        private class ControllerDetection {

            private PlayeDropInDetection owner;

            public ControllerDetection(PlayeDropInDetection playerDropIn) {
				// Subscribe to events
		        ReInput.ControllerConnectedEvent += OnControllerConnected;
		        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
		        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

                owner = playerDropIn;
			}

		    // This function will be called when a controller is connected
		    // You can get information about the controller that was connected via the args parameter
		    void OnControllerConnected(ControllerStatusChangedEventArgs args) {
		        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
               
                AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
		    }

		     // This function will be called when a controller is fully disconnected
		     // You can get information about the controller that was disconnected via the args parameter
		     void OnControllerDisconnected(ControllerStatusChangedEventArgs args) {
		        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
		    }

		     // This function will be called when a controller is about to be disconnected
		     // You can get information about the controller that is being disconnected via the args parameter
		     // You can use this event to save the controller's maps before it's disconnected
		     void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args) {
		        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
		    }

            void AssignJoystickToNextOpenPlayer(Joystick j)
            {
                int i = 0;
                foreach (Player p in ReInput.players.Players)
                {
                    if (p.controllers.joystickCount > 0 || owner.jointPlayers[i]) {
                        continue; // player already has a joystick
                    }
                    p.controllers.AddController(j, true); // assign joystick to player
                    i++;
                    return;
                }
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