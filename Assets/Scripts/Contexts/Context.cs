using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Contexts
{
    public class Context : MonoBehaviour
    {
        [SerializeField]
        private ContextProvider[] _contextProvidersInChildren;

        private void OnValidate()
        {
            Clear();
        }

        private static Context Instance { get; set; }
        public Context()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }
        
        public static T Get<T>() where T : ContextProvider
        {
            if (!Has<T>())
            {
                Debug.LogWarning($"IContextProvider for type {typeof(T).Name} was not found in the registry.");
                return null;
            }
            return (T) Instance._contextProviders[typeof(T)];
        }
        
        public static bool Has<T>() where T : ContextProvider
        {
            return Instance._contextProviders.ContainsKey(typeof(T));
        }

        public static void Clear()
        {
            Instance._contextProviders.Clear();
            Instance.RegisterContextProviders();
#if UNITY_EDITOR
            Instance._contextProvidersInChildren = Instance._contextProviders.Select(pair => (ContextProvider) pair.Value).ToArray();
#endif
        }

        private void RegisterContextProviders()
        {
            IContextProvider[] contextProviders = GetComponentsInChildren<IContextProvider>();
            foreach (IContextProvider contextProvider in contextProviders)
            {
                RegisterProvider(contextProvider);
            }
        }
        
        private void RegisterProvider(IContextProvider contextProvider)
        {
            if (!contextProvider.Active)
            {
                Debug.LogWarning($"IContextProvider for type {contextProvider.GetType().Name} was in active; skipping registration. Most likely component was removed from GameObject.");
                return;
            }
            
            _contextProviders[contextProvider.ProviderType] = contextProvider;
        }

        private Dictionary<Type, IContextProvider> _contextProviders = new Dictionary<Type, IContextProvider>();
    }
}