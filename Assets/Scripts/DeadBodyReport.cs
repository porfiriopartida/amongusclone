using Photon.Pun;
using Photon.Realtime;

public class DeadBodyReport
{
    public Player Reporter;
    
    public Player Body;
    
    
    public DeadBodyReport(string[] uuids)
    {
        this.Reporter = SceneStateManager.Instance.FindPlayer(uuids[0]);
        this.Body = SceneStateManager.Instance.FindPlayer(uuids[1]);
    }
}