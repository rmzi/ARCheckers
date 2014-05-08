using UnityEngine;
using System.Collections;

public class CubeSpaceScript : MonoBehaviour {

	// Use this for initialization
	private Color color;
	public bool isOccupied;
	public GameScript game;
	private GamePieceScript piece;
	private bool isValid;
	private Color highlightColor;
	private Color fromColor;
	Vector2 boardSpot;
	void Start () {
		isOccupied = false;
		transform.tag = "space";
		this.makeInvalid ();
		piece = null;
		highlightColor = Color.yellow;
		fromColor = Color.grey;
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
		Debug.DrawRay (other.transform.position, Vector3.down * 10);
		RaycastHit hit;
		
		if (Physics.Raycast (other.transform.position, Vector3.down, out hit, 10)) {
			Debug.Log("Hit tag: " + hit.transform.tag);
			if(hit.transform.tag == "board"){
				if(hit.transform.GetComponent<CubeSpaceScript>().getLocation().Equals(boardSpot)){
					this.highlight();
				}
				// Highlight Piece that's focused on
				Debug.Log("hovering over black spot");
			} else {
				Debug.Log("Searching");
			}
		}
		
	}
	void OnTriggerExit(Collider other){
		resetColor ();
	}
	public void highlight(){
		transform.gameObject.renderer.material.color = highlightColor;
	}
	public void fromHightlight(){
		transform.gameObject.renderer.material.color = fromColor;
	}
	public void setSpace(Vector2 loc){
		boardSpot = loc;
	}
	public Vector2 getLocation(){
		return boardSpot;
	}
	public void makeValid(){
		isValid = true;
		highlightColor = Color.green;
	}
	public void makeInvalid(){
		isValid = false;
		highlightColor = Color.yellow;
	}
	public void resetColor(){
		transform.gameObject.renderer.material.color = color;
	}
	public void setPiece(Transform piece){
		isOccupied = true;
		this.piece = piece.GetComponent<GamePieceScript>();
	}
	public GamePieceScript getPiece(){
		return this.piece;
	}
	public void unOccupy(){
		isOccupied = false;
		this.piece = null;
	}
}
