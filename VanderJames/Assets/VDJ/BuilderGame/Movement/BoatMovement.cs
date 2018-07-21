using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VDJ.BuilderGame {
	public class BoatMovement : MonoBehaviour {

		public enum Direction { Left, Right };

		public GameObject rightSideDock;
		public GameObject leftSideDock;

		[Space]
		public float speed = 0.5f;
		public Direction direction = Direction.Right;
		public float waitTime = 2.0f;

		[Space]
		public GameObject cargo;

		State state;
		private Rigidbody rb;

		void Start () {
			rb = GetComponent<Rigidbody>();
			SetState(new Crossing(this));
		}
		
		// Update is called once per frame
		void Update () {
			state.Update();
		}

		void ChangeDirection() {
			
			if (direction == Direction.Right) {
				direction = Direction.Left;
			}
			else {
				direction = Direction.Right;
			}
		}

		private void SetState(State value)
		{
		    if (state != null)
		        state.Leave();


		    state = value;

		    if (state != null)
		        state.Begin();
		}

		private void GoToDockedState(){
			SetState(new Docked(this));
		}

		private void GoToCrossingState() {
			SetState(new Crossing(this));
		}

		#region States
	        private abstract class State
	        {
	            protected BoatMovement owner;

	            protected State(BoatMovement owner)
	            {
	                this.owner = owner;
	            }

	            public abstract void Update();

	            public abstract void Begin();
	            public abstract void Leave();
	        }

	        private class Docked: State {
	        	private float timer ;


	        	public Docked(BoatMovement owner) :base(owner) {
	        		timer = owner.waitTime;
	        	}

	        	public override void Begin() {
	        	}

	        	public override void Leave() {

	        	}

	        	public override void Update() {
	        		timer -= Time.deltaTime;

	        		if(timer< 0)
	        		{
	        			owner.ChangeDirection();
	        			owner.GoToCrossingState();
	        		}
	        	}
	        }

	        private class Crossing: State {
	        	public Crossing(BoatMovement owner) :base(owner) {

	        	}

	        	public override void Begin() {

	        	}

	        	public override void Leave() {

	        	}

	        	public override void Update() {
	        		// The step size is equal to speed times frame time.
	        		float step = speed * Time.deltaTime;

	        		// Move our position a step closer to the target.
	        		if (direction == Direction.Right) {
	        			transform.position = Vector3.MoveTowards(transform.position, rightSideDock.transform.position, step);
	        		}
	        		else {
	        			transform.position = Vector3.MoveTowards(transform.position, leftSideDock.transform.position, step);
	        		}

	        		if(transform.position == rightSideDock.transform.position) {
	        			owner.GoToDockedState();
	        		}
	        		else if (transform.position == leftSideDock.transform.position) {
	        			owner.GoToDockedState();
	        		}
	        	}
	        }
	#endregion
	}
}
