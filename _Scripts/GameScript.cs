using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Transform boardPiecePrefab;
	public Transform gamePiecePrefab;
	private Transform[,] boardPieces;
	private CubeSpaceScript[,] gameBoard;
	private GamePieceScript[] player1GamePieces;
	private GamePieceScript[] player2GamePieces;
	private Transform board;

	// Use this for initialization
	void Start () {
		gameBoard	= new CubeSpaceScript[8,8];
		boardPieces = new Transform[8,8];
		player1GamePieces = new GamePieceScript[12];
		player2GamePieces = new GamePieceScript[12];
		board = GameObject.FindGameObjectWithTag ("Board").transform;
	}

	public void makeBoard(){
			int boardSize = 8;
			for(int i = 0; i < boardSize; i++){
				for(int j = 0; j < boardSize; j++){
					Vector3 nextLocation = new Vector3(-35 + 10f * i, 0, -35 + 10f * j);
					boardPieces[i,j] = (Transform)Instantiate(boardPiecePrefab, nextLocation, Quaternion.identity);
					boardPieces[i,j].parent = board;
					if((i + j) % 2 == 0){
						gameBoard[i,j] = boardPieces[i,j].GetComponent<CubeSpaceScript> ();
						gameBoard[i,j].setColor(Color.red);
					} else {
						gameBoard[i,j] = boardPieces[i,j].GetComponent<CubeSpaceScript> ();
						gameBoard[i,j].setColor(Color.black);
						if(i!=4 || i!=5){
							Vector3 offset = new Vector3(0,1,0);
							Transform newGamePiece = (Transform)Instantiate(gamePiecePrefab, nextLocation + offset, Quaternion.identity);
							newGamePiece.parent = board;
							if(i<4){
								newGamePiece.renderer.material.color = Color.red;
							}else{
								newGamePiece.renderer.material.color = Color.gray;
							}
						}
					}
				}
			}
		

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public class Player{
		public int player;
		public GamePieceScript[] pieces;
		public int numPieces;
		Player(int p){
			player = p;
			numPieces = 0;
		}
	}
}
