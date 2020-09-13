public class DeadBodyReport
{
    public PlayerWrapper Reporter;
    
    public PlayerWrapper Body;
    
    
    public DeadBodyReport(string[] uuids)
    {
        this.Reporter = SceneStateManager.Instance.FindPlayer(uuids[0]);
        this.Body = SceneStateManager.Instance.FindPlayer(uuids[1]);
    }
}