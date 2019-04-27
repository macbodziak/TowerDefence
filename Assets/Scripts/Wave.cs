using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave : MonoBehaviour, IComparable<Wave>{

	[SerializeField] List<MobileUnit> enemyPrefabs;
	List<MobileUnit> enemies;
	[SerializeField] List<Transform> waypoints;
	[SerializeField] float startTime;
	[SerializeField] float spawningInterval = 1.5f;
	[SerializeField] bool commonSpeed;
	int index = 0;
	float minSpeed = float.PositiveInfinity;

	public int CompareTo(Wave other)
	{
		if(startTime < other.startTime)
		{
			return -1;
		}
		else if(startTime > other.startTime)
		{
			return 1;
		}

		return 0;
	}

	IEnumerator Start()
	{
		enemies = new List<MobileUnit>();
		InstantiateAllEnemies();
		yield return new WaitForSeconds(startTime);
		StartCoroutine(StartWave());
	}
	
	IEnumerator StartWave()
	{
		while(index < enemies.Count)
		{
			SpawnNextEnemy();
			yield return new WaitForSeconds(spawningInterval);
		}
		yield return null;
	}

	void SpawnNextEnemy()
	{
		if(commonSpeed)
		{
			enemies[index].Speed = minSpeed;
		}
		enemies[index].gameObject.SetActive(true);
		index++;
	}

	void InstantiateAllEnemies()
	{
		for(int i = 0; i < enemyPrefabs.Count; i++)
		{
			MobileUnit obj = Instantiate(enemyPrefabs[i],waypoints[0].position,waypoints[0].rotation);
			enemies.Add(obj);
			obj.Waypoints = waypoints;
			obj.gameObject.SetActive(false);
			
			//check for the minimum 
			if(commonSpeed)
			{
				if(enemyPrefabs[i].Speed < minSpeed)
				{
					minSpeed = enemyPrefabs[i].Speed;
				}
			}
		}
	}
}
