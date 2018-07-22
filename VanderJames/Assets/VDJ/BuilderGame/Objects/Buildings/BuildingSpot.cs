using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using VDJ.BuilderGame.GameState;
using VDJ.BuilderGame.Resources;

namespace VDJ.BuilderGame.Objects.Buildings
{
    public class BuildingSpot : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            public ResourceCost cost;
            public Sprite sprite;
            public string name;
            public int score;
            public bool hasText;
            public string lineCharacter;
            public string lineText;
        }


        public SpriteRenderer sr;
        public ResourceDeliveryPoint deliveryPoint;

        public Transform hiddenPortion;

        public UnityEvent Activated;
        public UnityEvent Built;

        public Settings settings;


        public void Activate()
        {
            Init();

            hiddenPortion.gameObject.SetActive(true);
            Activated.Invoke();
        }


        private void Init()
        {
            sr.sprite = settings.sprite;
            deliveryPoint.cost = settings.cost;
        }

        public void OnBuilt()
        {
            StageManager.Instance.score += settings.score;
            Built.Invoke();
            
            CameraHighlight.instance.LookAtTarget(transform, settings.name);
            if (settings.hasText)
                DialogSystem.Instance.Read(settings.lineCharacter, settings.lineText);
        }
    }
}
