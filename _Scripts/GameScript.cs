using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Transform boardPiecePrefab;
	public Transform gamePiecePrefab;
	private Transform[,] boardPieces;
	private CubeSpaceScript[,] gameBoard;
	private Player player1;
	private Player player2;
	private Transform board;

	// Use this for initialization
	void Start () {
		gameBoard	= new CubeSpaceScript[8,8];
		boardPieces = new Transform[8,8];
		player1 = new Player(1);
		player2 = new Player(2);
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
						if(i!=3 && i!=4){
							Vector3 offset = new Vector3(0,10,0);
							Transform newGamePiece = (Transform)Instantiate(gamePiecePrefab, nextLocation + offset, Quaternion.identity);
							newGamePiece.parent = board;
							if(i<4){
								player1.addPiece(newGamePiece);
							}else{
								player2.addPiece(newGamePiece);
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
		public Transform[] piecesTransform;
		public int numPieces;
		public Color playerColor;
		Player(int p){
			player = p;
			numPieces = 0;
			pieces = new GamePieceScript[12];
			piecesTransform = new Transform[12];
			if(player==1){
				playerColor = Color.red;
			}else{
				playerColor = Color.grey;
			}
		}
		public void addPiece(Transform p){
			pieces[numPieces]=p.GetComponent<GamePieceScript> ();
			pieces[numPieces].setColor (playerColor);
			piecesTransform [numPieces] = p;
			numPieces++;
		}
	}
}
