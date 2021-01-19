
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerCounter : UdonSharpBehaviour
{

    [SerializeField] private Text countText, limitText;
    [SerializeField] private float limit = 32f;
    [SerializeField] private Animator anim;
    
    private int localPlayerCount = 0;
    //[UdonSynced] private int syncedPlayerCount = 0;
    
    public void Start()
    {
    }

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
