using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstantiateSlot : MonoBehaviour
{
    public WeaponModelSlot weaponSlot;
    // 왼쪽 슬롯인가 오른쪽 슬롯인가?
    public GameObject currentWeaponModel;

    public void UnloadWeapon()
    {
        //커런트 웨폰모델이 존재할 시, 파괴.
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeapon(GameObject weaponModel)
    {
        // 웨폰을 불러올때, 페런트의 트랜스폼을 트랜스폼으로 지정해쥬기.
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        //로컬포지션들 설정.
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }


}
