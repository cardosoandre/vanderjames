using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
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
        }


        public SpriteRenderer sr;
        public ResourceDeliveryPoint deliveryPoint;

        public Transform hiddenPortion;

        public UnityEvent Activated;


        public void Init(Settings settings)
        {
            sr.sprite = settings.sprite;
            deliveryPoint.cost = settings.cost;

        }

        public void Activate()
        {
            hiddenPortion.gameObject.SetActive(true);
            Activated.Invoke();
        }
    }
}
