using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Transform boardPiecePrefab;
	private Transform[][] boardPieces;

	// Use this for initialization
	void Start () {
		Transform newPiece = (Transform)Instantiate (boardPiecePrefab, transform.position, Quaternion.identity);
		Transform board = GameObject.FindGameObjectWithTag ("Board").transform;
		newPiece.parent = board;
		newPiece.renderer.material.color = Color.green;

		int boardSize = 8;
		/*
		for(int i = 0; i < boardSize; i++){
			for(int i = 0; i < boardSize; i++){
				Instantiate(boardPiecePrefab, 
			}
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
