using Fusion;

namespace MainGame
{
    public struct Health : INetworkStruct
    {
        private int _maxHealth;
        private int _currentHealth;

        public int CurrentHealth => _currentHealth;

        public Health(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }
        
        public void TakeDamage(uint damage)
        {
            _currentHealth -= (int)damage;

            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }
        }

        public void AddHealth(uint health)
        {
            _currentHealth += (int)health;
        }

        public bool IsAlive()
        {
            return _currentHealth > 0;
        }
    }
}

