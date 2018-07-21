using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace VDJ.BuilderGame.Resources
{
    public class ResourceToken : MonoBehaviour
    {
        public UnityEvent OnConsumed;


        public Transform root;

        public enum Type { Wood, Stone}


        [SerializeField]
        private Type type;



        public Type ResourceType { get { return type; } }

        public void Consume()
        {
            OnConsumed.Invoke();
            Destroy(root.gameObject);
        }
    }
}
