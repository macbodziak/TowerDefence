using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

	[SerializeField] bool isEnemy;
	[SerializeField] protected int hitPoints;
	[SerializeField] protected int armour = 0;
	int currentHitPoints;
	[SerializeField] private GameObject explosionPrefab;
	[SerializeField] private GameObject damageEffectPrefab;
	protected LifeIndicator lifeBar;
	List<ArmedUnit> engagingEnemies; 

	public int HitPoints
	{
		get {return hitPoints;}
	}

	public int CurrentHitPoints
	{
		get
		{
			return currentHitPoints;
		}

		set
		{
			currentHitPoints = value;
			if(lifeBar != null)
			{
				lifeBar.SetValue((float)currentHitPoints / (float)hitPoints);
			}
		}
	}

	void Start()
	{
		lifeBar = GetComponent<LifeIndicator>();
		engagingEnemies = new List<ArmedUnit>();
		RegisterUnit();
		CurrentHitPoints = hitPoints;
	}	

	public void TakeDamage(int damage, int AP = 0) 
	{
		int armourModifier = armour - AP;
		if(armourModifier < 0)
			armourModifier = 0;
			
		Debug.Log(this.name + "hase taken " + (damage - armourModifier) + " damage: " + damage + " - " + armourModifier);
		CurrentHitPoints -= damage - armourModifier;

		if(CurrentHitPoints <= 0)
		{
			OnDestruction();
		}
		else
		{
			GameObject damageEffect = Instantiate(damageEffectPrefab, transform.position, transform.rotation) as GameObject;
			damageEffect.transform.parent = GameController.Instance.transform;
			Destroy(damageEffect, damageEffect.GetComponent<ParticleSystem>().main.duration);
		}
	}

	public void RegisterEngagingEnemy(ArmedUnit armedUnit)
	{
		engagingEnemies.Add(armedUnit);
	}

	public void UnRegisterEngagingEnemy(ArmedUnit armedUnit)
	{
		engagingEnemies.Remove(armedUnit);
	}

	protected void NotifyOnDestruction()
	{
		foreach(var armedUnit in engagingEnemies)
		{
			armedUnit.Untarget(this);
		}
	}

	protected virtual void OnDestruction()
	{
		InstantiateExplosion();
		NotifyOnDestruction();
		UnRegisterUnit();
		Destroy(gameObject,0.1f);
	}


	void InstantiateExplosion()
	{
		GameObject expolsion = Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation, GameController.Instance.transform) as GameObject;
		// expolsion.transform.parent = GameController.Instance.transform;
		Destroy(expolsion, expolsion.GetComponent<ParticleSystem>().main.duration);
	}

	protected void RegisterUnit()
	{
		if(isEnemy)
		{
			GameController.Instance.enemyUnits.Add(this);
		}
		else
		{
			GameController.Instance.playerUnits.Add(this);
		}
	}

	protected void UnRegisterUnit()
	{
		if(isEnemy)
		{
			GameController.Instance.enemyUnits.Remove(this);
		}
		else
		{
			GameController.Instance.playerUnits.Remove(this);
		}
	}
}
