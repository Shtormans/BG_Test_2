namespace MainGame
{
    public class Health
    {
        private int _maxHealth;
        private int _currentHealth;

        public int CurrentHealth => _currentHealth;

        public Health(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }
        
        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }
        }

        public bool IsAlive()
        {
            return _currentHealth > 0;
        }
    }
}

