using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Destructable {

	[SerializeField] int cost;
	[ContextMenuItem("Default","AutoCostPerHP")]
	[SerializeField] int costPerHP;

	bool isRepairing = false;
	
	public int Cost {
		get {return cost;}
	}

	public void Sell()
	{
		GameController.Instance.Money += (int)(CurrentHitPoints * costPerHP * 0.75f);
		NotifyOnDestruction();
		UnRegisterUnit();
		Destroy(gameObject);
	}

	//the return value is for the popup menu to decide what sound to play
	public bool ToggleRepair()
	{
		if((CurrentHitPoints == HitPoints) || GameController.Instance.Money < costPerHP)
		{
			return false;
		}

		if(!isRepairing)
		{
			StartCoroutine(RepairCoroutine());
		}
		else
		{
			StopCoroutine("RepairCoroutine");
			isRepairing = false;
		}
		return true;
	}

	public void Upgrade()
	{
		Debug.Log(name + " : Upgrade() invoked");
	}

	void AutoCostPerHP()
	{
		costPerHP = (int)((float)(cost/hitPoints) + 0.5f);
		if(costPerHP < 1)
		{
			costPerHP = 1;
		}
	}

	IEnumerator RepairCoroutine()
	{
		lifeBar.EnableRepairIcon(true);
		isRepairing = true;
		Debug.Log("entering repair routine");
		Debug.Log((costPerHP.ToString() + "  " +  GameController.Instance.Money.ToString()));
		Debug.Log((costPerHP <= GameController.Instance.Money).ToString() + " " + (CurrentHitPoints < HitPoints).ToString());
		while((costPerHP <= GameController.Instance.Money) && (CurrentHitPoints < HitPoints) && isRepairing)
		{
			GameController.Instance.Money -= costPerHP;
			CurrentHitPoints++;
			yield return new WaitForSeconds(0.1f);
		}
		Debug.Log("exiting repair routine");
		lifeBar.EnableRepairIcon(false);
		isRepairing = false;
		yield return null;
	}

	
}
