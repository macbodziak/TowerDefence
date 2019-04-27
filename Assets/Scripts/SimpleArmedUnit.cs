using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleArmedUnit : ArmedUnit {

	[SerializeField] int damage;
	[SerializeField] int AP;

	override protected void FireWeapon()
	{
		// Debug.Log(this.name + "has fired its weapon at" + target.name + " dealing " + damage + " damage");
		PlayWeaponEffect();
		target.TakeDamage(damage, AP);
	}
}
