using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour 
{
	public Transform boardPiecePrefab;
	public Transform gamePiecePrefab;
	private GameObject[,] boardPieces;

	public int turn;
	private Player player1;
	private Player player2;
	private GameObject board;
	private GameObject redTray;
	private GameObject blackTray;
	private ArrayList lastMove;
	private bool showingMoves;
	private ArrayList highlightedCubes;
	private Vector2 currLoc;

	// Use this for initialization
	void Start () {
		showingMoves = false;
		highlightedCubes = new ArrayList ();
		boardPieces = new GameObject[8, 8];

		player1 = new Player(1);
		player2 = new Player(2);
		board = GameObject.FindGameObjectWithTag ("Board");
		redTray = GameObject.FindGameObjectWithTag ("redTray");
		blackTray = GameObject.FindGameObjectWithTag ("blackTray");
		turn = 1;
		lastMove = new ArrayList ();
	}
	public void showMoves(GameObject piece){
		Debug.Log ("Piece Spot:"+piece.GetComponent<GamePieceScript>().location.ToString());
		CheckersMove[] moves = getLegalMoves(turn);
		foreach (CheckersMove move in moves) {
			if(move.fromCol == piece.GetComponent<GamePieceScript>().location.y && move.fromRow == piece.GetComponent<GamePieceScript>().location.x){
				boardPieces[move.toRow,move.toCol].GetComponent<CubeSpaceScript>().highlight();
				boardPieces[move.fromRow,move.fromCol].GetComponent<CubeSpaceScript>().fromHightlight();
				highlightedCubes.Add(boardPieces[move.toRow,move.toCol]);
				highlightedCubes.Add(boardPieces[move.fromRow,move.fromCol]);
			}
		}
	}
	public void stopShowingMoves(){
		if (highlightedCubes.Count != 0) {
			foreach (Object cube in highlightedCubes) {
				CubeSpaceScript tmpCube = ((GameObject)cube).GetComponent<CubeSpaceScript>();
				tmpCube.resetColor();
			}
			highlightedCubes.Clear();
		}
	}
	public void makeBoard(){
			int boardSize = 8;
			for(int i = 0; i < boardSize; i++){
				for(int j = 0; j < boardSize; j++){
					currLoc = new Vector2((float)i,(float)j);
					Vector3 nextLocation = new Vector3(-35 + 10f * i, 0, -35 + 10f * j);
					Transform newPiece = Instantiate(boardPiecePrefab, nextLocation, Quaternion.identity) as Transform;
					newPiece.parent = board.transform;
					boardPieces[i,j] = newPiece.gameObject;
					if((i + j) % 2 == 0){
						CubeSpaceScript script = boardPieces[i,j].GetComponent<CubeSpaceScript> ();
						script.setColor(Color.red);
						script.setSpace(currLoc);
					} else {
						CubeSpaceScript script = boardPieces[i,j].GetComponent<CubeSpaceScript> ();
						script.setColor(Color.black);
						script.setSpace(currLoc);
						if(i!=3 && i!=4){
							Vector3 offset = new Vector3(0,10,0);
							Transform newGamePiece = Instantiate(gamePiecePrefab, nextLocation + offset, Quaternion.identity) as Transform;
							newGamePiece.parent = board.transform;
							newGamePiece.gameObject.GetComponent<GamePieceScript>().location = currLoc;
							if(i<4){
								boardPieces[i,j].GetComponent<CubeSpaceScript> ().setPiece(newGamePiece.gameObject);
								player1.addPiece(newGamePiece.gameObject);
							}else{
								boardPieces[i,j].GetComponent<CubeSpaceScript> ().setPiece(newGamePiece.gameObject);
								player2.addPiece(newGamePiece.gameObject);
							}
						}
					}
				}
			}
		//make trays
		redTray.renderer.material.color = Color.red;
		blackTray.renderer.material.color = Color.black;

		Debug.Log (boardPieces);

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void makeMove(CheckersMove move){

	}

	public class Player{
		public static int turn = 1;
		//player number
		public int player;
		//pieces that belong to the player
		public ArrayList pieces;
		public int numPieces;
		public Player(int p){
			player = p;
			numPieces = 0;
			pieces = new ArrayList();
		}
		public void addPiece(GameObject p){
			pieces[numPieces] = p;
			((GameObject)pieces[numPieces]).GetComponent<GamePieceScript>().setColor(player);
			numPieces++;
		}
		//public
		//allows player to request draw and pass the device to the other user to either accept or deny
		public bool requestDraw(){
			return false;
		}
	}

	//A move in checkers
	public class CheckersMove {
		public int fromRow, fromCol;  // Position of piece to be moved.
		public int toRow, toCol;      // Square it is to move to.
		public CheckersMove(int r1, int c1, int r2, int c2) {
			// Constructor.  Just set the values of the instance variables.
			fromRow = r1;
			fromCol = c1;
			toRow = r2;
			toCol = c2;
		}
		bool isJump() {
			// Test whether this move is a jump.  It is assumed that
			// the move is legal.  In a jump, the piece moves two
			// rows.  (In a regular move, it only moves one row.)
			return (fromRow - toRow == 2 || fromRow - toRow == -2);
		}
	}

	public CheckersMove[] getLegalMoves(int player) {
		ArrayList moves = new ArrayList();  // Moves will be stored in this list.
		if (this.turn == player1.player) {
			// Player1

			foreach(GameObject piece in player1.pieces){
				int row = (int) piece.GetComponent<GamePieceScript>().location.x;
				int col = (int) piece.GetComponent<GamePieceScript> ().location.y;
				if (canJump(player, row, col, row+1, col+1, row+2, col+2))
					moves.Add(new CheckersMove(row, col, row+2, col+2));
				if (canJump(player, row, col, row-1, col+1, row-2, col+2))
					moves.Add(new CheckersMove(row, col, row-2, col+2));
				if (canJump(player, row, col, row+1, col-1, row+2, col-2))
					moves.Add(new CheckersMove(row, col, row+2, col-2));
				if (canJump(player, row, col, row-1, col-1, row-2, col-2))
					moves.Add(new CheckersMove(row, col, row-2, col-2));
			}
			// FORCE MOVE 
			// If there are no valid jumps in the possible moveset,
			// Test single space moves.
			if (moves.Count == 0) {
				foreach(GameObject piece in player1.pieces){
					int row = (int) piece.GetComponent<GamePieceScript> ().location.x;
					int col = (int) piece.GetComponent<GamePieceScript> ().location.y;
					if (canMove(player,row,col,row+1,col+1))
						moves.Add(new CheckersMove(row,col,row+1,col+1));
					if (canMove(player,row,col,row-1,col+1))
						moves.Add(new CheckersMove(row,col,row-1,col+1));
					if (canMove(player,row,col,row+1,col-1))
						moves.Add(new CheckersMove(row,col,row+1,col-1));
					if (canMove(player,row,col,row-1,col-1))
						moves.Add(new CheckersMove(row,col,row-1,col-1));
				}
			}
			if (moves.Count == 0)
				return null;
			else {
				CheckersMove[] moveArray = new CheckersMove[moves.Count];
				for (int i = 0; i < moves.Count; i++)
					moveArray[i] = (CheckersMove)moves[i];
				return moveArray;
			}
		}else{
			// Player2
			foreach(GameObject piece in player2.pieces){
				int row = (int) piece.GetComponent<GamePieceScript> ().location.x;
				int col = (int) piece.GetComponent<GamePieceScript> ().location.y;
				if (canJump(player, row, col, row+1, col+1, row+2, col+2))
					moves.Add(new CheckersMove(row, col, row+2, col+2));
				if (canJump(player, row, col, row-1, col+1, row-2, col+2))
					moves.Add(new CheckersMove(row, col, row-2, col+2));
				if (canJump(player, row, col, row+1, col-1, row+2, col-2))
					moves.Add(new CheckersMove(row, col, row+2, col-2));
				if (canJump(player, row, col, row-1, col-1, row-2, col-2))
					moves.Add(new CheckersMove(row, col, row-2, col-2));
			}
			if (moves.Count == 0) {
				foreach(GameObject piece in player2.pieces){
					int row = (int) piece.GetComponent<GamePieceScript> ().location.x;
					int col = (int) piece.GetComponent<GamePieceScript> ().location.y;
					if (canMove(player,row,col,row+1,col+1))
						moves.Add(new CheckersMove(row,col,row+1,col+1));
					if (canMove(player,row,col,row-1,col+1))
						moves.Add(new CheckersMove(row,col,row-1,col+1));
					if (canMove(player,row,col,row+1,col-1))
						moves.Add(new CheckersMove(row,col,row+1,col-1));
					if (canMove(player,row,col,row-1,col-1))
						moves.Add(new CheckersMove(row,col,row-1,col-1));
				}
			}
			if (moves.Count == 0)
				return null;
			else {
				CheckersMove[] moveArray = new CheckersMove[moves.Count];
				for (int i = 0; i < moves.Count; i++)
					moveArray[i] =(CheckersMove)moves[i];
				return moveArray;
			}
		}
	}

	/**
       * Return a list of the legal jumps that the specified player can
       * make starting from the specified row and column.  If no such
       * jumps are possible, null is returned.  The logic is similar
       * to the logic of the getLegalMoves() method.
       */
	CheckersMove[] getLegalJumpsFrom(int player, int row, int col) {
		if (player != player1.player && player != player2.player)
			return null;

		ArrayList moves = new ArrayList();  // The legal jumps will be stored in this list.
		if (boardPieces[row,col].GetComponent<CubeSpaceScript>().getPiece().GetComponent<GamePieceScript>().player == player) {
			if (canJump(player, row, col, row+1, col+1, row+2, col+2))
				moves.Add(new CheckersMove(row, col, row+2, col+2));
			if (canJump(player, row, col, row-1, col+1, row-2, col+2))
				moves.Add(new CheckersMove(row, col, row-2, col+2));
			if (canJump(player, row, col, row+1, col-1, row+2, col-2))
				moves.Add(new CheckersMove(row, col, row+2, col-2));
			if (canJump(player, row, col, row-1, col-1, row-2, col-2))
				moves.Add(new CheckersMove(row, col, row-2, col-2));
		}
		if (moves.Count == 0)
			return null;
		else {
			CheckersMove[] moveArray = new CheckersMove[moves.Count];
			for (int i = 0; i < moves.Count; i++)
				moveArray[i] = (CheckersMove)moves[i];
			return moveArray;
		}
	}
	
	/**
       * This is called by the two previous methods to check whether the
       * player can legally jump from (r1,c1) to (r3,c3).  It is assumed
       * that the player has a piece at (r1,c1), that (r3,c3) is a position
       * that is 2 rows and 2 columns distant from (r1,c1) and that 
       * (r2,c2) is the square between (r1,c1) and (r3,c3).
       */
	private bool canJump(int player, int r1, int c1, int r2, int c2, int r3, int c3) {

		// (r3,c3) is off the board.
		if (r3 < 0 || r3 >= 8 || c3 < 0 || c3 >= 8)
			return false; 
		
		if (boardPieces[r3,c3].GetComponent<CubeSpaceScript>().isOccupied)
			return false;  // (r3,c3) already contains a piece.
		
		if (player == player1.player) {
			if (r3 < r1 && boardPieces[r1,c1].GetComponent<CubeSpaceScript>().getPiece().GetComponent<GamePieceScript>().isKing==false)
				return false;  // Regular black piece can only move up.
			else
				Debug.Log("not a regular piece");
			if (boardPieces[r2,c2].GetComponent<CubeSpaceScript>().getPiece()!=null && boardPieces[r2,c2].GetComponent<CubeSpaceScript>().getPiece().GetComponent<GamePieceScript>().player != player2.player)
				return false;  // There is no black piece to jump.
			return true;  // The jump is legal.
		}
		else {
			if (boardPieces[r1,c1].GetComponent<CubeSpaceScript>().getPiece().GetComponent<GamePieceScript>().isKing==false && r3 > r1)
				return false;  // Regular black piece can only move downn.
			else
				Debug.Log("not a regular piece");
			if (boardPieces[r2,c2].GetComponent<CubeSpaceScript>().getPiece()!=null && boardPieces[r2,c2].GetComponent<CubeSpaceScript>().getPiece().GetComponent<GamePieceScript>().player!= player1.player)
				return false;  // There is no red piece to jump.
			return true;  // The jump is legal.
		}
		
	}
	
	
	/**
       * This is called by the getLegalMoves() method to determine whether
       * the player can legally move from (r1,c1) to (r2,c2).  It is
       * assumed that (r1,r2) contains one of the player's pieces and
       * that (r2,c2) is a neighboring square.
       */
	private bool canMove(int player, int r1, int c1, int r2, int c2) {
		if (r2 < 0 || r2 >= 8 || c2 < 0 || c2 >= 8)
				return false;  // (r2,c2) is off the board.
		if (boardPieces[r2, c2].GetComponent<CubeSpaceScript>().isOccupied)
				return false;  // (r2,c2) already contains a piece.

		if (player == player1.player) {
			if (boardPieces[r1, c1].GetComponent<CubeSpaceScript>().getPiece().GetComponent<GamePieceScript>().player == player1.player && r2 > r1)
						return false;  // Regular red piece can only move down.
				return true;  // The move is legal.
		} else {
			if (boardPieces[r1, c1].GetComponent<CubeSpaceScript>().getPiece().GetComponent<GamePieceScript>().player == player2.player && r2 < r1)

						return false;  // Regular black piece can only move up.
				return true;  // The move is legal.
		}
	}

}
