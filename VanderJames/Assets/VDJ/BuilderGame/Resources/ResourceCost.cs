using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.BuilderGame.Resources
{
    [Serializable]
    public class ResourceCost
    {
        [Serializable]
        public class Requirement
        {
            public ResourceToken.Type type;
            public int quantity;
        }

        private Dictionary<ResourceToken.Type, int> consumedResources = new Dictionary<ResourceToken.Type, int>();
        [SerializeField]
        private List<Requirement> requirements = new List<Requirement>();


        public bool IsMet
        {
            get
            {
                foreach (var requirement in requirements)
                {
                    Debug.Assert(requirement.quantity >= CurrentCount(requirement.type));
                    if(requirement.quantity > CurrentCount(requirement.type))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool Accepts(ResourceToken.Type type)
        {
            return CurrentCount(type) < RequiredCount(type);
        }
        public void Take(ResourceToken.Type type)
        {
            if (!consumedResources.ContainsKey(type))
                consumedResources[type] = 0;
            consumedResources[type]++;
        }

        private int RequiredCount(ResourceToken.Type val)
        {
            if(requirements.Any(x=>x.type == val))
            {
                return requirements.First(x => x.type == val).quantity;
            }else
            {
                return 0;
            }
        }

        private int CurrentCount(ResourceToken.Type type)
        {
            int value = 0;
            consumedResources.TryGetValue(type, out value);
            return value;
        }

        
        public IEnumerable<Requirement> CurrentRequirements
        {
            get
            {
                var list = new List<Requirement>();

                foreach (var item in requirements)
                {
                    var req = new Requirement()
                    {
                        type = item.type,
                        quantity = (RequiredCount(item.type) - CurrentCount(item.type))
                    };

                    if (req.quantity > 0)
                        list.Add(req);

                }
                return list.AsEnumerable();
            }
        }
    }
}
