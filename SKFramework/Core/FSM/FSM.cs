﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 有限状态机管理器
    /// </summary>
    public class FSM : MonoBehaviour
    {
        #region NonPublic Variables
        private static FSM instance;

        //状态机列表
        private List<StateMachine> machines;
        #endregion

        #region Public Properties
        public static FSM Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.FSM]").AddComponent<FSM>();
                    instance.machines = new List<StateMachine>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }
        #endregion

        #region NonPublic Methods
        private void Update()
        {
            for (int i = 0; i < machines.Count; i++)
            {
                //更新状态机
                machines[i].OnUpdate();
            }
        }
        private void OnDestroy()
        {
            instance = null;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public T Create<T>(string stateMachineName) where T : StateMachine, new()
        {
            Type type = typeof(T);
            stateMachineName = string.IsNullOrEmpty(stateMachineName) ? type.Name : stateMachineName;
            if (machines.Find(m => m.Name == stateMachineName) == null)
            {
                T machine = (T)Activator.CreateInstance(type);
                machine.Name = stateMachineName;
                machines.Add(machine);
                Log.Info("<color=cyan><b>[SKFramework.FSM.Info]</b></color> 成功创建状态机[{0}]", stateMachineName);
                return machine;
            }
            Log.Error("<color=red><b>[SKFramework.FSM.Error]</b></color> 已存在名称为[{0}]的状态机 创建失败", stateMachineName);
            return null;
        }
        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>销毁成功返回true 否则返回false</returns>
        public bool Destroy(string stateMachineName)
        {
            var targetMachine = machines.Find(m => m.Name == stateMachineName);
            if (targetMachine != null)
            {
                targetMachine.OnDestroy();
                machines.Remove(targetMachine);
                Log.Info("<color=cyan><b>[SKFramework.FSM.Info]</b></color> 成功销毁状态机[{0}]", stateMachineName);
                return true;
            }
            Log.Error("<color=red><b>[SKFramework.FSM.Error]</b></color> 不存在名称为[{0}]的状态机 销毁失败", stateMachineName);
            return false;
        }
        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachine">状态机</param>
        /// <returns>销毁成功返回true 否则返回false</returns>
        public bool Destroy<T>(T stateMachine) where T : StateMachine, new()
        {
            if (machines.Contains(stateMachine))
            {
                Log.Info("<color=cyan><b>[SKFramework.FSM.Info]</b></color> 成功销毁状态机[{0}]", stateMachine.Name);
                stateMachine.OnDestroy();
                machines.Remove(stateMachine);
                return true;
            }
            Log.Error(message: "<color=red><b>[SKFramework.FSM.Error]</b></color> 销毁状态机失败");
            return false;
        }
        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public T GetMachine<T>(string stateMachineName) where T : StateMachine
        {
            return (T)machines.Find(m => m.Name == stateMachineName);
        }
        #endregion
    }
}