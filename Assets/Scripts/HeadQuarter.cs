using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadQuarter : Destructable {
	
	protected override void OnDestruction()
	{
		base.OnDestruction();
		GameController.Instance.GameOver();
	}

}
