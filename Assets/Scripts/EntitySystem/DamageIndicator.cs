using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(TMP_Text))]
    public class DamageIndicator: MonoBehaviour
    {
        private TMP_Text _text;
        
        private async void Awake()
        {
            _text = GetComponent<TMP_Text>();
            
            var randomDirection = Random.insideUnitCircle.normalized;
            transform.DOMove(transform.position + new Vector3(randomDirection.x * .25f, randomDirection.y * .25f, 0), .2f);
            
            await UniTask.Delay(1000);
            
            transform.DOScale(Vector3.zero, .2f).OnComplete(
                () => Destroy(gameObject));
        }
        
        public void SetDamage(float damage)
        {
            var rounded = Mathf.RoundToInt(damage);
            
            _text.SetText(rounded.ToString());
            _text.color = damage > 0 ? Color.white : Color.red;
        }
    }
}