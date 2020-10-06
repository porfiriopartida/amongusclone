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
        SceneStateManager.Instance.EnableRegularInput();
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
        SabotageMap.SetActive(false);
        AdminMiniMap.SetActive(false);
        SceneStateManager.Instance.EnableRegularInput();
    }
    public void CloseMinimap()
    {
        SabotageMap.SetActive(false);
        AdminMiniMap.SetActive(false);
        MiniMap.SetActive(false);
    }
    public void ToggleAdminMiniMap()
    {
        MiniMap.SetActive(false);
        SabotageMap.SetActive(false);
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
        SabotageMap.SetActive(false);
        
        SceneStateManager.Instance.EnableRegularInput();
    }
}
