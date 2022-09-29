using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

[CreateAssetMenuAttribute(fileName = "New Mage Data", menuName = "Character Data/Mage")]
public class MageData : CharacterData {
	public MageDamageType damageType;
	public MageWeaponType weaponType;
}
