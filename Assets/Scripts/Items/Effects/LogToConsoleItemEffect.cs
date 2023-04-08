using System;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class LogToConsoleItemEffect: IItemEffect 
    {
        [SerializeField]
        private string _message;
        
        public void OnUse(IItemUser user, UsableItem item)
        {
            Debug.Log(_message);
        }
    }
}