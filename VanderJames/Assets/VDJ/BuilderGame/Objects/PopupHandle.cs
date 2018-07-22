using System;
using UnityEngine;
using UnityEngine.Events;

namespace VDJ.BuilderGame.Objects
{
    public class PopupHandle : Handle
    { 
        
        [Serializable]
        public class Settings
        {
            public float pullTime = 1.0f;
            public float backTime = .5f;

            public bool DoesLock = true;
        }

        //Dependencies
        [SerializeField]
        private Popup anchor;
        [SerializeField]
        private Popup handle;
        [SerializeField]
        private Popup popup;
        [SerializeField]
        private Settings settings;

        //State
        private bool grabbed = false;
        private float pull = 0.0f;

        private IPullProvider pullProvider;
        private bool isLocked = false;

        public override bool CanBeGrabbed { get { return !grabbed; } }
        public override Transform Anchor { get { return anchor.transform; } }

        public UnityEvent Locked;
        public UnityEvent Grabbed;
        public UnityEvent Released;

        public override void OnGrab(IPullProvider pullProvider)
        {
            this.pullProvider = pullProvider;
            grabbed = true;
            Grabbed.Invoke();
        }
        public override void OnLeave()
        {
            grabbed = false;
            Released.Invoke();
        }

        #region Unity Messages

        private void Update()
        {
            if (!isLocked)
            {
                ProcessMovement();
                UpdatePopups();


                if (settings.DoesLock && pull >= 1.0f)
                {
                    Lock () ;
                }
            }
        }

        private void Lock()
        {
            isLocked = true;
            Locked.Invoke();
        }


        #endregion



        private void ProcessMovement()
        {
            if (grabbed)
            {
                pull += Mathf.Abs(PullVel * pullProvider.Pull.z) * Time.deltaTime;
            }
            else
            {
                pull -= BackVel * Time.deltaTime;
            }

            pull = Mathf.Clamp(pull, 0, 1);
        }

        private void UpdatePopups()
        {
            anchor.progress = pull;
            popup.progress = pull;
            handle.progress = pull; 
        }

        private float PullVel { get { return 1 / settings.pullTime; } }
        private float BackVel { get { return 1 / settings.backTime; } }

    }
}