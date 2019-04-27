using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildTowerButton : Button {

	[SerializeField] BuildOptionPanel infoPanelPrefab;
	BuildOptionPanel infoPanel;

	int x;
	int y;
	string towerName;
	int cost;

	public void Init(string towerName_, int cost_, int x_, int y_)
	{
		towerName = towerName_;
		cost = cost_;
		x = x_;
		y = y_;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		infoPanel = Instantiate(infoPanelPrefab, GameController.Instance.canvas.transform, false);
		infoPanel.Init(towerName, cost, x, y);
		base.OnPointerEnter(eventData);
		Debug.Log("Pointer ENter");
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		Destroy(infoPanel.gameObject);
		base.OnPointerExit(eventData);
		Debug.Log("Pointer Exit");
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if(infoPanel != null)
			Destroy(infoPanel.gameObject);
	}
}
