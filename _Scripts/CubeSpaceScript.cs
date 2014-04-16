using UnityEngine;
using System.Collections;

public class CubeSpaceScript : MonoBehaviour {

	// Use this for initialization
	private Color color;
	void Start () {
		transform.gameObject.renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setColor(Color c){
		color = c;
	}
}
