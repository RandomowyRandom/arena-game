﻿using System;
using System.Collections.Generic;
using Common.Extensions;
using Items;
using Items.ItemDataSystem;
using Player.Interfaces;
using UnityEngine;

namespace ItemSelection
{
    public class ItemPedestalsHandler: MonoBehaviour
    {
        [SerializeField]
        private List<ItemPedestal> _pedestals;
        
        [SerializeField]
        private ItemDatabase _itemDatabase;

        private IPlayerLevel _playerLevel;
        
        private List<ItemData> _itemPool;
        
        private void Start()
        {
            _playerLevel = ServiceLocator.ServiceLocator.Instance.Get<IPlayerLevel>();

            _itemPool = _itemDatabase.GetItemDataByQuery(
                item => item is Weapon or Equipment
                        && item.RequiredLevel <= _playerLevel.CurrentLevel);

            SpawnItems();
        }

        private void SpawnItems()
        {
            foreach (var pedestal in _pedestals)
            {
                pedestal.GameObject.SetActive(true);

                var randomItem = _itemPool.GetRandomElement();

                pedestal.OnItemTaken += Destroy;
                pedestal.SetItem(new Item(randomItem, 1));

                _itemPool.Remove(randomItem);
            }
        }

        private void Destroy()
        {
            foreach (var pedestal in _pedestals)
                pedestal.OnItemTaken -= Destroy;
            
            Destroy(gameObject);
        }
    }
}