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


        public IEnumerable<T> AllInArea { get { return InArea.AsEnumerable(); } }

        public T Target { get; private set; }
        public event Action<TargetChangeEventData<T>> TargetChanged;

        

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
        }

        private void OnObjectLeft(T target)
        {
            InArea.Remove(target);
        }

        private void OnTargetChangedFrom(T prevTarget)
        {
            if (TargetChanged != null)
                TargetChanged(new TargetChangeEventData<T>(prevTarget, Target));
        }



        private void CheckForMainTargetChanges()
        {

            var best = InArea.Count == 0? default(T) : AllInArea.OrderBy(x => TargetScoreFunction(x)).First();
            if (!Equals(best, Target))
            {
                var prev = Target;
                Target = best;
                OnTargetChangedFrom(Target);
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
