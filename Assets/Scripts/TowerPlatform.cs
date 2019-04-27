using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlatform : MonoBehaviour {

	Tower tower;
	TowerPlatformMenu menu = null;

	public void ShowMenu()
	{
		//raycast mask so the tower does not block ray!
		if(menu != null)
		{
			return;
		}

		if(tower == null)
		{
			menu = Instantiate(GameController.Instance.buildTowerMenu, GameController.Instance.canvas.transform, false);
			// menu.GetComponent<TowerPlatformMenu>().Init(this,(int)Input.mousePosition.x, (int)Input.mousePosition.y);
		}
		else
		{
			menu = Instantiate(GameController.Instance.manageTowerMenu, GameController.Instance.canvas.transform, false);
		}
		// the line below should be uncommented once the manage tower menu has been implemented
		menu.GetComponent<TowerPlatformMenu>().Init(this,(int)Input.mousePosition.x - 1, (int)Input.mousePosition.y - 1);
	}

	public void BuildTower(Tower towerPrefab)
	{
		if(tower == null)
		{
			if(GameController.Instance.Money >= towerPrefab.Cost)
			{
				GameController.Instance.Money -= towerPrefab.Cost;
				tower = Instantiate(towerPrefab, transform.position + new Vector3(0f, 0.2f, 0f), Quaternion.identity, transform);
			}
		}
	}

	public Tower Tower 
	{
		get 
		{
			return tower;
		}
	}
}
