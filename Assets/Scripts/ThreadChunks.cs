using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadChunks : MonoBehaviour
{
    public static ThreadChunks instance;
    public bool isStarted = false;
    Queue<Action> threadActions = new Queue<Action>();
    Thread thread;
    
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        JobRunning();
    }

    void _init_()
    {
        thread = new Thread(JobRunning);
    }

    public void StartThread()
    {
        if (!thread.IsAlive || thread == null)
        {
            thread = new Thread(JobRunning);
            thread.IsBackground = true;
            thread.Start();
        }
    }

    public void RequestBlock(Action _action)
    {
        ThreadStart threadStart = delegate
        {
            BlockThread(_action);
        };
        new Thread(threadStart).Start();
    }

    void BlockThread(Action _action)
    {
        lock (threadActions)
        {
            threadActions.Enqueue(_action);
        }
    }

    public void JobRunning()
    {
        if (threadActions.Count > 0)
        {
            for (int i = 0; i < threadActions.Count; i++)
            {
                Action a = threadActions.Dequeue();
                a?.Invoke();
            }
        }
    }
}
