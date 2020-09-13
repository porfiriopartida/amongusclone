using Photon.Realtime;
using UI;
using UnityEngine;

public class DiscussionPanelManager : MonoBehaviour
{
    public GameObject RootPanel;
    public GameObject PlayerPanelContainer;
    public GameObject PlayerPanelPrefab;

    public GameObject AddPlayer(PlayerWrapper player, bool isLocalAlive)
    {
        GameObject playerGo = Instantiate(PlayerPanelPrefab, PlayerPanelContainer.transform);
        PlayerPanelController p = playerGo.GetComponent<PlayerPanelController>();
        p.SetPlayer(player, isLocalAlive);
        return playerGo;
    }
}
