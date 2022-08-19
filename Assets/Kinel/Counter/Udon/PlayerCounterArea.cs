using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Kinel.Counter.Udon
{
    public class PlayerCounterArea : UdonSharpBehaviour
    {

        private const string DEBUG_PREFIX = "[[<color=#58ACFA>KineL</color><color=#b44c97>#Counter</color>]";
        
        private PlayerCounter[] _listeners;

        [UdonSynced, FieldChangeCallback(nameof(GlobalPlayerCount))] private int globalPayerCount = 0;
        private int playerCount = 0;
        

        public int GlobalPlayerCount
        {
            get => globalPayerCount;
            private set {
                globalPayerCount = value;
                playerCount = globalPayerCount;
            }
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
            if (!Networking.IsOwner(player, gameObject))
            {
                if(playerCount == 0)
                    Networking.SetOwner(player, gameObject);
            }

            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CountUp));

        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (!Networking.IsOwner(player, gameObject))
                return;

            playerCount--;
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
            {
                RequestSerialization();
            }
        }
      

        public void CountUp()
        {
            
        }

        public void CoundDown()
        {
            
        }
        

        public void UpdateCount()
        {
            foreach (var counter in _listeners)
            {
                counter.OnUpdateAreaCount();
            }
        }

        public void TakeOwnership()
        {
            
        }
    }
    
}