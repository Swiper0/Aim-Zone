using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models
{
   #region - Player -

public enum PlayerStance{
   Stand,
   Crouch
}

[System.Serializable]

 public class PlayerSettingsModel
 {
    [Header("View Settings")]
    public float ViewXSensitivity;
    public float ViewYSensitivity;

    public float AimingSensitivityEffector;
    
    public bool ViewXInverted;
    public bool ViewYInverted;

   [Header("Movement Settings")]
   public bool SprintingHold;
   public float MovementSmoothing;

   [Header("Movement - Running")]
   public float RunningForwardSpeed;
   public float RunningStrafeSpeed;


   [Header("Movement - Walking")]
   public float WalkingForwardSpeed;
   public float WalkingStrafeSpeed;
   public float WalkingBackwardSpeed;

   [Header("Jumping")]
   public float JumpingHeight;
   public float JumpingFalloff;
   public float FallingSmoothing;

   [Header("Speed Effectors")]
   public float SpeedEffector = 1;
   public float CrouchSpeedEffector;
   public float FallingSpeedEffector;
   public float AimingSpeedEffector;

   [Header("Is Grounded / Falling")]
   public float isGroundedRadius;
   public float isFallingSpeed;

 }
   [System.Serializable]
   public class CharacterStance{
      public float cameraHeight;
      public CapsuleCollider StanceCollider;
   }

 #endregion

   #region - Weapons -

   public enum WeaponFireType{
      SemiAuto,
      Auto
   }

   [System.Serializable]
   public class WeaponSettingsModel
   {
      public float weaponDamage = 25f;

      [Header("Weapon Sway")]
      public float SwayAmount;
      public bool SwayYInverted;
      public bool SwayXInverted;
      public float SwaySmoothing;
      public float SwayResetSmoothing;
      public float SwayClampX;
      public float SwayClampY;

      [Header("Weapon Movement Sway")]
      public float MovementSwayX;
      public float MovementSwayY;
      public bool MovementSwayYInverted;
      public bool MovementSwayXInverted;
      public float MovementSwaySmoothing;
   }
   #endregion
}
