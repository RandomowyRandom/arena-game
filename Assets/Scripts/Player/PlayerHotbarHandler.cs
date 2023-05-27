using System;
using System.Collections.Generic;
using Inventory.Interfaces;
using Items;
using Items.Abstraction;
using Items.ItemDataSystem;
using Items.RaritySystem;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Stats;
using Stats.Interfaces;
using UI;
using UI.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerHotbarHandler: SerializedMonoBehaviour, IUsableItemProvider, IStatsDataProvider
    {
        [OdinSerialize]
        private IInventory _hotbarInventory;
        
        [OdinSerialize]
        private IItemUseLock _itemUseLock;
        
        [SerializeField]
        private InstantiatedSlotsInventoryUI _hotbarUI;
        
        public event Action OnUsableItemChanged;
        
        private int _currentHotbarSlot = 0;
        
        private int LastHotbarSlot => _hotbarInventory.Capacity - 1;
        
        private bool _initialized;
        private readonly List<HotbarSlotUI> _hotbarSlots = new();
        
        public Item CurrentItem => _hotbarInventory.Items[_currentHotbarSlot];

        private void Start()
        {
            _hotbarInventory.OnInventoryChanged += OnUsableItemChanged;
            
            Initialize();
            // SetCurrentHotbarSlot(0);
        }
        
        private void OnDestroy()
        {
            _hotbarInventory.OnInventoryChanged -= OnUsableItemChanged;
        }

        public UsableItem GetUsableItem()
        {
            if(_hotbarInventory.Items[_currentHotbarSlot] == null)
                return null;
            
            if(_hotbarInventory.Items[_currentHotbarSlot].ItemData is UsableItem usableItem)
                return usableItem;
            
            return null;
        }

        public void ConsumeItem(UsableItem item)
        {
            if(CurrentItem == null)
                return;
            
            var currentItemInSlot = _hotbarInventory.GetItem(_currentHotbarSlot);
            
            if(CurrentItem.ItemData == item)
                _hotbarInventory.SetItem(_currentHotbarSlot, new Item(item, currentItemInSlot.Amount - 1));
        }

        private void SetCurrentHotbarSlot(int slotIndex)
        {
            if(slotIndex == _currentHotbarSlot)
                return;
            
            _currentHotbarSlot = slotIndex;
            _hotbarSlots[_currentHotbarSlot].SetSelected(true);
            
            for (var i = 0; i < _hotbarSlots.Count; i++)
            {
                if (i == _currentHotbarSlot)
                    continue;
                
                _hotbarSlots[i].SetSelected(false);
            }
            
            OnUsableItemChanged?.Invoke();
        }

        private void Initialize()
        {
            foreach (var slot in _hotbarUI.Slots)
            {
                var hotbarSlot = slot.GetComponent<HotbarSlotUI>();
                hotbarSlot.ItemUseLock = _itemUseLock;

                if (hotbarSlot == null)
                    continue;

                _hotbarSlots.Add(hotbarSlot);
            }
            
            _initialized = true;
        }

        private void Update()
        {
            if(_itemUseLock.IsLocked)
                return;

            HandleScroll();
            HandleNumberKeys();
        }

        private void HandleNumberKeys()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                SetCurrentHotbarSlot(0);
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
                SetCurrentHotbarSlot(1);
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
                SetCurrentHotbarSlot(2);
            else if (Keyboard.current.digit4Key.wasPressedThisFrame)
                SetCurrentHotbarSlot(3);
            else if (Keyboard.current.digit5Key.wasPressedThisFrame)
                SetCurrentHotbarSlot(4);
        }

        private void HandleScroll()
        {
            var input = Mouse.current.scroll.y.ReadValue();

            switch (input)
            {
                case < 0:
                    SetCurrentHotbarSlot(_currentHotbarSlot == LastHotbarSlot ? 0 : _currentHotbarSlot + 1);
                    break;
                case > 0:
                    SetCurrentHotbarSlot(_currentHotbarSlot == 0 ? LastHotbarSlot : _currentHotbarSlot - 1);
                    break;
            }
        }

        public StatsData GetStatsData(GearRarity gearRarity)
        {
            var usableItem = GetUsableItem();

            if (usableItem == null)
                return new StatsData();

            var itemInSlot = _hotbarInventory.GetItem(_currentHotbarSlot);

            if (!itemInSlot.IsRarityItem)
                return new StatsData();
            
            if (usableItem is IStatsDataProvider statsDataProvider)
                return statsDataProvider.GetStatsData(itemInSlot.GearRarity);

            return new StatsData();
        }
    }
}