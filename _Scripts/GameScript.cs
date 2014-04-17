using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Transform boardPiecePrefab;
	private Transform[][] boardPieces;

	// Use this for initialization
	void Start () {
		int boardSize = 8;

		for(int i = 0; i < boardSize; i++){
			for(int j = 0; j < boardSize; j++){
				Vector3 nextLocation = new Vector3(-35 + 10f * i, 0, -35 + 10f * j);
				Transform newPiece = (Transform)Instantiate(boardPiecePrefab, nextLocation, Quaternion.identity);
				Transform board = GameObject.FindGameObjectWithTag ("Board").transform;
				newPiece.parent = board;
				if((i + j) % 2 == 0){
					newPiece.renderer.material.color = Color.white;
				} else {
					newPiece.renderer.material.color = Color.black;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
