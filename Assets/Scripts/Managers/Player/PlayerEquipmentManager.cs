using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponInstantiateSlot rightHandSlot;
    public WeaponInstantiateSlot leftHandSlot;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

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
            rightHandWeaponModel = Instantiate(player.playerinventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
        }
    }

    public void LoadLeftWeapon()
    {
        if (player.playerinventoryManager.currentLeftHandWeapon != null)
        {
            leftHandWeaponModel = Instantiate(player.playerinventoryManager.currentRightHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
        }
    }
}
