using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICompassRose : MonoBehaviour
{
	public RectTransform Needle;
	public RectTransform WindNeedle;

	Transform cameraTransform;
	Vector3 north;
		
	void Start ()
	{
		cameraTransform = Camera.main.transform;
		north = Vector3.forward;
	}

	void Update ()
	{
		Vector3 cameraForward = new Vector3 (cameraTransform.forward.x, 0f, cameraTransform.forward.z);

		var angle = Utils.Math.AngleSigned (cameraForward.normalized, north, Vector3.up);

		Needle.rotation = Quaternion.Euler (0f, 0f, angle);

		angle = Utils.Math.AngleSigned (WeatherController.Instance.GetWindVector (), north, Vector3.up);
		WindNeedle.rotation = Quaternion.Euler (0f, 0f, angle);
	}
}
