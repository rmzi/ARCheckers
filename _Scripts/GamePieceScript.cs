using UnityEngine;
using System.Collections;

public class GamePieceScript : MonoBehaviour {

	public bool isDead;
	public bool isMoving;
	public Vector2 location;
	public Color color;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setColor(Color c){
		color = c;
		transform.gameObject.renderer.material.color = color;
	}
}
