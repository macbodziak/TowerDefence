using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class TowerPlatformMenu : MonoBehaviour {

	[SerializeField] int margin;
	[SerializeField] protected AudioClip startupAudioClip;
	[SerializeField] protected AudioClip confirmAudioClip;
	[SerializeField] protected AudioClip rejectAudioClip;
	protected TowerPlatform towerPlatform;
	int minX;
	int maxX;
	int minY;
	int maxY;
	protected bool initialized = false;

	void Awake()
	{
		Assert.IsNotNull(startupAudioClip);
		Assert.IsNotNull(confirmAudioClip);
		Assert.IsNotNull(rejectAudioClip);
	}
	
	protected virtual void Update () 
	{
		if(initialized && (Input.mousePosition.x < minX || Input.mousePosition.x > maxX || Input.mousePosition.y < minY || Input.mousePosition.y > maxY))
		{
			Debug.Log("destroying" + name);
			Destroy(gameObject);
		}
	}

	virtual public void Init(TowerPlatform _towerPlatform, int x, int y)
	{
		towerPlatform = _towerPlatform;
		StartCoroutine(SetPosition(x,y));
		GameController.Instance.MenuAudioSource.PlayOneShot(startupAudioClip);
	}

	public IEnumerator SetPosition(int x, int y)
	{
		yield return null;
		RectTransform rectTrans = GetComponent<RectTransform>();
		if(rectTrans != null)
		{
			rectTrans.anchoredPosition = new Vector2(x, y);

			//set up boundry values
			minX = x - margin;
			maxX = x + (int)(rectTrans.sizeDelta.x) + margin;

			maxY = y + margin;
			minY = y - (int)(rectTrans.sizeDelta.y) - margin;
		}
		initialized = true;
		yield return null;
	}

	public void PlayRejectSound()
	{
		GameController.Instance.MenuAudioSource.PlayOneShot(rejectAudioClip);
	}

	public void PlayConfirmSound()
	{
		GameController.Instance.MenuAudioSource.PlayOneShot(confirmAudioClip);
	}
}
