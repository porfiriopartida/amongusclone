using System;
using LopapaGames.Common.Core;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    public GameConfiguration GameConfiguration;

    public GameEvent ConfigurationChanged;

    public Text numberOfImpostors;
    public Text PlayerMovSpeed;
    
    public Text CrewmateVision;
    public Text ImpostorVision;
    public Text VoteTime;

    public Text KillCooldown;
    
    public Text LongTasks;
    public Text MediumTasks;
    public Text ShortTasks;

    public Text EmergencyCount;
    public Text EmergencyCooldown;
    private void OnEnable()
    {
        Refresh();
        ConfigurationChanged.Raise();
    }

    public void Refresh()
    {
        numberOfImpostors.text = "0" + GameConfiguration.TotalImpostors;
        PlayerMovSpeed.text = GameConfiguration.MovSpeed.ToString();
        CrewmateVision.text = GameConfiguration.VisionRange.ToString();
        ImpostorVision.text = GameConfiguration.ImpostorVisionRange.ToString();
        VoteTime.text = GameConfiguration.VoteTime.ToString();

        KillCooldown.text = GameConfiguration.KillCooldown.ToString();
        
        LongTasks.text = GameConfiguration.LongTask.ToString();
        MediumTasks.text = GameConfiguration.MidTask.ToString();
        ShortTasks.text = GameConfiguration.ShortTask.ToString();
        
        EmergencyCount.text = GameConfiguration.EmergencyCount.ToString();
        EmergencyCooldown.text = GameConfiguration.EmergencyCooldown.ToString();
    }

    public void AddImpostor()
    {
        if (GameConfiguration.TotalImpostors < 5)
        {
            GameConfiguration.TotalImpostors++;
            ConfigurationChanged.Raise();
        }
    }

    public void RemoveImpostor()
    {
        if (GameConfiguration.TotalImpostors > 1)
        {
            GameConfiguration.TotalImpostors--;
            ConfigurationChanged.Raise();
        }
    }
    public void AddMovSpeed()
    {
        if (GameConfiguration.MovSpeed < 10)
        {
            GameConfiguration.MovSpeed += .25f;
            ConfigurationChanged.Raise();
        }
    }

    public void ReduceMovSpeed()
    {
        if (GameConfiguration.MovSpeed > .25)
        {
            GameConfiguration.MovSpeed -= .25f;
            ConfigurationChanged.Raise();
        }
    }

    public void VisionAdd()
    {
        if (GameConfiguration.VisionRange < 10)
        {
            GameConfiguration.VisionRange += .25f;
            ConfigurationChanged.Raise();
        }
    }

    public void VisionSubstract()
    {
        if (GameConfiguration.VisionRange > .25)
        {
            GameConfiguration.VisionRange -= .25f;
            ConfigurationChanged.Raise();
        }
    }
    public void VisionImpostorAdd()
    {
        if (GameConfiguration.ImpostorVisionRange < 10)
        {
            GameConfiguration.ImpostorVisionRange += .25f;
            ConfigurationChanged.Raise();
        }
    }

    public void VisionImpostorSubstract()
    {
        if (GameConfiguration.ImpostorVisionRange > .25)
        {
            GameConfiguration.ImpostorVisionRange -= .25f;
            ConfigurationChanged.Raise();
        }
    }
    
    
    public void VoteTimeAdd()
    {
        if (GameConfiguration.VoteTime < 200)
        {
            GameConfiguration.VoteTime += 10;
            ConfigurationChanged.Raise();
        }
    }

    public void VoteTimeSubstract()
    {
        if (GameConfiguration.VoteTime > 10)
        {
            GameConfiguration.VoteTime -= 10;
            ConfigurationChanged.Raise();
        }
    }
    
    
    public void KillCoolDownAdd()
    {
        if (GameConfiguration.KillCooldown < 60)
        {
            GameConfiguration.KillCooldown += 5;
            ConfigurationChanged.Raise();
        }
    }

    public void KillCoolDownSubstract()
    {
        if (GameConfiguration.KillCooldown > 5)
        {
            GameConfiguration.KillCooldown -= 5;
            ConfigurationChanged.Raise();
        }
    }
    
    
    
    public void TaskLongAdd()
    {
        if (GameConfiguration.LongTask < 5)
        {
            GameConfiguration.LongTask += 1;
            ConfigurationChanged.Raise();
        }
    }

    public void TaskMidAdd()
    {
        if (GameConfiguration.MidTask < 5)
        {
            GameConfiguration.MidTask += 1;
            ConfigurationChanged.Raise();
        }
    }
    public void TaskShortAdd()
    {
        if (GameConfiguration.ShortTask < 5)
        {
            GameConfiguration.ShortTask += 1;
            ConfigurationChanged.Raise();
        }
    }
    
    public void TaskLongSubstract()
    {
        if (GameConfiguration.LongTask > 0)
        {
            GameConfiguration.LongTask -= 1;
            ConfigurationChanged.Raise();
        }
    }

    public void TaskMidSubstract()
    {
        if (GameConfiguration.MidTask > 0)
        {
            GameConfiguration.MidTask -= 1;
            ConfigurationChanged.Raise();
        }
    }
    public void TaskShortSubstract()
    {
        if (GameConfiguration.ShortTask > 0)
        {
            GameConfiguration.ShortTask -= 1;
            ConfigurationChanged.Raise();
        }
    }
    
    
    public void EmergencyCountAdd()
    {
        if (GameConfiguration.EmergencyCount < 2)
        {
            GameConfiguration.EmergencyCount += 1;
            ConfigurationChanged.Raise();
        }
    }
    public void EmergencyCountSubstract()
    {
        if (GameConfiguration.EmergencyCount > 0)
        {
            GameConfiguration.EmergencyCount -= 1;
            ConfigurationChanged.Raise();
        }
    }
    public void EmergencyCooldownAdd()
    {
        if (GameConfiguration.EmergencyCooldown < 30)
        {
            GameConfiguration.EmergencyCooldown += 5;
            ConfigurationChanged.Raise();
        }
    }
    public void EmergencyCooldownSubstract()
    {
        if (GameConfiguration.EmergencyCooldown > 0)
        {
            GameConfiguration.EmergencyCooldown -= 5;
            ConfigurationChanged.Raise();
        }
    }
}
