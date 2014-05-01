using UnityEngine;
using System.Collections;

public class CubeSpaceScript : MonoBehaviour {

	// Use this for initialization
	private Color color;
	private bool isOccupied;
	void Start () {
		isOccupied = false;
		transform.tag = "space";
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setColor(Color c){
		color = c;
		transform.gameObject.renderer.material.color = color;
		if (color != Color.black) {
			transform.collider.isTrigger=false;
		}
	}
	void OnTriggerStay(Collider other) {

		
	}
	void OnTriggerExit(Collider other){
	
	}
	public void highlight(Color h){
		transform.gameObject.renderer.material.color = h;
	}
}
