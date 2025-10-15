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

        //플레이어 인벤토리에 접속할 수 있도록 플레이어매니저를 선언해주고 컴포넌트 인식.
        player = GetComponent<PlayerManager>();
        
    }
}
