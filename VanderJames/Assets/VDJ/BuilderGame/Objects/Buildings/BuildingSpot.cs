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
            public Color lineColor;
        }

        public event Action BuiltEvent;

        public SpriteRenderer sr;
        public ResourceDeliveryPoint deliveryPoint;

        public Transform hiddenPortion;
        public Transform lookPos;

        public UnityEvent Activated;
        public UnityEvent Built;

        public Settings settings;

        public LineRenderer lr;

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
            lr.startColor = settings.lineColor;
            lr.endColor = settings.lineColor;
        }

        public void OnBuilt()
        {
            StageManager.Instance.score += settings.score;
            Built.Invoke();
            if (BuiltEvent != null)
                BuiltEvent();
            
            CameraHighlight.instance.LookAtTarget(lookPos, settings.name);
            if (settings.hasText)
                DialogSystem.Instance.Read(settings.lineCharacter, settings.lineText);
        }
    }
}
