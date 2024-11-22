using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;
using System.Linq;

public class WeaponController : MonoBehaviour
{
    private PlayerControll characterControl;

    [Header("References")]
    public Animator weaponAnimator;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public LineRenderer lineRenderer;

    [Header("Settings")] 
    public WeaponSettingsModel settings;

    bool isInitialised;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    private bool isGroundedTrigger;
    private bool isFallingTrigger;

    [Header("Weapon Sway")]
    public Transform weaponSwayObject;

    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 100;
    public float swayLerpSpeed = 14;

    public float swayTime;
    Vector3 swayPosition;

    [Header("Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;
    [HideInInspector]
    public bool isAimingIn;

    [Header("Shooting")]
    public float rateOffFire;
    private float currentFireRate;
    public List<WeaponFireType> allowedFireTypes;
    public WeaponFireType currentFireType;
    [HideInInspector]
    public bool isShooting;
    private float fireRate = 0.1f;
    private float nextFireTime = 0f;

    public AudioSource gunAudioSource;
    public AudioClip fireSound;

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
        currentFireType = allowedFireTypes.First();
        lineRenderer.enabled = false;

         if (gunAudioSource == null)
    {
        gunAudioSource = GetComponent<AudioSource>();
    }
        
    }

    public void Initialise(PlayerControll characterController)
    {
        characterControl = characterController;
        isInitialised = true;
    }

    private void Update()
{
    if(!isInitialised)
    {
        return;
    }
    
    // Periksa jika aiming, maka recoil aktif, sway dinonaktifkan
    if (isAimingIn)
    {
        swayPosition = Vector3.zero;  // Nonaktifkan sway saat aiming
    }
    else
    {

        CalculateWeaponSway();  // Aktifkan sway saat tidak aiming
    }


    CalculateWeaponRotation();
    SetWeaponAnimation();
    CalculateAimingIn();
    CalculateShooting();

    if(characterControl.isGrounded && !isGroundedTrigger)
    {
        isGroundedTrigger = true;
    } 
    else if(!characterControl.isGrounded && isGroundedTrigger) 
    {
        isGroundedTrigger = false;
    }
}


    private void CalculateShooting()
    {
        if (isShooting && characterControl.isAimingIn && !characterControl.isSprinting)
        {
            if (currentFireType == WeaponFireType.Auto)
            {
                Shoot();
            }
            else if (currentFireType == WeaponFireType.SemiAuto)
            {
                Shoot();
                isShooting = false;
            }
        }
    }

private void Shoot()
{
    if (Time.time >= nextFireTime)
    {
        RaycastHit hit;
        Vector3 direction = characterControl.CameraHolder.transform.forward;

        if (fireSound != null && gunAudioSource != null)
        {
            gunAudioSource.PlayOneShot(fireSound);
        }

        // Raycast untuk mendeteksi apakah mengenai objek
        if (Physics.Raycast(bulletSpawn.position, direction, out hit))
        {
            RenderShotLine(bulletSpawn.position, hit.point);

            TargetSpawner spawner = FindObjectOfType<TargetSpawner>();


            // Tentukan tindakan berdasarkan tag collider
            switch (hit.collider.tag)
            {
                case "Enemy":
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(settings.weaponDamage);
                    }
                    break;

                case "Easy":
                    if (spawner != null) spawner.SetMode("Easy");
                    break;

                case "Normal":
                    if (spawner != null) spawner.SetMode("Normal");
                    break;

                case "Hard":
                    if (spawner != null) spawner.SetMode("Hard");
                    break;

                case "Start":
                    if (spawner != null) spawner.StartSpawning();
                    break;

                case "Stop":
                    if (spawner != null) spawner.StopSpawning();
                    break;

                default:
                    break;
            }
        }
        else
        {
            RenderShotLine(bulletSpawn.position, bulletSpawn.position + direction * 100f);
        }

        nextFireTime = Time.time + fireRate;
    }
}


private void RenderShotLine(Vector3 start, Vector3 end)
{
    lineRenderer.enabled = true;

    lineRenderer.SetPosition(0, start);
    lineRenderer.SetPosition(1, end);


    StartCoroutine(DisableShotLine());
}

private IEnumerator DisableShotLine()
{
    yield return new WaitForSeconds(0.05f);
    lineRenderer.enabled = false;
}

    private void CalculateAimingIn()
    {
        var targetPosition = transform.position;

        if(isAimingIn)
        {
            targetPosition = characterControl.CameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (characterControl.CameraHolder.transform.forward * sightOffset);
        }
        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingInTime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
    }

    private void CalculateWeaponRotation()
{
    weaponAnimator.speed = characterControl.weaponAnimationSpeed;

    if (isAimingIn)
    {
        targetWeaponRotation.x = 0;
        targetWeaponRotation.y = 0;
        targetWeaponRotation.z = 0;
    }
    else
    {
        // Menghitung sway jika tidak aiming
        targetWeaponRotation.y += (settings.SwayAmount / 3) * (settings.SwayXInverted ? -characterControl.input_View.x : characterControl.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += (settings.SwayAmount / 3) * (settings.SwayYInverted ? characterControl.input_View.y : -characterControl.input_View.y) * Time.deltaTime;
    }

    targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
    targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);
    targetWeaponRotation.z = isAimingIn ? 0 : targetWeaponRotation.y;

    targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
    newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothing);

    targetWeaponMovementRotation.z = (isAimingIn ? settings.MovementSwayX / 3 : settings.MovementSwayX) * (settings.MovementSwayXInverted ? -characterControl.input_Movement.x : characterControl.input_Movement.x);
    targetWeaponMovementRotation.x = (isAimingIn ? settings.MovementSwayY / 3 : settings.MovementSwayY) * (settings.MovementSwayYInverted ? -characterControl.input_Movement.y : characterControl.input_Movement.y);

    targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);
    newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);

    transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
}


    private void SetWeaponAnimation()
    {
        weaponAnimator.SetBool("IsSprinting", characterControl.isSprinting);
    }

    private void CalculateWeaponSway()
    {
        if (isAimingIn)
        {
            swayPosition = Vector3.zero;
        }
        else
        {
            var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / swayScale;
            swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        }

        swayTime += Time.deltaTime;

        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }
    }

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }
}
