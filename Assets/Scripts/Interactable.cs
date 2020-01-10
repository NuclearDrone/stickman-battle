using System;
using UnityEngine;

public class Interactable : MonoBehaviour {
    [SerializeField] private string pickUpText;
    [SerializeField] private GameObject PickUpPanel;

    private GameObject Player;
    private void Start() {
        PickUpPanel.SetActive(false);
    }

    public Weapon weapon = null;
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.gameObject.tag == "Player") {
            PickUpPanel.SetActive(true);
            Player = collision.transform.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.transform.gameObject.tag == "Player") {
            PickUpPanel.SetActive(false);
            Player = null;
        }
    }

    private void Update() {
        if (Player != null && Player.GetComponent<PlayerController>().pickUp) {
            WeaponSlot weaponSlot = Player.GetComponentInChildren<WeaponSlot>();
            if (weapon != null && weaponSlot.weapons.Count <= weaponSlot.inventorySize) {
                weaponSlot.GetWeapon(weapon.name);
                //gameObject.SetActive(false);
            }
        }
    }
}
