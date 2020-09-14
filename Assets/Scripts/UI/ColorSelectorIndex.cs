using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ColorSelectorIndex : MonoBehaviour
{
    public int Idx;
    public GameObject Taken;

    private void Start()
    {
        //Taken.SetActive(false);
    }

    public void SetColor(ColorSelectorIndex csi)
    {
        List<int> takenColors = SceneStateManager.Instance.GetTakenColors();
        if (!takenColors.Contains(csi.Idx))
        {
            SceneStateManager.Instance.SetColor(PhotonNetwork.LocalPlayer, csi.Idx);
        }
    }
}
