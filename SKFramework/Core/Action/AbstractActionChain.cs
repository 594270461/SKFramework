﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 抽象事件链
    /// </summary>
    public abstract class AbstractActionChain : AbstractAction, IActionChain
    {
        protected MonoBehaviour executer;
        protected List<IAction> cacheList;
        protected List<IAction> invokeList;
        protected Func<bool> stopWhen;
        public bool IsPaused { get; protected set; }
        protected int loops = 1;

        public AbstractActionChain()
        {
            executer = ActionMaster.Instance;
            cacheList = new List<IAction>();
            invokeList = new List<IAction>();
        }
        public AbstractActionChain(MonoBehaviour executer)
        {
            this.executer = executer;
            cacheList = new List<IAction>();
            invokeList = new List<IAction>();
        }
        public IActionChain Append(IAction action)
        {
            cacheList.Add(action);
            invokeList.Add(action);
            return this;
        }
        public IActionChain StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }
        public IActionChain OnStop(Action action)
        {
            onCompleted = action;
            return this;
        }
        public IActionChain Begin()
        {
            ActionChain.Execute(this, executer);
            return this;
        }
        public void Pause()
        {
            IsPaused = true;
        }
        public void Resume()
        {
            IsPaused = false;
        }
        public void Stop()
        {
            isCompleted = true;
        }
        public IActionChain SetLoops(int loops)
        {
            this.loops = loops;
            return this;
        }
    }
}