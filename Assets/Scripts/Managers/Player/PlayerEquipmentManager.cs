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

        //�÷��̾� �κ��丮�� ������ �� �ֵ��� �÷��̾�Ŵ����� �������ְ� ������Ʈ �ν�.
        player = GetComponent<PlayerManager>();
        // Get Our Slots
        InitializeWeaponSlots();
    }

    private void InitializeWeaponSlots()
    {
        WeaponInstantiateSlot[] weaponSlots = GetComponentsInChildren<WeaponInstantiateSlot>();

        //���� ������ üũ�ϰ�, �ش� ��Ʈ�� ���� �ν��Ͻ��� �Ҵ����ֱ�.
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
