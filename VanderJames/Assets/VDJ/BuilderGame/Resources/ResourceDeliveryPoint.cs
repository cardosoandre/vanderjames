using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace VDJ.BuilderGame.Resources
{
    public class ResourceDeliveryPoint : MonoBehaviour
    {
        public ResourceCost cost;

        public ResourceTokenFinder finder;

        public UnityEvent CostMet;
        public UnityEvent ValueChanged;


        private void Awake()
        {
            finder.OnNewObject += Finder_ObjectEntered;
        }

        private void Start()
        {
            ValueChanged.Invoke();
        }

        private void Finder_ObjectEntered(ResourceToken token)
        {
            if(cost.Accepts(token.ResourceType))
            {
                cost.Take(token.ResourceType);
                token.Consume();
                OnTokenConsumed();
            }
        }

        private void OnTokenConsumed()
        {
            ValueChanged.Invoke();

            if (cost.IsMet)
            {
                OnCostMet();
            }
        }

        private void OnCostMet()
        {
            CostMet.Invoke();
        }

        public IEnumerable<ResourceCost.Requirement> CurrentRequirements
        {
            get
            {
                return cost.CurrentRequirements;
            }
        }
    }
}
