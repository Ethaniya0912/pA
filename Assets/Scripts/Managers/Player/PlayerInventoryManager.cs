using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : InventoryManager
{
    PlayerManager player;

    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;

    protected override void Awake()
    {
        base.Awake();

        //�÷��̾� �κ��丮�� ������ �� �ֵ��� �÷��̾�Ŵ����� �������ְ� ������Ʈ �ν�.
        player = GetComponent<PlayerManager>();
        
    }
}
