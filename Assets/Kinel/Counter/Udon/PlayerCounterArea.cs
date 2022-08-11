using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Kinel.Counter.Udon
{
    public class PlayerCounterArea : UdonSharpBehaviour
    {

        [CanBeNull] private const string DEBUG_PREFIX = "[[<color=#58ACFA>KineL</color><color=#b44c97>#Counter</color>]";
        
        private PlayerCounter[] _listeners;

        private int playerCount = 0;

        public int PlayerCount
        {
            get => playerCount;
            private set => playerCount = value;
        }
        
        public void RegisterCounter(PlayerCounter listener)
        {
            if (_listeners == null)
                _listeners = new PlayerCounter[0];

            var temp = new PlayerCounter[_listeners.Length + 1];

            for (int i = 0; i < _listeners.Length; i++)
            {
                temp[i] = _listeners[i];
            }
            
            temp[_listeners.Length] = listener;
            
            _listeners = temp;
            
            Debug.Log($"{DEBUG_PREFIX}" + " Register " + $"{listener.name}");
        }
        
        public void UnregisterCounter(PlayerCounter listener)
        {
            var temp = new PlayerCounter[_listeners.Length];
            int i = 0;
            for (; i < _listeners.Length; i++)
            {
                temp[i] = _listeners[i];
                if (_listeners[i].name.Equals(listener.name))
                    break;
            }

            for (; i < _listeners.Length - 1; i++)
            {
                temp[i] = _listeners[i + 1];
            }
    
            _listeners = temp;

            Debug.Log($"{DEBUG_PREFIX}" + " Unregister " + $"{listener.name}");
        }


        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            playerCount++;
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            playerCount--;
        }

        public void UpdateCount()
        {
            foreach (var counter in _listeners)
            {
                counter.OnUpdateAreaCount();
            }
        }
    }
    
}