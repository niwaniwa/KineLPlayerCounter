using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Kinel.Counter.Udon
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PlayerCounter : UdonSharpBehaviour
    {

        public Text countText, limitText;
        public float limit = 32f;
        public Animator anim;
        public PlayerCounterArea[] areas;
        public bool areaManagement = false; // true = エリアごとの人数カウント, false = ワールド全体の人数カウント
    
        public bool isPlatformCountMode = false, isQuest = false;

        public MultiPlatformPlayerCounter platformCounter;

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

            if (isPlatformCountMode)
            {
                UpdateCounter();
                return;
            }


            localPlayerCount++;
            UpdateCounter();
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (areaManagement)
                return;

            if (isPlatformCountMode)
            {
                // platformCounter.OnPlatformPlayerLeft(player);
                // UpdateCounter();
                return;
            }
            
            localPlayerCount--;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            if(areaManagement)
            {
                countText.text = (areas.Length == 0) ? "?" : $"{localPlayerCount}";
                anim.SetFloat("value", localPlayerCount / limit);
            } 
            else if (isPlatformCountMode && platformCounter != null)
            {
                countText.text = (isQuest) ? $"{platformCounter.QuestCount()}" : $"{platformCounter.PCCount()}";
                anim.SetFloat("value", (isQuest ? platformCounter.QuestCount() : platformCounter.PCCount()) / limit);
            }
            else
            {
                countText.text = $"{localPlayerCount}";
                anim.SetFloat("value", localPlayerCount / limit);
            }
            
        }

        public void OnUpdateAreaCount()
        {
            localPlayerCount = 0;
            foreach (var area in areas)
            {
                localPlayerCount += area.GlobalPlayerCount;
            }

            UpdateCounter();
        }

        public void OnUpdatePlatformCount()
        {
            UpdateCounter();
        }
        



    }
}
