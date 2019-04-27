using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageTowerMenu : TowerPlatformMenu {

	public void Sell()
	{
		Tower t = towerPlatform.Tower;
		if(t != null)
		{
			t.Sell();
		}
		Destroy(gameObject);
	} 

	public void Repair()
	{
		Tower t = towerPlatform.Tower;
		if(t != null)
		{
			bool confirm = t.ToggleRepair();
			if(confirm)
				PlayConfirmSound();
			else
				PlayRejectSound();
		}
		Destroy(gameObject);
	} 

	protected override void Update()
	{
		base.Update();
		if(towerPlatform.Tower == null)
		{
			Destroy(gameObject);
		}
	}
}
