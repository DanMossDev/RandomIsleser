using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Spawnable _spawnable;

        public GameObject Spawn()
        {
            var GO = Services.Instance.ObjectPoolController.Get(_spawnable.ObjectPoolKey);
            GO.transform.position = transform.position;
            return GO;
        }
    }
}
