using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class Animations
    {
        public static int RollHash = Animator.StringToHash("OnRoll");
        public static int HammerAttackHash = Animator.StringToHash("OnHammerAttack");
        public static int IsAimingHash = Animator.StringToHash("IsAiming");
        public static int FishingRodCastHash = Animator.StringToHash("FishingRodCast");
        public static int WeaponIndexHash = Animator.StringToHash("WeaponIndex");
    }
}
