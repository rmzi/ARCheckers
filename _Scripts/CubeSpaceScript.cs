using UnityEngine;
using System.Collections;

public class CubeSpaceScript : MonoBehaviour {

	// Use this for initialization
	private Color color;
	private bool isOccupied;
	void Start () {
		isOccupied = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setColor(Color c){
		color = c;
		transform.gameObject.renderer.material.color = color;
	}
}
