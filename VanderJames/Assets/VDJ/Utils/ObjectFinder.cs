using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VDJ.Utils
{
    public abstract class ObjectFinder<T> : MonoBehaviour
    {
        private List<T> InArea = new List<T>();


        public IEnumerable<T> AllInArea { get { return InArea.Where(x=>x!=null); } }

        public T Target { get; private set; }
        public event Action<TargetChangeEventData<T>> TargetChanged;


        public event Action<T> OnNewObject;

        private bool wasOnZero = false;



        #region Unity Messages
        private void Awake()
        {
            if (!IsTypeValid)
                Debug.LogErrorFormat("Cannot have an object finder of type that's not a component or interface. Type is {0}", typeof(T).ToString());
        }

        private void OnTriggerEnter(Collider other)
        {
            var target = other.GetComponent<T>();

            if (target != null)
                OnObjectEnter(target);
        }

        private void OnTriggerExit(Collider other)
        {
            var target = other.GetComponent<T>();

            if (target != null)
                OnObjectLeft(target);
        }

        private void Update()
        {

            CheckForMainTargetChanges();
        }
        #endregion


        private void OnObjectEnter(T target)
        {
            InArea.Add(target);
            if (OnNewObject != null)
                OnNewObject(target);
        }

        private void OnObjectLeft(T target)
        {
            Debug.LogFormat("Object Left:  {0}", target);
            InArea.Remove(target);
        }

        private void OnTargetChangedFrom(T prevTarget)
        {
            if (TargetChanged != null)
                TargetChanged(new TargetChangeEventData<T>(prevTarget, Target));
        }



        private void CheckForMainTargetChanges()
        {
            CheckDead();
            
            if (InArea.Count == 0)
            {
                if(!wasOnZero)
                {
                    wasOnZero = true;
                    Target = default(T);
                    OnTargetChangedFrom(Target);
                } 
            } else
            {
                wasOnZero = false;
                var best = AllInArea.OrderBy(x => TargetScoreFunction(x)).First();

                if (!Equals(best, Target))
                {
                    var prev = Target;
                    Target = best;
                    OnTargetChangedFrom(Target);
                }
            }
        }

        private void CheckDead()
        {

            for (int i = InArea.Count-1; i >=0 ; i--)
            {
                if(typeof(T).IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    UnityEngine.Object obj = InArea[i] as UnityEngine.Object;
                    if(obj == null)
                    {
                        Debug.Log("Destroyed");
                        InArea.RemoveAt(i);
                    }
                }
            }
        }

        protected abstract float TargetScoreFunction(T t);

        private bool IsTypeValid { get { return IsTypeComponent || IsTypeInterface; } }
        private bool IsTypeComponent { get { return typeof(T).IsSubclassOf(typeof(Component)); } }
        private bool IsTypeInterface { get { return typeof(T).IsInterface; } }
    }


    public class TargetChangeEventData<T>
    {
        public TargetChangeEventData(T prevTarget, T newTarget)
        {
            PrevTarget = prevTarget;
            NewTarget = newTarget;
        }

        public T PrevTarget { get; private set; }
        public T NewTarget { get; private set; }
    }
}
