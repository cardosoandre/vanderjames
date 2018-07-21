using System;
using UnityEngine;

namespace VDJ.BuilderGame.Objects
{
    public class PopupHandle : MonoBehaviour
    { 
        
        [Serializable]
        public class Settings
        {
            public float pullTime = 1.0f;
            public float backTime = .5f;
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

        public bool CanBeGrabbed { get { return !grabbed; } }
        public Transform Anchor { get { return anchor.transform; } }

        public void OnGrab(IPullProvider pullProvider)
        {
            this.pullProvider = pullProvider;
            grabbed = true;
        }
        public void OnLeave()
        {
            grabbed = false;
        }

        #region Unity Messages

        private void Update()
        {
            ProcessMovement();
            UpdatePopups();
        }


        #endregion



        private void ProcessMovement()
        {
            if (grabbed)
            {
                Debug.Log(pullProvider.Pull.z);
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

        public interface IPullProvider
        {
            Vector3 Pull { get; }
        }
    }
}