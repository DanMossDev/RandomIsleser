using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "EnemyModel", menuName = AssetMenuNames.Models + "EnemyModel")]
    public class EnemyModel : ScriptableObject
    {
        public int HP;
        public float MovementSpeed;
        public float InvincibleTime;
        public float AggroRange;
        public float WanderRadius;
    }
}
