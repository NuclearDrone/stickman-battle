using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSlot : MonoBehaviour {

    WeaponObject[] allWeapons;


    public int inventorySize = 2;
    public List<WeaponObject> weapons = new List<WeaponObject>();
    public int selectedWeaponIndex = -1;
    public string activeWeaponName = "";
    private bool preventDouble = true;
    private void Awake() {
        allWeapons = GetComponentsInChildren<WeaponObject>();
    }
    
    private string GetWeaponName() {
        return weapons[selectedWeaponIndex].weapon.name;
    }

    public void GetWeapon(string name) {
        foreach(WeaponObject wea in allWeapons) {
            wea.GetComponent<WeaponObject>().SelectThisWeapon(false);
        }
        try {
            WeaponObject temp = transform.Find(name).GetComponent<WeaponObject>();
            weapons.Add(temp);
            SelectWeapon();
        } catch {

        }
    }
    public void UseWeapon(bool facingRight) {
        weapons[selectedWeaponIndex].UseWeapon(facingRight);
    }
    public void SelectWeapon() {
        preventDouble = !preventDouble;
        if (preventDouble) {
            return;
        }
        try {
            selectedWeaponIndex += 1;
            if (selectedWeaponIndex >= weapons.Count) {
                selectedWeaponIndex = 0;
            }
            if (selectedWeaponIndex < 0) {
                selectedWeaponIndex = weapons.Count - 1;
            }
            foreach (WeaponObject wep in allWeapons) {
                wep.GetComponent<WeaponObject>().SelectThisWeapon(false);
            }
            weapons[selectedWeaponIndex].SelectThisWeapon(true);
            Debug.Log(GetWeaponName());
            activeWeaponName = GetWeaponName();
        }
        catch {

        }
    }


}
