using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstantiateSlot : MonoBehaviour
{
    // 왼쪽 슬롯인가 오른쪽 슬롯인가?
    public GameObject currentWeaponModel;

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
<<<<<<< .merge_file_EC7z5w
        {
<<<<<<< .merge_file_OBVQ2k

=======
            Destroy(currentWeaponModel);
>>>>>>> .merge_file_FAPNYY
        }
=======
>>>>>>> .merge_file_xhYUVd
    }
}
