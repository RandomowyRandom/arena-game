using System;
using Items.Abstraction;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class LogToConsoleItemEffect: IItemEffect 
    {
        [SerializeField]
        private string _message;
        
        public void OnUse(IItemUser user)
        {
            Debug.Log(_message);
        }
    }
}