﻿using System;
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


        private void Awake()
        {
            finder.OnNewObject += Finder_ObjectEntered;
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
            if(cost.IsMet)
            {
                OnCostMet();
            }
        }

        private void OnCostMet()
        {
            CostMet.Invoke();
        }
    }
}