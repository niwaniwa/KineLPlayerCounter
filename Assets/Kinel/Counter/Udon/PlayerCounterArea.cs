using System;
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

        private bool isInside = false;
        

        public int GlobalPlayerCount
        {
            get => globalPayerCount;
            private set {
                globalPayerCount = value;
                UpdateCount();
            }
        }

        public void Start()
        {

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

            if (Networking.LocalPlayer.Equals(player))
                isInside = true;

            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject))
                return;
            
            Debug.Log($"{DEBUG_PREFIX} {player.displayName} join");


            
            CountUp();
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (!Utilities.IsValid(player))
                return;
            
            if (Networking.LocalPlayer.Equals(player))
                isInside = false;
            
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject))
                return;
            
            Debug.Log($"{DEBUG_PREFIX} {player.displayName} exit");
            
            CountDown();
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            Debug.Log($"------------------------------");
            Debug.Log($"-|        Player Left       |-");
            Debug.Log($"------------------------------");
            Debug.Log($"-|                          |-");
            Debug.Log($"-|           Name           |-");
            // Debug.Log($"-|  {player.displayName}  |-");
            Debug.Log($"-|   {Utilities.IsValid(player)}     |-");
            Debug.Log($"-|                          |-");
            Debug.Log($"------------------------------");
            


            if (!Networking.IsOwner(player, gameObject))
                return;

        }

        public override void OnPlayerRespawn(VRCPlayerApi player)
        {
            Debug.Log($"{DEBUG_PREFIX} respawn");
        }

        public void CountUp()
        {
            globalPayerCount++;
            RequestSerialization();
            UpdateCount();
        }

        public void CountDown()
        {
            globalPayerCount = Mathf.Clamp(globalPayerCount--, 0, VRCPlayerApi.GetPlayerCount());
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