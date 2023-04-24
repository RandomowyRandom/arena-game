using System;
using Cysharp.Threading.Tasks;
using Player;
using TMPro;
using UnityEngine;

namespace Items
{
    public class ItemWorld: MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField]
        private TMP_Text _amountText;
        
        [SerializeField]
        private Item _startingItem;

        private Item _item;
        
        private Collider2D _collider2D;
        private Rigidbody2D _rigidbody2D;
        
        public Item Item => _item;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _collider2D.enabled = false;
            EnableColliderAfterSeconds(0.5f).Forget();
            
            SetItem(_startingItem);
        }

        private void Start()
        {
            var randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            _rigidbody2D.AddForce(randomDirection * 2f, ForceMode2D.Impulse);
        }

        public void SetItem(Item item)
        {
            _item = item;
            
            _spriteRenderer.sprite = item.ItemData.Icon;
            var amount = item.Amount > 1 ? $"x{item.Amount.ToString()}" : string.Empty;
            
            _amountText.SetText(amount);
        }
        
        public Item GetItem()
        {
            var item = _item;
            return item;
        }
        
        private async UniTask EnableColliderAfterSeconds(float seconds)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            GetComponent<Collider2D>().enabled = true;
        }
        
        public void OnPlayerInventoryEnter(GameObject collisionObject)
        {
            var playerInventory = collisionObject.GetComponent<PlayerInventory>();
            
            if (playerInventory == null)
                return;
            
            var item = GetItem();
            var rest = playerInventory.TryAddItem(item);
            
            if(rest == null)
                Destroy(gameObject);
            else
                SetItem(rest);
        }
    }
}