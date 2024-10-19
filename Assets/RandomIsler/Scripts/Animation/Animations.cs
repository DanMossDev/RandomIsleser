using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class Animations
    {
        //Humanoid
        public static int RollHash = Animator.StringToHash("OnRoll");
        public static int HammerAttackHash = Animator.StringToHash("OnHammerAttack");
        public static int IsAimingHash = Animator.StringToHash("IsAiming");
        public static int FishingRodCastHash = Animator.StringToHash("FishingRodCast");
        public static int FishingRodReturnHash = Animator.StringToHash("FishingRodReturn");
        public static int WeaponIndexHash = Animator.StringToHash("WeaponIndex");
        public static int WeaponEquippedHash = Animator.StringToHash("WeaponEquipped");
        public static int CycloneJarChargingHash = Animator.StringToHash("CycloneJarCharging");
        public static int CycloneJarJumpChargeHash = Animator.StringToHash("CycloneJarJumpCharge");
        public static int MovementSpeedHash = Animator.StringToHash("MovementSpeed");
        public static int HorizontalMovementSpeedHash = Animator.StringToHash("HorizontalMovementSpeed");
        public static int OnLadderHash = Animator.StringToHash("OnLadder");
        public static int AscendLadderHash = Animator.StringToHash("AscendLadder");
        public static int DescendLadderHash = Animator.StringToHash("DescendLadder");
        public static int ExitLadderHash = Animator.StringToHash("ExitLadder");
        
        //Environment
        public static int OpenDoorHash = Animator.StringToHash("OpenDoor");
    }
}
