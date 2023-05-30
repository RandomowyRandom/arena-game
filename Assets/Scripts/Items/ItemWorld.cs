using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        
        [SerializeField]
        private bool _performAnimation = true;

        private Item _item;
        
        private Collider2D _collider2D;
        
        public Item Item => _item;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            
            _collider2D.enabled = false;
            EnableColliderAfterSeconds(0.5f).Forget();
            
            SetItem(_startingItem);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void Start()
        {
            if(_performAnimation)
                PerformAnimation();

            PerformJumps();
        }

        private void PerformAnimation()
        {
            var randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            var moveDuration = UnityEngine.Random.Range(0.2f, 0.4f);
            
            transform.DOMove(transform.position + (Vector3)randomDirection * .8f, moveDuration).SetEase(Ease.OutQuart);
            transform.DOScale(new Vector2(1, 1.3f), moveDuration * .5f).OnComplete(() =>
                transform.DOScale(Vector3.one, moveDuration * .5f));
        }

        private async void PerformJumps()
        {
            var spriteRendererTransform = _spriteRenderer.transform;

            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(2.5f, 3.5f)));
                
                if(spriteRendererTransform == null)
                    return;
                
                spriteRendererTransform.DOJump(spriteRendererTransform.position, 0.2f, 1, .8f); 
            }
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