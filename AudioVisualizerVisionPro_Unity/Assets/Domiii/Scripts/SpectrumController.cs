using System;
using UnityEngine;
using System.Collections;

public class SpectrumController : MonoBehaviour {
	public float maxHeight;
	public int spectrumIndex;
	public float responseSpeed = 32;

	Vector3 scale;
	private float initialScaleZ;

	void Start() {
		scale = transform.localScale;
		initialScaleZ = transform.localScale.z;
	}

	private void OnValidate()
	{
		initialScaleZ = transform.localScale.z;
	}

	void  Update () {
		var desiredScale = initialScaleZ+AudioManager.spectrum[spectrumIndex]*maxHeight;
		//var desiredScale = Mathf.Log(AudioManager.spectrum[spectrumIndex], 2)*maxHeight * 100;
		scale.z = Mathf.Lerp(transform.localScale.z,desiredScale, Time.deltaTime * responseSpeed);
		transform.localScale = scale;
	}
}