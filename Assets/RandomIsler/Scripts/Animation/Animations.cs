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
        public static int FishingRodReturnHash = Animator.StringToHash("FishingRodReturn");
        public static int WeaponIndexHash = Animator.StringToHash("WeaponIndex");
        public static int WeaponEquippedHash = Animator.StringToHash("WeaponEquipped");
        public static int CycloneJarChargingHash = Animator.StringToHash("CycloneJarCharging");
        public static int CycloneJarJumpChargeHash = Animator.StringToHash("CycloneJarJumpCharge");
        public static int CycloneJarJumpHash = Animator.StringToHash("CycloneJarJump");
    }
}
