using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] protected Spawnable _spawnable;

        protected Spawnable _spawnedObject;

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
            
            Services.Instance.ObjectPoolController.Return(_spawnedObject.gameObject, _spawnable.ObjectPoolKey);
        }
    }
}
