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
        public bool isPlatformCountMode = false, isQuest = false;

        private int localPlayerCount = 0;
        private int questCount = 0;
        private int pcCount = 0;
        
        
    
        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            localPlayerCount++;
            if (Networking.LocalPlayer == player)
            {
                limitText.text = $"{limit}";
                if (isPlatformCountMode)
                {
# if UNITY_ANDROID 
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OnQuestPlayerJoin));
# else 
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OnPCPlayerJoin));
# endif
                }
            }
            
            UpdateCounter();
        }

        /// <summary>
        /// これLocalUserがLeftしたとき発火する...?
        /// </summary>
        /// <param name="player"></param>
        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            localPlayerCount--;
            if (Networking.LocalPlayer == player)
            {
                if (isPlatformCountMode)
                {
# if UNITY_ANDROID 
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OnQuestPlayerLeft));
# else 
                    SendCustomNetworkEvent(NetworkEventTarget.All, nameof(OnPCPlayerLeft));
# endif
                }
            }
            
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
            countText.text = isPlatformCountMode ? $"{(isQuest ? questCount : pcCount)}" : $"{localPlayerCount}";
            anim.SetFloat("value", (isPlatformCountMode ? (isQuest ? questCount : pcCount) : localPlayerCount) / limit);
        }



    }
}
