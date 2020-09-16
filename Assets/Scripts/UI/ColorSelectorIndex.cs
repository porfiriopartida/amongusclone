using Photon.Pun;
using UnityEngine;

public class ColorSelectorIndex : MonoBehaviour
{
    public int Idx;
    
    public void SetColor(ColorSelectorIndex csi)
    {
        SceneStateManager.Instance.SetColor(PhotonNetwork.LocalPlayer, csi.Idx);
    }
}
