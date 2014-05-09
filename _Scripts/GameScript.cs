using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour 
{
	public Transform boardPiecePrefab;
	public Transform gamePiecePrefab;
	private Transform[,] boardPieces;
	private CubeSpaceScript[,] gameBoard;
	private int turn;
	private Player player1;
	private Player player2;
	private Transform board;
	private ArrayList lastMove;
	private bool showingMoves;
	private ArrayList highlightedCubes;
	private Vector2 currLoc;

	// Use this for initialization
	void Start () {
		showingMoves = false;
		gameBoard	= new CubeSpaceScript[8,8];
		boardPieces = new Transform[8,8];
		player1 = new Player(1);
		player2 = new Player(2);
		board = GameObject.FindGameObjectWithTag ("Board").transform;
		turn = 1;
		lastMove = new ArrayList ();
	}
	public void showMoves(GamePieceScript piece){
		Debug.Log ("Piece Spot:"+piece.location.ToString());
		CheckersMove[] moves = getLegalMoves(turn);
		ArrayList myMoves = new ArrayList ();
		foreach (CheckersMove move in moves) {
			/*gameBoard[move.toRow,move.toCol].highlight();
			gameBoard[move.fromRow,move.fromCol].fromHightlight();
			myMoves.Add(gameBoard[move.toRow,move.toCol]);
			myMoves.Add(gameBoard[move.fromRow,move.fromCol]);*/
			if(move.fromCol == piece.location.y && move.fromRow == piece.location.x){
				gameBoard[move.toRow,move.toCol].highlight();
				gameBoard[move.fromRow,move.fromCol].fromHightlight();
				myMoves.Add(gameBoard[move.toRow,move.toCol]);
				myMoves.Add(gameBoard[move.fromRow,move.fromCol]);
			}
		}
		highlightedCubes = myMoves;

	}
	public void stopShowingMoves(){
		foreach (Object cube in highlightedCubes) {
			CubeSpaceScript tmpCube = (CubeSpaceScript) cube;
			tmpCube.resetColor();
		}
	}
	public void makeBoard(){
			int boardSize = 8;
			for(int i = 0; i < boardSize; i++){
				for(int j = 0; j < boardSize; j++){
					currLoc = new Vector2((float)i,(float)j);
					Vector3 nextLocation = new Vector3(-35 + 10f * i, 0, -35 + 10f * j);
					boardPieces[i,j] = (Transform)Instantiate(boardPiecePrefab, nextLocation, Quaternion.identity);
					boardPieces[i,j].parent = board;
					if((i + j) % 2 == 0){
						gameBoard[i,j] = boardPieces[i,j].GetComponent<CubeSpaceScript> ();
						gameBoard[i,j].setColor(Color.red);
						gameBoard[i,j].setSpace(currLoc);
					} else {
						gameBoard[i,j] = boardPieces[i,j].GetComponent<CubeSpaceScript> ();
						gameBoard[i,j].setColor(Color.black);
						gameBoard[i,j].setSpace(currLoc);
						if(i!=3 && i!=4){
							Vector3 offset = new Vector3(0,10,0);
							Transform newGamePiece = (Transform)Instantiate(gamePiecePrefab, nextLocation + offset, Quaternion.identity);
							newGamePiece.parent = board;
							newGamePiece.GetComponent<GamePieceScript>().location = currLoc;
							
							if(i<4){
								gameBoard[i,j].setPiece(newGamePiece);
								player1.addPiece(newGamePiece);
							}else{
								gameBoard[i,j].setPiece(newGamePiece);
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

	public void makeMove(CheckersMove move){

	}
	public class Player{
		public static int turn=1;
		//player number
		public int player;
		//pieces that belong to the player
		public GamePieceScript[] pieces;
		public Transform[] piecesTransform;
		public int numPieces;
		public Player(int p){
			player = p;
			numPieces = 0;
			pieces = new GamePieceScript[12];
			piecesTransform = new Transform[12];
		}
		public void addPiece(Transform p){
			pieces[numPieces]=p.GetComponent<GamePieceScript> ();
			pieces[numPieces].setColor (player);
			piecesTransform [numPieces] = p;
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
			foreach(GamePieceScript piece in player1.pieces){
				int row = (int) piece.location.x;
				int col = (int) piece.location.y;
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
				foreach(GamePieceScript piece in player1.pieces){
					int row = (int) piece.location.x;
					int col = (int) piece.location.y;
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
			foreach(GamePieceScript piece in player2.pieces){
				int row = (int) piece.location.x;
				int col = (int) piece.location.y;
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
				foreach(GamePieceScript piece in player2.pieces){
					int row = (int) piece.location.x;
					int col = (int) piece.location.y;
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
		if (gameBoard[row,col].getPiece().GetComponent<GamePieceScript>().player == player) {
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
		
		if (gameBoard[r3,c3].isOccupied)
			return false;  // (r3,c3) already contains a piece.
		
		if (player == player1.player) {
			if (gameBoard[r1,c1].getPiece().GetComponent<GamePieceScript>().isKing==false && r3 < r1)
				return false;  // Regular black piece can only move up.
			else
				Debug.Log("not a regular piece");
			if (gameBoard[r2,c2].getPiece()!=null && gameBoard[r2,c2].getPiece().GetComponent<GamePieceScript>().player != player2.player)
				return false;  // There is no black piece to jump.
			return true;  // The jump is legal.
		}
		else {
			if (gameBoard[r1,c1].getPiece().GetComponent<GamePieceScript>().isKing==false && r3 > r1)
				return false;  // Regular black piece can only move downn.
			else
				Debug.Log("not a regular piece");
			if (gameBoard[r2,c2].getPiece()!=null && gameBoard[r2,c2].getPiece().GetComponent<GamePieceScript>().player!= player1.player)
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

		if (gameBoard [r2, c2].isOccupied)
				return false;  // (r2,c2) already contains a piece.

		if (player == player1.player) {
			if (gameBoard [r1, c1].getPiece ().GetComponent<GamePieceScript>().player == player1.player && r2 > r1)
						return false;  // Regular red piece can only move down.
				return true;  // The move is legal.
		} else {
			if (gameBoard [r1, c1].getPiece ().GetComponent<GamePieceScript>().player == player2.player && r2 < r1)
						return false;  // Regular black piece can only move up.
				return true;  // The move is legal.
		}
	}

}
