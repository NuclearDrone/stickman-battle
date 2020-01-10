using UnityEngine;

public class WeaponObject : MonoBehaviour {
    public Weapon weapon;
    public SpriteRenderer spriteRenderer;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public bool activeWeapon = false;
    private float currentCooldown = 0f;
    public AudioClip attackSound;
    public AudioSource effectSource;
    public GameObject player;
    public GameObject muzzleFlash;
    private void Awake() {
        effectSource = gameObject.GetComponent<AudioSource>();
        player = gameObject.GetComponentInParent<PlayerController>().gameObject;
        if (weapon.sprite != null) {
            spriteRenderer.sprite = weapon.sprite;
        }
        spriteRenderer.enabled = false;
    }
    public void SelectThisWeapon(bool active) {

        spriteRenderer.enabled = active;
        activeWeapon = active;
    }

    public void UseWeapon(bool facingRight) {
        if(activeWeapon) {
            switch (weapon.name) {
                case "Desert Eagle":
                    Shoot(facingRight);
                    break;
                default:
                    Debug.Log(weapon.name + " has no useWeapon case.");
                    break;
            }
        } else {
            Debug.Log("Weapon is not active weapon: this shouldn't be happening");
        }
    }
    private void FixedUpdate() {
        if (currentCooldown > 0) {
            currentCooldown -= Time.fixedDeltaTime;
        }
    }
    private void Shoot(bool f) {
        if (currentCooldown <= 0) {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(new Vector3(0f, 0f, f ? 0f : 180f)));
            bullet.GetComponent<Bullet>().damage = weapon.damage;
            bullet.GetComponent<Bullet>().shooter = player.name;
            Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
            currentCooldown = weapon.attackCooldown;
            effectSource.clip = attackSound;
            effectSource.Play();
        }
    }
}
