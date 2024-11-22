using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class PlayerControll : MonoBehaviour
{
    private CharacterController characterController;
    private DefaultInput defaultInput;
    [HideInInspector]
    public Vector2 input_Movement;
    [HideInInspector]
    public Vector2 input_View;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("References")]
    public Transform CameraHolder;
    public Transform feetTransform;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;
    public LayerMask playerMask;
    public LayerMask groundMask;

    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;

    [Header("Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;
    public CharacterStance playerStandStance;
    public CharacterStance playerCrouchStance;
    public float stanceCheckErrorMargin = 0.05f;
    private float cameraHeight;
    private float cameraHeightVelocity;
    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;

    [HideInInspector]
    public bool isSprinting;

    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;

    [Header("Weapon")]
    public WeaponController currentWeapon;
    public float weaponAnimationSpeed;

    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isFalling;

    [Header("Leaning")]
    public Transform leanPivot;
    private float currentLean;
    private float targetLean;
    public float leanAngle;
    public float leanSmoothing;
    private float leanVelocity;

    private bool isLeaningLeft;
    private bool isLeaningRight;

    [Header("Aiming In")]
    public bool isAimingIn;

    public GameObject redDot;

    #region - Awake -
    void Awake()
    {
        defaultInput = new DefaultInput();

        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();

        defaultInput.Character.Crouch.performed += e => SetCrouch(true);
        defaultInput.Character.Crouch.canceled += e => SetCrouch(false);

        defaultInput.Character.Sprint.performed += e => ToggleSprint();
        defaultInput.Character.SprintReleased.performed += e => StopSprint();

        defaultInput.Character.LeanLeftPressed.performed += e => isLeaningLeft = true;
        defaultInput.Character.LeanLeftReleased.performed += e => isLeaningLeft = false;

        defaultInput.Character.LeanRightPressed.performed += e => isLeaningRight = true;
        defaultInput.Character.LeanRightReleased.performed += e => isLeaningRight = false;

        defaultInput.Weapon.Fire2Pressed.performed += e => AimingInPressed();
        defaultInput.Weapon.Fire2Released.performed += e => AimingInReleased();

        defaultInput.Weapon.Fire1Pressed.performed += e => ShootingPressed();
        defaultInput.Weapon.Fire1Released.performed += e => ShootingReleased();

        defaultInput.Enable();

        newCameraRotation = CameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();

        cameraHeight = CameraHolder.localPosition.y;

        if (currentWeapon)
        {
            currentWeapon.Initialise(this);
        }
    }
    #endregion

     void Start()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;

            LoadPlayerSettings();
        }

    void OnApplicationQuit()
    {
    SavePlayerSettings();
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    }


    #region - Update -
    private void Update()
    {   
        AdjustSensitivity();

        SetIsGrounded();
        SetIsFalling();

        CalculateView();
        CalculateMovement();
        CalculateStance();
        CalculateLeaning();
        CalculateAimingIn();

    }
    #endregion

    private void SavePlayerSettings()
    {
        PlayerPrefs.SetFloat("ViewXSensitivity", playerSettings.ViewXSensitivity);
        PlayerPrefs.SetFloat("ViewYSensitivity", playerSettings.ViewYSensitivity);

        PlayerPrefs.SetInt("IsCrouching", playerStance == PlayerStance.Crouch ? 1 : 0);

        PlayerPrefs.SetInt("IsSprinting", isSprinting ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void LoadPlayerSettings()
{
    if (PlayerPrefs.HasKey("ViewXSensitivity"))
    {
        playerSettings.ViewXSensitivity = PlayerPrefs.GetFloat("ViewXSensitivity");
        playerSettings.ViewYSensitivity = PlayerPrefs.GetFloat("ViewYSensitivity");
    }

    if (PlayerPrefs.HasKey("IsCrouching"))
    {
        playerStance = PlayerPrefs.GetInt("IsCrouching") == 1 ? PlayerStance.Crouch : PlayerStance.Stand;
    }

    if (PlayerPrefs.HasKey("IsSprinting"))
    {
        isSprinting = PlayerPrefs.GetInt("IsSprinting") == 1;
    }
}


    #region - Shooting -
    private void ShootingPressed()
    {
        if (currentWeapon && isAimingIn && !isSprinting) 
        {
            currentWeapon.isShooting = true; 
        }
    }

    private void ShootingReleased()
    {
        if (currentWeapon)
        {
            currentWeapon.isShooting = false; 
        }
    }


    #endregion

    #region - Aiming In -
    private void AimingInPressed(){
        isAimingIn = true;
    }

    private void AimingInReleased(){
        isAimingIn = false;
    }

    private void CalculateAimingIn()
    {
        if (!currentWeapon) return;

        // Jika sedang sprinting dan mulai aiming, otomatis hentikan sprinting
        if (isSprinting && isAimingIn)
        {
            isSprinting = false;
        }

        currentWeapon.isAimingIn = isAimingIn;
    }

    private void AdjustSensitivity()
    {
        if (Input.GetKey(KeyCode.Equals))
        {
            playerSettings.ViewXSensitivity += 0.1f;
            playerSettings.ViewYSensitivity += 0.1f;
            Debug.Log("Horizontal Sensitivity: " + playerSettings.ViewXSensitivity);
            Debug.Log("Vertical Sensitivity: " + playerSettings.ViewYSensitivity);
        }
        else if (Input.GetKey(KeyCode.Minus)
        {
            if (playerSettings.ViewXSensitivity > 0.1f)
            {
                playerSettings.ViewXSensitivity -= 0.1f;
                playerSettings.ViewYSensitivity -= 0.1f;
                Debug.Log("Horizontal Sensitivity: " + playerSettings.ViewXSensitivity);
                Debug.Log("Vertical Sensitivity: " + playerSettings.ViewYSensitivity);
            }
        }
    }


    #endregion

    #region - IsFalling / isGrounded -
    private void SetIsGrounded()
    {
        isGrounded = Physics.CheckSphere(feetTransform.position, playerSettings.isGroundedRadius, groundMask);
    }

    private void SetIsFalling()
    {
        isFalling = (!isGrounded && characterController.velocity.magnitude >= playerSettings.isFallingSpeed);
    }
    #endregion

    #region - View / Movement -
    private void CalculateView()
    {
        newCharacterRotation.y += (isAimingIn ? playerSettings.ViewXSensitivity * playerSettings.AimingSpeedEffector : playerSettings.ViewXSensitivity) * (playerSettings.ViewXInverted ? -input_View.x : input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x -= (isAimingIn ? playerSettings.ViewYSensitivity * playerSettings.AimingSpeedEffector : playerSettings.ViewYSensitivity) * (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        CameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {
        if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
        }

        var verticalSpeed = isSprinting ? playerSettings.RunningForwardSpeed : playerSettings.WalkingForwardSpeed;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed;

        if (!isGrounded)
        {
            playerSettings.SpeedEffector = playerSettings.FallingSpeedEffector;
        }
        else if (playerStance == PlayerStance.Crouch)
        {
            playerSettings.SpeedEffector = playerSettings.CrouchSpeedEffector;
        }
        else if (isAimingIn)
        {
            playerSettings.SpeedEffector = playerSettings.AimingSpeedEffector;
        }
        else
        {
            playerSettings.SpeedEffector = 1;
        }

        weaponAnimationSpeed = characterController.velocity.magnitude / (playerSettings.WalkingForwardSpeed * playerSettings.SpeedEffector);
        weaponAnimationSpeed = Mathf.Min(weaponAnimationSpeed, 1);

        verticalSpeed *= playerSettings.SpeedEffector;
        horizontalSpeed *= playerSettings.SpeedEffector;

        var targetMovementSpeed = new Vector3(horizontalSpeed * input_Movement.x, 0, verticalSpeed * input_Movement.y);

        if (isGrounded)
        {
            newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, targetMovementSpeed, ref newMovementSpeedVelocity, playerSettings.MovementSmoothing);
        }
        else
        {
            newMovementSpeed = targetMovementSpeed;
        }

        var movementSpeed = transform.TransformDirection(newMovementSpeed) * Time.deltaTime;

        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (isGrounded && playerGravity < -0.1f)
        {
            playerGravity = -0.1f;
        }

        movementSpeed.y = playerGravity;

        characterController.Move(movementSpeed);
    }
    #endregion

    #region - Leaning -

    private void CalculateLeaning()
    {
        if (isLeaningLeft)
        {
            targetLean = leanAngle;
        } else if (isLeaningRight)
        {
            targetLean = -leanAngle;
        } else {
            targetLean = 0;
        }
        
        


        currentLean = Mathf.SmoothDamp(currentLean, targetLean, ref leanVelocity, leanSmoothing);


        leanPivot.localRotation = Quaternion.Euler(new Vector3(0, 0, currentLean));
    }

    #endregion

    #region - Stance -
    private void CalculateStance()
    {
        CharacterStance currentStance = playerStance == PlayerStance.Crouch ? playerCrouchStance : playerStandStance;

        cameraHeight = Mathf.SmoothDamp(CameraHolder.localPosition.y, currentStance.cameraHeight, ref cameraHeightVelocity, playerStanceSmoothing);
        CameraHolder.localPosition = new Vector3(CameraHolder.localPosition.x, cameraHeight, CameraHolder.localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.StanceCollider.height, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.StanceCollider.center, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    private void SetCrouch(bool isCrouching)
    {
        playerStance = isCrouching ? PlayerStance.Crouch : PlayerStance.Stand;
    }

    private bool StanceCheck(float stanceCheckheight)
    {
        var start = new Vector3(feetTransform.position.x, feetTransform.position.y + characterController.radius + stanceCheckErrorMargin, feetTransform.position.z);
        var end = new Vector3(feetTransform.position.x, feetTransform.position.y - characterController.radius - stanceCheckErrorMargin + stanceCheckheight, feetTransform.position.z);

        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }
    #endregion

    #region - Sprinting -
    private void ToggleSprint()
    {
        if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
            return;
        }

        isSprinting = !isSprinting;
    }

    private void StopSprint()
    {
        if (playerSettings.SprintingHold)
        {
            isSprinting = false;
        }
    }
    #endregion

    #region - Gizmos -
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetTransform.position, playerSettings.isGroundedRadius);
    }
    #endregion
}
