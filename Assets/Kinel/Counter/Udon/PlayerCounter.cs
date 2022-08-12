using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Kinel.Counter.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class PlayerCounter : UdonSharpBehaviour
    {

        public Text countText, limitText;
        public float limit = 32f;
        public Animator anim;
        public PlayerCounterArea[] areas;
        public bool areaManagement = false; // true = エリアごとの人数カウント, false = ワールド全体の人数カウント
    
        private int localPlayerCount = 0;
        

        public void Start()
        {
            foreach (PlayerCounterArea area in areas)
            {
                area.RegisterCounter(this);
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (Networking.LocalPlayer == player)
            {
                limitText.text = $"{limit}";
                UpdateCounter();
            }


            if (areaManagement)
                return;
            
            localPlayerCount++;
            UpdateCounter();
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (areaManagement)
                return;
            
            localPlayerCount--;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            countText.text = $"{localPlayerCount}";
            anim.SetFloat("value", localPlayerCount / limit);
        }

        public void OnUpdateAreaCount()
        {
            localPlayerCount = 0;
            foreach (var area in areas)
            {
                localPlayerCount += area.PlayerCount;
            }
        }
        



    }
}
