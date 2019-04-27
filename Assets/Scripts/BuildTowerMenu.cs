using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

[System.Serializable]
public struct TowerBuildOption {
	public Tower towerPrefab;
	public Sprite sprite;
	public string towerName;
}

public class BuildTowerMenu : TowerPlatformMenu {

	[SerializeField] TowerBuildOption [] options;
	[SerializeField] GameObject buttonPrefab;
	LayoutGroup layoutGroup;
	int x;
	int y;

	public override void Init(TowerPlatform _towerPlatform, int x_, int y_)
	{
		x = x_;
		y = y_;
		layoutGroup = GetComponent<LayoutGroup>();
		Assert.IsNotNull(buttonPrefab);
		for(int i = 0; i < options.Length; i++)
		{
			RegisterButton(options[i]);
		}
		base.Init(_towerPlatform, x_, y_);
	}

	void RegisterButton(TowerBuildOption option)
	{
		GameObject obj = Instantiate(buttonPrefab, layoutGroup.transform, false);
		obj.GetComponent<Image>().sprite = option.sprite;
		obj.GetComponent<BuildTowerButton>().Init(option.towerName, option.towerPrefab.Cost,x,y);
		obj.GetComponent<Button>().onClick.AddListener(() => playSelectionSound(option.towerPrefab));
		obj.GetComponent<Button>().onClick.AddListener(() => towerPlatform.BuildTower(option.towerPrefab));
		obj.GetComponent<Button>().onClick.AddListener(() => Destroy(gameObject));
	}

	void playSelectionSound(Tower tower)
	{
		if(tower.Cost <= GameController.Instance.Money)
		{
			PlayConfirmSound();
		}
		else
		{
			PlayRejectSound();
		}
	}
}

