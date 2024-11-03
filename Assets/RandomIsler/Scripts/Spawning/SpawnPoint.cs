using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] protected Spawnable _spawnable;
        [SerializeField] private bool _spawnOnEnable;

        protected Spawnable _spawnedObject;

        private void Start()
        {
            if (_spawnOnEnable)
                Spawn();
        }

        public virtual GameObject Spawn()
        {
            var GO = Services.Instance.ObjectPoolController.Get(_spawnable.ObjectPoolKey);
            GO.transform.position = transform.position;
            _spawnedObject = GO.GetComponent<Spawnable>();
            _spawnedObject.OnSpawned(this);
            return GO;
        }

        public virtual void Despawn()
        {
            if (_spawnedObject == null || !_spawnedObject.gameObject.activeSelf) return;
            _spawnedObject.OnDespawned();
            Services.Instance.ObjectPoolController.Return(_spawnedObject.gameObject, _spawnable.ObjectPoolKey);
        }
    }
}
