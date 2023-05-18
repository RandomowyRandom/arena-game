using System;
using InteractionSystem;
using InteractionSystem.Abstraction;
using Inventory.Interfaces;
using Items;
using Mono.CSharp;
using Player.Interfaces;
using TMPro;
using UnityEngine;

namespace ItemSelection
{
    public class ItemPedestal: MonoBehaviour, IInteractable
    {
        [SerializeField]
        private SpriteRenderer _itemRenderer;
        
        [SerializeField]
        private TMP_Text _tooltipText;

        [SerializeField]
        private OutlineInteractionEffect _outlineInteractionEffect;
        
        public event Action OnItemTaken; 
        public GameObject GameObject => gameObject;

        private Item _item;
        
        private IInventory _playerInventory;

        private void Start()
        {
            _playerInventory = ServiceLocator.ServiceLocator.Instance.Get<IPlayerInventory>().Inventory;
            
            _tooltipText.gameObject.SetActive(false);
        }

        public void SetItem(Item item)
        {
            _item = item;
            _itemRenderer.sprite = item.ItemData.Icon;
            _tooltipText.SetText(item.GetTooltip());
        }

        public void Interact()
        {
            _playerInventory.TryAddItem(_item);
            OnItemTaken?.Invoke();
        }

        public void OnHandlerEnter(IInteractionHandler handler)
        {
            _tooltipText.gameObject.SetActive(true);
            _outlineInteractionEffect?.Show();
        }

        public void OnHandlerExit(IInteractionHandler handler)
        {
            if(this == null)
                return;
            
            _tooltipText.gameObject.SetActive(false);
            _outlineInteractionEffect?.Hide();
        }
    }
}