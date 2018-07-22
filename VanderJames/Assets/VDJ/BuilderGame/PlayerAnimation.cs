using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VDJ.BuilderGame.Movement;

namespace VDJ.BuilderGame
{
    public class PlayerAnimation : MonoBehaviour
    {
        
        public PlayerInput input;
        private Animator animator;
        private SpriteRenderer sr;

        private void Start()
        {
            animator = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            animator.SetBool("walking", (input.Horizontal != 0 || input.Vertical != 0));
            animator.SetBool("pulling", input.MainButton);

            if (!animator.GetBool("pulling"))
            {
                if (input.Horizontal > 0) sr.flipX = false;
                if (input.Horizontal < 0) sr.flipX = true;
            }else{
                if (input.Horizontal > 0) sr.flipX = true;
                if (input.Horizontal < 0) sr.flipX = false;
            }
        }
    }
}
