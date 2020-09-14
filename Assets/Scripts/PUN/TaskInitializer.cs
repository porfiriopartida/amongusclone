using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Utils;
using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using UnityEngine;

public class TaskInitializer : MonoBehaviour
{
    public GameObject[] LongTasks;
    public GameObject[] MidTasks;
    public GameObject[] ShortTasks;
    private int _longTaskCount;
    private int _midTaskCount;
    private int _shortTaskCount;
    void Start()
    {
        LoadCustomizations();
        RandomizeTasks();
        // DisableController();
    }

    private void RandomizeTasks()
    {
        
        GameObject[] AssignedLongTasks = AmongosRandomGenerator.RandomizeTasks(LongTasks, _longTaskCount);
        GameObject[] AssignedMidTasks = AmongosRandomGenerator.RandomizeTasks(MidTasks, _midTaskCount);
        GameObject[] AssignedShortTasks = AmongosRandomGenerator.RandomizeTasks(ShortTasks, _shortTaskCount);

        BulkActive(AssignedLongTasks);
        BulkActive(AssignedMidTasks);
        BulkActive(AssignedShortTasks);
    }
    
    private void BulkActive(GameObject[] Tasks)
    {
        //TODO: Enable them and enable them for map too. Event? Scriptable Object instead?
        foreach (var obj in Tasks)
        {
            obj.SetActive(true);
        }
    }


    private void LoadCustomizations()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        _longTaskCount = (int) PhotonNetwork.CurrentRoom.CustomProperties["LongTask"];
        _midTaskCount = (int) PhotonNetwork.CurrentRoom.CustomProperties["MidTask"];
        _shortTaskCount = (int) PhotonNetwork.CurrentRoom.CustomProperties["ShortTask"];

        if (_longTaskCount > LongTasks.Length)
        {
            //TODO: Throw error or award instead?
            _longTaskCount = LongTasks.Length;
        }

        if (_midTaskCount > MidTasks.Length)
        {
            _midTaskCount = MidTasks.Length;
        }

        if (_shortTaskCount > ShortTasks.Length)
        {
            _shortTaskCount = ShortTasks.Length;
        }
    }

}
