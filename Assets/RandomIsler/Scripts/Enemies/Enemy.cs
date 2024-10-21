using System;
using System.Threading.Tasks;

namespace RandomIsleser
{
    public class Enemy : Spawnable
    {
        //[Space, Header("Enemy")]
        //[SerializeField] private EnemyModel _enemyModel;

        public event Action<Enemy> OnDeath;

        private void OnEnable()
        {
            Initialise();
            KillInAFewSeconds();
        }

        private void Initialise()
        {
            //Do stuff like reset animator values
        }

        private async void KillInAFewSeconds()
        {
            await Task.Delay(3000);

            Die();
        }

        private void Die()
        {
            OnDeath?.Invoke(this);
            Services.Instance.ObjectPoolController.Return(gameObject, ObjectPoolKey);
        }
    }
}
