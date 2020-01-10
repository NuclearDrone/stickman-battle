using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class PlayerController : MonoBehaviour {


    public float movementSpeed = 40f;
    public bool testMovement = false;
    public bool pickUp = false;
    public float aimRange;
    public float aimRotationOffset = 0f;
    public float health = 100;
    public WeaponSlot weaponSlot;
    public Transform aimPoint;
    public Transform aimTarget;
    public Transform rightHandSolver;
    public Transform leftHandSolver;
    public Transform head;
    public TMPro.TextMeshProUGUI text;

    private Dictionary<string, float[]> aimOffsets = new Dictionary<string, float[]>() {
        { "", new[]{37.2f, 17.2f} },
        { "Desert Eagle", new[]{ 22.7f, 2.7f } }
    };
        
        

    private Camera cam;
    private CharacterController2D controller;
    private Animator animator;
    private LineRenderer line;

    private float horizontal = 0f;
    private bool jump = false;
    private bool crouch = false;
    private bool aiming = false;
    private string activeWeaponName = "";
    private bool usingWeapon = false;
    public void Awake() {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }
    
    private void Update() {
        if(health <= 0) {
            Destroy(gameObject);
        } else {
            text.text = health.ToString();
        }
        if(aiming) {
            animator.SetBool("Aiming", true);
        } else {
            animator.SetBool("Aiming", false);
        }
        if (Mathf.Abs(horizontal) > 0.01) {
            animator.SetBool("Running", true);
        }
        else {
            animator.SetBool("Running", false);
        }
        activeWeaponName = weaponSlot.activeWeaponName;
        switch (activeWeaponName) {
            case "Desert Eagle":
                animator.SetBool("Desert Eagle", true);
                break;
            default:
                animator.SetBool("Desert Eagle", false);
                break;
        }
    }
    public void PickUp(InputAction.CallbackContext context) {
        pickUp = context.ReadValue<float>() == 1;
    }
    public void Jump(InputAction.CallbackContext context) {
        jump = context.ReadValue<float>() == 1;
    }
    public void Crouch() {
        crouch = !crouch;
    }
    public void OnMovement(InputAction.CallbackContext context) {
        horizontal = context.ReadValue<float>();
    }
    public void UseWeapon(InputAction.CallbackContext context) {
        if(context.ReadValue<float>() > 0.1f && aiming) {
            usingWeapon = true;
        } else {
            usingWeapon = false;
        }
    }

    public void StartAim(InputAction.CallbackContext context) {
        //Solution for the time being
        if(context.ReadValue<float>() > 0.1f) {
            aiming = true;
        } else {
            aiming = false;
        }

    }
    public void Aim(InputAction.CallbackContext context) {
        //Debug.Log(context.ReadValue<Vector2>()); // (x, y) screen point on Position, -1 to 1 on Right Stick 
        //Debug.Log(context.control.displayName); Position or Right Stick

        Vector2 direction = Vector2.zero;

        if(context.control.displayName == "Position") {
            //Debug.Log(context.control.displayName);
            Vector2 screenpoint = context.ReadValue<Vector2>();
            Vector3 cursorPos = cam.ScreenToWorldPoint(new Vector3(screenpoint.x, screenpoint.y));
            direction = (new Vector2(cursorPos.x, cursorPos.y) - new Vector2(aimPoint.position.x, aimPoint.position.y)).normalized;

        } else if(context.control.displayName == "Right Stick"){
            direction = context.ReadValue<Vector2>(); // (-1, -1) to (1, 1)
        }

        aimTarget.transform.position = new Vector3(aimPoint.position.x + direction.x * aimRange, aimPoint.position.y + direction.y * aimRange);

        if (aiming && animator.GetCurrentAnimatorStateInfo(1).IsTag("empty")) {
            animator.enabled = false;
            float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
            if (gameObject.transform.localScale.x < 0) {
                leftHandSolver.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180 + angle + aimOffsets[activeWeaponName][0] + aimRotationOffset));
                rightHandSolver.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180 + angle + aimOffsets[activeWeaponName][1] + aimRotationOffset));
            } else {
                leftHandSolver.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - aimOffsets[activeWeaponName][0] - aimRotationOffset));
                rightHandSolver.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - aimOffsets[activeWeaponName][1] - aimRotationOffset));
            }
        } else {
            animator.enabled = true;
        }
    }

    private void FixedUpdate() {
        controller.Move(horizontal * movementSpeed * Time.fixedDeltaTime, crouch, jump, aiming);
        if(usingWeapon) {
            weaponSlot.UseWeapon(transform.localScale.x > 0);
        }
    }
}