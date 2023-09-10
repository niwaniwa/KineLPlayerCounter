using System;
using BestHTTP.Extensions;
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

        private const string DEBUG_PREFIX = "[<color=#58ACFA>KineL</color><color=#b44c97>#Counter</color>]";

        [Tooltip("デバッグメッセージを出力します")]
        [SerializeField] private bool debug;
        
        private PlayerCounter[] _listeners;

        [UdonSynced, FieldChangeCallback(nameof(GlobalPlayerCount))] private int globalPayerCount = 0;
        [UdonSynced] private string playerIDs = "";

        private bool isInside = false;
        

        public int GlobalPlayerCount
        {
            get => globalPayerCount;
            set {
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
                if (_listeners[i].name.Equals(listener.name))
                    break;
                temp[i] = _listeners[i];
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
            
            if(debug)
                Debug.Log($"{DEBUG_PREFIX} {player.displayName} enter, {playerIDs}");

            AddPlayer(player.playerId);
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
            
            if(debug)
                Debug.Log($"{DEBUG_PREFIX} {player.displayName} exit");
            
            RemovePlayer(player.playerId);
            CountDown();
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (debug)
            {
                Debug.Log($"{DEBUG_PREFIX} Player left {player.displayName}, check:{CheckID(player.playerId)} ");
                Debug.Log($"{DEBUG_PREFIX} {playerIDs} {player.playerId}");
            }
            
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject))
                return;

            
            if (CheckID(player.playerId))
            {
                RemovePlayer(player.playerId);
                CountDown();
            }

        }

        public override void OnPlayerRespawn(VRCPlayerApi player)
        {
            if (debug)
                Debug.Log($"{DEBUG_PREFIX} respawn");
        }

        public void CountUp()
        {
            GlobalPlayerCount++;
            RequestSerialization();
            UpdateCount();
        }

        public void CountDown()
        {
            GlobalPlayerCount = Mathf.Clamp(GlobalPlayerCount - 1, 0, VRCPlayerApi.GetPlayerCount());
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

        public void AddPlayer(int id)
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject))
                return;
            
            playerIDs += $"{id},";
            RequestSerialization();
        }

        public void RemovePlayer(int id)
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject) || String.IsNullOrEmpty(playerIDs))
                return;
            
            var idArray = playerIDs.Split(',');
            string temp = "";

            int i = 0;
            for (; i < idArray.Length; i++)
            {
                if (Int32.Parse(idArray[i]).Equals(id))
                    break;
                
                temp += $"{idArray[i]},";
            }

            for (; i < idArray.Length - 1; i++)
            {
                if (String.IsNullOrEmpty(idArray[i + 1]))
                    continue;
                
                temp += $"{idArray[i + 1]},";
            }

            playerIDs = temp;
            RequestSerialization();

        }

        public bool CheckID(int id)
        {
            if (String.IsNullOrEmpty(playerIDs))
                return false;
            
            var idArray = playerIDs.Split(',');
            
            for (int i = 0; i < idArray.Length; i++)
                if (Int32.Parse(idArray[i]).Equals(id))
                    return true;
            
            return false;
        }


      
    }
    
}