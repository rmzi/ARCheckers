using UnityEngine;
using System.Collections;

public class GamePieceScript : MonoBehaviour {

	public bool isDead;
	public Vector2 location;
	public Color color;
	public bool isKing;
	public int player;

	// Use this for initialization
	void Start () {
		gameObject.tag = "piece";
		isKing = false;
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setColor(int player){
		this.player = player;
		if(player==1){
			color = Color.gray;
		}else{
			color = Color.red;
		}
		transform.gameObject.renderer.material.color = color;
	}
	public void focus(){
		transform.gameObject.renderer.material.color = Color.cyan;
	}
	public void select(){
		transform.gameObject.renderer.material.color = Color.magenta;
	}
	public void resetColor(){
		transform.gameObject.renderer.material.color = color;
	}
	public void die(){
		isDead = true;
		Destroy (this.gameObject);
	}
	public void makeKing(){
		isKing = true;
		transform.localScale += new Vector3 (0f, 3f, 0f);
	}
	public void setLocation(Vector3 loc){
		transform.position = loc;
	}
}
