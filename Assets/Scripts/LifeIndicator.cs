using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LifeIndicator : MonoBehaviour {

	Renderer [] renderers;
	Slider slider;
	Image repairIcon;

	void Awake()
	{
		renderers = GetComponentsInChildren<Renderer>();
		slider = Instantiate(Resources.Load("HealthBar", typeof(Slider)), GameController.Instance.canvas.transform, false) as Slider;
		repairIcon = Instantiate(Resources.Load("RepairIcon", typeof(Image)),GameController.Instance.canvas.transform, false ) as Image;
		
		Assert.IsNotNull(renderers);
		Assert.IsNotNull(slider);
		Assert.IsNotNull(repairIcon);
		repairIcon.gameObject.SetActive(false);
	}

	void Update()
	{

		float minX = Camera.main.WorldToScreenPoint(renderers[0].bounds.min).x;
		float maxX = Camera.main.WorldToScreenPoint(renderers[0].bounds.max).x;
		float maxY = Camera.main.WorldToScreenPoint(renderers[0].bounds.max).y;

		for(int i = 1; i < renderers.Length; i++)
		{
			if(Camera.main.WorldToScreenPoint(renderers[i].bounds.min).x < minX)
			{
				minX = Camera.main.WorldToScreenPoint(renderers[i].bounds.min).x;
			}
			if(Camera.main.WorldToScreenPoint(renderers[i].bounds.max).x > maxX)
			{
				maxX = Camera.main.WorldToScreenPoint(renderers[i].bounds.max).x;
			}
			if(Camera.main.WorldToScreenPoint(renderers[i].bounds.max).y > maxY)
			{
				maxY = Camera.main.WorldToScreenPoint(renderers[i].bounds.max).y;
			}
		}

		RectTransform rt = slider.transform as RectTransform;
		RectTransform rtRepairIcon = repairIcon.transform as RectTransform;

		rt.position = new Vector3(minX, maxY +15f, 0f); 
		rt.sizeDelta = new Vector2(maxX - minX, 12f);

		rtRepairIcon.position = new Vector3(maxX + 2f, maxY +15f, 0f); 
	}

	void OnDestroy()
	{
		if(slider != null)
			Destroy(slider.gameObject);

		if(repairIcon != null)
			Destroy(repairIcon.gameObject);
	}

	public void SetValue(float value)
	{
		slider.value = value;
	}

	public void EnableRepairIcon(bool flag)
	{
		repairIcon.gameObject.SetActive(flag);
	}
}
