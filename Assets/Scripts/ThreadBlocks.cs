using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadBlocks : MonoBehaviour
{
    public static ThreadBlocks instance;
    List<Action> threadActions = new List<Action>();
    Thread thread;

    private void Awake()
    {
        instance = this;
        _init_();
    }
    
    void _init_()
    {
        thread = new Thread(SetMeshes);
    }

    public void StartThread()
    {
        if (thread == null)
        {
            _init_();
        }
        if (!thread.IsAlive)
        {
            thread = new Thread(SetMeshes);
            thread.IsBackground = true;
            thread.Start();
        }
    }

    public void AddToThread(Action _action)
    {
        threadActions.Add(_action);
        StartThread();
    }

    public void SetMeshes()
    {
        while (threadActions.Count > 0)
        {
            Action _func = threadActions[0];
            threadActions.RemoveAt(0);

            _func?.Invoke();
        }
    }
}
