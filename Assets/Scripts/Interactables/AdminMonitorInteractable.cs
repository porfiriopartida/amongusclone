public class AdminMonitorInteractable : UseInteractable
{
    public override void Interact()
    {
        UIMapManager.Instance.ToggleAdminMiniMap();
    }
}
