using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BuildOptionPanel : MonoBehaviour {

	Text nameText;
	Text costText;
	int cost = 0;
	
	[SerializeField]
	[Tooltip("the color of the next when there are sufficent funds")]
	Color goodColor;
	[SerializeField]
	[Tooltip("the color of the next when there are NOT sufficent funds")]
	Color badColor;

	public void Init(string nameArg, int costArg, int x, int y)
	{
		nameText = transform.Find("NameText").GetComponent<Text>();
		costText = transform.Find("CostText").GetComponent<Text>();
		Assert.IsNotNull(nameText);
		Assert.IsNotNull(costText);

		nameText.text = nameArg;
		cost = costArg;
		costText.text = costArg + " $";

		nameText.color = goodColor;
		costText.color = goodColor;
		StartCoroutine(SetPosition(x, y));

		
	}

	public void SetName(string newName)
	{
		nameText.text = newName;
	}

	public void SetCost(int newCost)
	{
		cost = newCost;
		costText.text = newCost + " $";
	}

	void Update()
	{
		if(cost < GameController.Instance.Money)
		{
			nameText.color = goodColor;
			costText.color = goodColor;
		}
		else
		{
			nameText.color = badColor;
			costText.color = badColor;
		}
	}

	public IEnumerator SetPosition(int x, int y)
	{
		yield return null;
		RectTransform rectTrans = GetComponent<RectTransform>();
		if(rectTrans != null)
		{
			rectTrans.anchoredPosition = new Vector2(x, y);
		}
		yield return null;
	}
}
