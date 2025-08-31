using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ItemData
{
    // 애니메이터 컨트롤러 

    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Acquirements")]
    /*public int strongthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0; 당장 필요하지 않음.*/

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    /*public int magicalDamage = 0;
    public int fireDamge = 0;
    public int holyDamge = 0;*/


    [Header("Weapon Poise")]
    public float poiseDamage = 10;
    // Weapon Modifier
    // Light Attack Modifier
    // Heavy Attack Modifier
    // Critical Damage Modifier

    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;
    // RUNNING ATTACK STAMINA COST MODIFIER
    // LIGHT ATTACK STAMINA COST MODIFIER
    // HEAVY ATTCK STAMINA COST MODIFIER ECT

}
