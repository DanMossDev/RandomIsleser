using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    [CreateAssetMenu(fileName = "EnemyModel", menuName = AssetMenuNames.Models + "EnemyModel")]
    public class EnemyModel : ScriptableObject
    {
        public int HP = 1;
        public float WanderSpeed = 1;
        public float ChaseSpeed = 2;
        public float TurnSpeed = 360;
        public float InvincibleTime = 0.5f;
        public float AggroRange = 10;
        public float WanderRadius = 20;
    }
}
