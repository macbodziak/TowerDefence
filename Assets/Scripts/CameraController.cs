using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] float moveSensitivity;
	[SerializeField] float zoomSensitivity;
	[SerializeField] Vector2 rangeX;
	[SerializeField] Vector2 rangeY;
	[SerializeField] Vector2 rangeZ;
	Vector3 refPos;

	void Update () 
	{
		Vector3 newPos = Camera.main.transform.position;

		if(Input.GetMouseButtonDown(2))
		{
			refPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		}
		if(Input.GetMouseButton(2))
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
			float dx = (pos.x - refPos.x) * moveSensitivity;
			float dz = (pos.y - refPos.y) * moveSensitivity;

			newPos += new Vector3(dx, 0.0f, dz);
		}

		float dy = Input.GetAxis("Mouse ScrollWheel");	
		if(dy != 0)
		{
			newPos += new Vector3(0.0f, - dy * zoomSensitivity, 0.0f);
		}

		newPos.x = Mathf.Clamp(newPos.x, rangeX.x, rangeX.y);
		newPos.y = Mathf.Clamp(newPos.y, rangeY.x, rangeY.y);
		newPos.z = Mathf.Clamp(newPos.z, rangeZ.x, rangeZ.y);

		Camera.main.transform.position = newPos;
	}
}
