using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class InventorySwitch: MonoBehaviour
    {
        [SerializeField]
        private List<RectTransform> _inventoryTransforms;
        
        private bool _isInventoryOpen = true;

        private void Update()
        {
            if (!Keyboard.current.tabKey.wasPressedThisFrame) 
                return;
            
            SwitchInventory();
        }

        private void SwitchInventory()
        {
            foreach (var rectTransform in _inventoryTransforms)
            {
                rectTransform.gameObject.SetActive(!_isInventoryOpen);
                
            }
            
            _isInventoryOpen = !_isInventoryOpen;
        }
    }
}