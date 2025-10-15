using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponInstantiateSlot rightHandSlot;
    public WeaponInstantiateSlot leftHandSlot;

    protected override void Awake()
    {
        base.Awake();

        //플레이어 인벤토리에 접속할 수 있도록 플레이어매니저를 선언해주고 컴포넌트 인식.
        player = GetComponent<PlayerManager>();
        // Get Our Slots
        InitializeWeaponSlots();
    }

    private void InitializeWeaponSlots()
    {
        WeaponInstantiateSlot[] weaponSlots = GetComponentsInChildren<WeaponInstantiateSlot>();

        //각자 슬롯을 체크하고, 해당 파트에 따라 인스턴스를 할당해주기.
        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    public void LoadRightWeapon()
    {
        if (player.playerinventoryManager.currentRightHandWeapon != null)
        {
            rightHandSlot.LoadWeapon(player.playerinventoryManager.currentRightHandWeapon.weaponModel);
        }
    }

    public void LoadLeftWeapon()
    {
        if (player.playerinventoryManager.currentLeftHandWeapon != null)
        {
            leftHandSlot.LoadWeapon(player.playerinventoryManager.currentLeftHandWeapon.weaponModel);
        }
    }
}
