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

        private int localPlayerCount = 0;
        private int questCount = 0;
        private int pcCount = 0;
        
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
                if (isPlatformCountMode)
                {
# if UNITY_ANDROID 
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OnQuestPlayerJoin));
# else 
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OnPCPlayerJoin));
# endif
                }
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

        public void OnQuestPlayerJoin()
        {
            questCount++;
            Debug.Log($"[{nameof(OnQuestPlayerJoin)}] {questCount}, {pcCount}");
        }

        public void OnPCPlayerJoin()
        {
            pcCount++;
            Debug.Log($"[{nameof(OnPCPlayerJoin)}] {questCount}, {pcCount}");
        }

        public void OnQuestPlayerLeft()
        {
            questCount--;
            Debug.Log($"[{nameof(OnQuestPlayerLeft)}] {questCount}, {pcCount}");
        }

        public void OnPCPlayerLeft()
        {
            pcCount--;
            Debug.Log($"[{nameof(OnPCPlayerLeft)}] {questCount}, {pcCount}");
        }

        private void UpdateCounter()
        {
            countText.text = (areaManagement && areas.Length == 0) ? "?" : $"{localPlayerCount}";
            anim.SetFloat("value", localPlayerCount / limit);
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
        



    }
}
