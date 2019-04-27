using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddWavesEditor : ScriptableWizard {

	public List<Destructable> enemies;
	public List<Transform> waypoints;
	public float startTime;

	[MenuItem("My Tools/Create Wave")]
	static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<AddWavesEditor> ("Create Wave", "Create new", "Update selected");
    }

}
