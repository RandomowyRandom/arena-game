using UnityEngine.UI;

namespace BuffSystem
{
    public class Buff
    {
        private BuffData _buffData;

        private float _remainingTime;
        
        public Buff(BuffData buffData)
        {
            _buffData = buffData;
            _remainingTime = buffData.Duration;
        }
        
        public BuffData BuffData => _buffData;
        
        public float RemainingTime
        {
            get => _remainingTime;
            set => _remainingTime = value;
        }
    }
}