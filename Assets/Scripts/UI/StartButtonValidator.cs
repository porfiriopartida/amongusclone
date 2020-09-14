using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonValidator : MonoBehaviour
{
    public GameConfiguration GameConfiguration;
    public Button button;
    public void Validate()
    {
        button.enabled = IsValid();
    }

    private bool IsValid()
    {
        if (GameConfiguration.TotalImpostors > PhotonNetwork.PlayerList.Length)
        {
            //Otherwise you get an infinite loop while generating the impostors.
            return false;
        }
        
        if ((PhotonNetwork.PlayerList.Length/2) < GameConfiguration.TotalImpostors)
        {
            //Otherwise you get an infinite loop while generating the impostors.
            return false;
        }


        return true;
    }
}
