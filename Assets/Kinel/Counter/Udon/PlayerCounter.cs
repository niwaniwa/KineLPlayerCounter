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
    
        private int localPlayerCount = 0;
    
        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            localPlayerCount++;
            UpdateCounter();
            if (Networking.LocalPlayer == player)
            {
                limitText.text = $"{limit}";
            }
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            localPlayerCount--;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            countText.text = $"{localPlayerCount}";
            anim.SetFloat("value", localPlayerCount / limit);
        }



    }
}
