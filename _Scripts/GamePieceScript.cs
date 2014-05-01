using UnityEngine;
using System.Collections;

public class GamePieceScript : MonoBehaviour {

	public bool isDead;
	public bool isMoving;
	public Vector2 location;
	public Color color;
	public bool isKing;

	// Use this for initialization
	void Start () {
		transform.tag = "piece";
		isKing = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setColor(Color c){
		color = c;
		transform.gameObject.renderer.material.color = color;
	}
	public void focus(){
		transform.gameObject.renderer.material.color = Color.cyan;
	}
	public void select(){
		transform.gameObject.renderer.material.color = Color.red;
	}
	public void resetColor(){
		transform.gameObject.renderer.material.color = color;
	}
}
