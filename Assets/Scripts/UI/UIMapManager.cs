using LopapaGames.Common.Core;
using UnityEngine;

public class UIMapManager : Singleton<UIMapManager>
{
    public GameObject MiniMap;
    public GameObject AdminMiniMap;
    public GameObject SabotageMap;

    public void ToggleSabotageMap()
    {
        SabotageMap.SetActive(!SabotageMap.activeSelf);
        AdminMiniMap.SetActive(false);
        MiniMap.SetActive(false);
    }
    public void CloseSabotageMap()
    {
        SabotageMap.SetActive(false);
        AdminMiniMap.SetActive(false);
        MiniMap.SetActive(false);
    }
    public void ToggleMiniMap()
    {
        MiniMap.SetActive(!MiniMap.activeSelf);
        AdminMiniMap.SetActive(false);
    }
    public void CloseMinimap()
    {
        MiniMap.SetActive(false);
    }
    public void ToggleAdminMiniMap()
    {
        MiniMap.SetActive(false);
        AdminMiniMap.SetActive(!AdminMiniMap.activeSelf);

        if (AdminMiniMap.activeSelf)
        {
            SceneStateManager.Instance.DisableRegularInput();
        }

    }
    public void CloseAdminMinimap()
    {
        MiniMap.SetActive(false);
        AdminMiniMap.SetActive(false);
        SceneStateManager.Instance.EnableRegularInput();
    }
}
