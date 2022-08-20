using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Kinel.Counter.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PlayerCounterArea : UdonSharpBehaviour
    {

        private const string DEBUG_PREFIX = "[[<color=#58ACFA>KineL</color><color=#b44c97>#Counter</color>]";
        
        private PlayerCounter[] _listeners;

        [UdonSynced, FieldChangeCallback(nameof(GlobalPlayerCount))] private int globalPayerCount = 0;
        private int playerCount = 0;

        private bool isInside = false;
        

        public int GlobalPlayerCount
        {
            get => globalPayerCount;
            private set {
                globalPayerCount = value;
                playerCount = globalPayerCount;
                UpdateCount();
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
            if (!Utilities.IsValid(player))
                return;
            
            if (!Networking.LocalPlayer.Equals(player))
                return;
            
            if (isInside)
                return;
            
            if (!Networking.IsOwner(player, gameObject))
            {
                if(playerCount == 0)
                    Networking.SetOwner(player, gameObject);
            }
            
            Debug.Log($"{DEBUG_PREFIX} {player.displayName}");
            isInside = true;
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CountUp));
            
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (!Utilities.IsValid(player))
                return;
            
            if (!Networking.LocalPlayer.Equals(player))
                return;
            // if (!Networking.IsOwner(player, gameObject))
            //     return;

            // playerCount--;
            
            if (!isInside)
                return;
            
            Debug.Log($"{DEBUG_PREFIX} {player.displayName}");
            isInside = false;
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(CountDown));
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            
        }
      

        public void CountUp()
        {
            playerCount++;
            GlobalPlayerCount = playerCount;
            RequestSerialization();
            UpdateCount();
        }

        public void CountDown()
        {
            playerCount--;
            GlobalPlayerCount = playerCount;
            RequestSerialization();
            UpdateCount();
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