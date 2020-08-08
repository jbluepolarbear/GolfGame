using System;
using UnityEngine;

namespace Contexts
{
    [ExecuteInEditMode]
    public abstract class ContextProvider : MonoBehaviour, IContextProvider
    {
        public abstract Type ProviderType { get; }
        public abstract bool Active { get; }
    }
    
    public class ContextProvider<T> : ContextProvider
    {
        public override Type ProviderType => typeof(T);
        public override bool Active => _active;

        private void OnValidate()
        {
            IContextProvider self = GetComponent<IContextProvider>();
            Context.Clear();
        }
        
#if UNITY_EDITOR
        private void OnDestroy()
        {
            _active = false;
            Context.Clear();
        }
#endif

        private bool _active = true;
    }
}