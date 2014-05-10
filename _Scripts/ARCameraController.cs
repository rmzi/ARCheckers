using UnityEngine;
using System.Collections;



public class ARCameraController : MonoBehaviour{
	//GameScript
	private GameScript game;
	//joystick
	public Transform joystick;



	// Modes
	// Modes will determine which code to run to increase efficiency of calculations
	// Low level modes are nested in high level modes
	// All modes are ENUM and their values are listed below
	// High level modes: Pregame, Play & Explore
	private const int PREGAME_MODE = 0;
	private const int PLAY_MODE = 1;
	private const int EXPLORE_MODE = 2;
	private const int END_GAME_MODE = 3;

	// GUI Low level modes: Init, Start, Instructions
	private const int INIT_MODE = 0;
	private const int START_MODE = 1;
	private const int INSTR_MODE = 2;

	// UPDATE Low level modes: Selection, Movement, Scaling Mode
	private const int SEL_MODE = 3;
	private const int MOVE_MODE = 4;
	private const int SCALE_MODE = 5;
	private const int BASE_EX_MODE = 6;
	private const int PASS_MODE = 7;


	//Variables containing current modes for GAME
	public int highMode;
	public int lowMode;

	// Pass Mode 
	private Vector2 referenceRay = new Vector2(1f,0f);
	private Vector3 rayToCamera;
	private float radialAngle;

	//GameObjects
	public GameObject ImageTarget;
	public GameObject Board;
	public GameObject screenText;


	// Player Mode
	public bool gameTurn;
	public bool p2;
	public GUIStyle turn;
	public GUIStyle redP2;
	public GUIStyle redP1;
	public GUIStyle wrap; //used to wrap text
	
	//Screen dimensions
	private int height;
	private int width;

	// Debug
	public GUIText sysinfo;

	public Transform debugSphere;
	public Color defaultColor = Color.white;

	// Focus	
	public int focusDistance;
	private Ray focusRay;
	private GameObject focusedObject;
	private Vector3 focusPoint;
	
	// Selection
	private GameObject selectedObject;

	//bools handling menu
	private bool targetFound;

	//initial position of camera
	Vector3 pos;    

	//initial scale of board
	Vector3 initScale;
	//textmesh for screen text
	TextMesh t; 
	//initial position of board
	Vector2 initialBoardPosition;
	
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;

		// DESKTOP DEBUG
		height = Screen.height;
		width = Screen.width;

		// PHONE DEBUG
		//height = Screen.width;
		//width = Screen.height;

		focusedObject = null;
		selectedObject = null;
		gameTurn = false;
		p2 = false;
		targetFound = false;
		highMode = PREGAME_MODE;
		lowMode = INIT_MODE;
		//turn = new GUIStyle ();
		//turn.normal.textColor = Color.black;
		//jose test
		//turn.fontSize = 25;
		game = Board.GetComponent<GameScript> ();

		//initial camera position
		pos = transform.position;

		//get initial local scale
		initScale = Board.transform.localScale;

		t = (TextMesh)screenText.GetComponent(typeof(TextMesh));
		t.text = "Find Image Target";
		initialBoardPosition = Board.transform.position;

	}


	// Update is called once per frame
	void Update () {
		if(!targetFound) {
			targetFound = ImageTarget.GetComponent<ImageTargetTracking> ().targetFound;
			if(targetFound){
				lowMode = START_MODE;
			}
		}

		// Point Ray through center of camera viewport 
		focusRay = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		focusPoint = transform.position + focusRay.direction * focusDistance;

		//how much the position of camera changed
		Vector3 howmuch = pos - transform.position;
		pos = transform.position;


		// Debug
		// Draw ray during pause mode with sphere at the end
		Debug.DrawRay (transform.position, focusRay.direction * focusDistance);
		debugSphere = GameObject.FindGameObjectWithTag("debug").transform;
		debugSphere.position = focusPoint;
		if (highMode == PREGAME_MODE){
			// DON'T DO ANYTHING!

		} else if (highMode == PLAY_MODE) {
			// PLAY MODE

			if(lowMode == SEL_MODE){
				// Setup variable to hold raycast information
				RaycastHit hit;

				if (Physics.Raycast (focusRay, out hit, focusDistance)) {
					Debug.Log("Hit tag: " + hit.transform.tag);
					if(hit.transform.tag == "piece"){
						// Highlight Piece that's focused on
						Debug.Log("hit piece");
						if(hit.transform.GetComponent<GamePieceScript>().player == Board.GetComponent<GameScript>().turn){
							hit.transform.GetComponent<GamePieceScript>().focus();
							focusedObject = hit.transform.gameObject;
						}
					} else {
						Debug.Log("Searching");
						if(focusedObject != null){
							focusedObject.GetComponent<GamePieceScript>().resetColor();
							focusedObject = null;
						}
					}
				}else if(focusedObject!=null){

						focusedObject.GetComponent<GamePieceScript>().resetColor();
						focusedObject = null;
				}

			} else if(lowMode == MOVE_MODE){
				Debug.Log("MOVE_MODE_UPDATE");
				selectedObject.transform.position = focusPoint;
				if(selectedObject.transform.position.y < 11.0f)
					selectedObject.transform.position = new Vector3(selectedObject.transform.position.x,11.0f,selectedObject.transform.position.z);
				if(selectedObject.transform.position.y > 15.0f)
					selectedObject.transform.position = new Vector3(selectedObject.transform.position.x,15.0f,selectedObject.transform.position.z);
			} else if(lowMode == PASS_MODE){
				rayToCamera = Board.transform.position - transform.position;
				Vector2 flattenedRay = new Vector2(rayToCamera.x, rayToCamera.z);

				radialAngle = Vector2.Angle(referenceRay, flattenedRay);
				Vector3 cross = Vector3.Cross (referenceRay, flattenedRay);

				if (cross.z > 0)
					radialAngle = 360 - radialAngle;

				if(radialAngle > 90.0 && radialAngle < 270.0){
					Debug.Log("In Player2 position");

				} else {
					Debug.Log("In Player1 position");
				}
			}
		} else if(highMode == EXPLORE_MODE){
			// EXPLORE MODE
			if(lowMode==BASE_EX_MODE){
			
			}else if(lowMode == SCALE_MODE){
				Vector3 myVector =  new Vector3(howmuch.y*0.001f,howmuch.y*0.001f,howmuch.y*0.001f);
				if(myVector.x +Board.transform.localScale.x <initScale.x)
					Board.transform.localScale = initScale;
				else
					Board.transform.localScale += myVector;
			}
		}

		//menu = true;
		
	}
	
	void OnGUI () {

		if (highMode == PREGAME_MODE){
			// PREGAME MODE
			if(lowMode == INIT_MODE){
			// INITIALIZE MODE
			// Prompt user to find image target
				//GUI.Label((new Rect (((width/2)-50),0, 150, 50)),"Find the Image Target", turn);
			}else if(lowMode == START_MODE){
			// START MODE
			// Prompt user to start game
				t.text = "LETS PLAY!";
				if (GUI.Button (new Rect ((width/2-75), (height/2-50), 150, 100), "START")) {
					lowMode = INSTR_MODE;


					//text.GetComponent(TextMesh)="blah";
				}
			}else if(lowMode == INSTR_MODE){
			//
				turn.normal.textColor = Color.magenta;
				wrap.normal.textColor = Color.magenta;
				//GUI.Label((new Rect (((width/2)-50),0, 150, 50)),"INSTRUCTIONS", turn);
				t.text = "Instructions\n\nBlack moves first. Players alternate turns\nOnly move diagonally on dark squres.\nA player wins when opponent can't make any \nmoves or is left without pieces.";
				wrap.wordWrap = true;
				//GUI.Label((new Rect (((width/5)-50),100, (width-200), (height-200))),"Black moves first. Players then alternate moves.Moves are allowed only on the dark squares, so pieces always move diagonally. Single pieces are always limited to forward moves (toward the opponent). A piece making a non-capturing move (not involving a jump) may move only one square. A piece making a capturing move (a jump) leaps over one of the opponent's pieces, landing in a straight diagonal line on the other side. Only one piece may be captured in a single jump; however, multiple jumps are allowed on a single turn.When a piece is captured, it is removed from the board.If a player is able to make a capture, there is no option -- the jump must be made. If more than one capture is available, the player is free to choose whichever he or she prefers.When a piece reaches the furthest row from the player who controls that piece, it is crowned and becomes a king. One of the pieces which had been captured is placed on top of the king so that it is twice as high as a single piece. Kings are limited to moving diagonally, but may move both forward and backward. (Remember that single pieces, i.e. non-kings, are always limited to forward moves.)Kings may combine jumps in several directions -- forward and backward -- on the same turn. Single pieces may shift direction diagonally during a multiple capture turn, but must always jump forward (toward the opponent). A player wins the game when the opponent cannot make a move. In most cases, this is because all of the opponent's pieces have been captured, but it could also be because all of his pieces are blocked in.",wrap);
				if (GUI.Button (new Rect ((width-150), (height-100), 150, 100), "Continue")) {
					highMode = PLAY_MODE;
					lowMode = SEL_MODE;
					gameTurn = true;
					game.makeBoard ();
				}
			}

		} else if (highMode == PLAY_MODE) {
			// PLAY MODE
			
			if(lowMode == SEL_MODE){
				// SELECTION MODE
				///////////////
				// Selection //
				///////////////
				/// These two buttons should be on top of each other;
				/// Pick Up
				/// Place
				if (GUI.Button (new Rect ((width-100), (height-100), 100, 100), "Select Piece")) {
					if(focusedObject!=null){
						//check if piece is valid move
						GameScript.CheckersMove[] valMoves = Board.GetComponent<GameScript>().getLegalMoves(focusedObject);
						if(valMoves!=null){
							selectedObject = focusedObject;
							lowMode = MOVE_MODE;
							selectedObject.GetComponent<GamePieceScript>().select();
							game.showMoves(selectedObject);
							focusedObject = null;
						}else{
							//dont allow move
						}
					}
				}
				/////////////////
				// Translation //
				/////////////////
				if (GUI.Button (new Rect ((width-200), (height-100), 100, 100), "Explore")) {
					highMode = EXPLORE_MODE;

				}
				
				/////////////////
				//   NewGame   //
				/////////////////
				if (GUI.Button (new Rect ((width-400), (height-100), 100, 100), "Resign")) {
					//translateMode = true;
				}
				//
				if (GUI.Button (new Rect ((width-300), (height-100), 100, 100), "Show Moves")) {

				}

			}else if(lowMode == MOVE_MODE){
				// MOVEMENT MODE

				selectedObject.transform.position = focusPoint;
				if (GUI.Button (new Rect ((width-100),(height-100), 100, 100), "Place Piece")) {
					selectedObject.GetComponent<GamePieceScript>().resetColor();
					selectedObject = null;
					game.stopShowingMoves();
					//check if there is another jump to force player to jump
					lowMode = PASS_MODE;
					if(game.turn ==1)
						game.turn=2;
					else if(game.turn ==2)
						game.turn =1;
				 }

				
				/////////////////
				//   NewGame   //
				/////////////////
				if (GUI.RepeatButton (new Rect ((width-200), (height-100), 100, 100), "Resign")) {
					//translateMode = true;
				}
			}else if(lowMode == PASS_MODE){
				if (GUI.Button (new Rect ((width/2-75),(height/2-50), 150, 100), "IM HERE")) {
					lowMode = SEL_MODE;


				}
			}
		} else if(highMode == EXPLORE_MODE){
			// EXPLORE MODE
			// Translation via Joystick
			joystick = (Transform)Instantiate(joystick, joystick.transform.position, Quaternion.identity);

			/////////////////
			// Translation //
			/////////////////
			if (GUI.RepeatButton (new Rect ((width-200), (height-100), 100, 100), "SCALE")) {
				lowMode = SCALE_MODE;
			}
			else{
				lowMode = BASE_EX_MODE;
			}
			if (GUI.Button (new Rect ((width-100), (height-100), 100, 100), "PLAY")) {
				highMode = PLAY_MODE;
				lowMode = SEL_MODE;
				Board.transform.localScale = initScale;
			}
			
			/////////////////
			//   NewGame   //
			/////////////////
			if (GUI.RepeatButton (new Rect ((width-300), (height-100), 100, 100), "Resign")) {
				//translateMode = true;
			}
			if(lowMode == SCALE_MODE){
				// SCALE MODE
			}
		}
		if (gameTurn) {
						//player message
						if (game.turn == 1) {

								turn.normal.textColor = Color.magenta;
								//GUI.Label((new Rect (((width/2)-50),0, 150, 50)),"YOUR TURN BLACK", turn);
								t.text = "Your Turn Black";
								//p2 = false;
						}
						if (game.turn == 2) {
								turn.normal.textColor = Color.magenta;
								//GUI.Label((new Rect (((width/2)-50),0, 150, 50)),"YOUR TURN RED", turn);
								t.text = "Your Turn Red";
								//p1 = false;
						}
				}
		int number = 8;
		redP2.normal.textColor = Color.red;
		redP1.normal.textColor = Color.white;
		GUI.Label((new Rect ((width-100), 0, 100, 100)),"Player 1", redP1);
		GUI.Label((new Rect ((width-100), 50, 100, 100)),"Count: "+ number, redP1);
		GUI.Label((new Rect ((width-100), 100, 100, 100)),"Score: "+ number, redP1);
		GUI.Label((new Rect ((width-50), 0, 100, 100)),"Player 2", redP2);
		GUI.Label((new Rect ((width-50), 50, 100, 100)),"Count: "+ number, redP2);
		GUI.Label((new Rect ((width-50), 100, 100, 100)),"Score: "+ number, redP2);
	}
	
	void focusOnObject (Transform obj){
		if (focusedObject != null) {
			focusedObject.GetComponent<GamePieceScript>().resetColor();
		}
		Debug.Log("I'm looking at " + obj.name);
		GamePieceScript piece = obj.GetComponent<GamePieceScript> ();
		piece.focus ();
		focusedObject = obj.gameObject;
		sysinfo.text = "Focused on object: " + focusedObject.transform.name;
	}
	
	void defocusObject (){
		Debug.Log ("DEFOCUS: " + focusedObject.transform.name);
		focusedObject.GetComponent<GamePieceScript> ().resetColor ();
		focusedObject = null; 
		sysinfo.text = "Approach a game piece to select it.";
	}
	
	void selectObject (Transform obj){
		// Change object color to red
		Debug.Log ("Selecting Object: " + obj.transform.name);
		obj.gameObject.GetComponent<GamePieceScript> ().select ();
		selectedObject = obj.gameObject;
		focusedObject = null;

		sysinfo.text = "Selected Object: " + selectedObject.transform.name;
	}
	
	void deselectObject (){
		selectedObject.GetComponent<GamePieceScript> ().resetColor ();
		selectedObject = null;
		sysinfo.text = "Approach a game piece to select it.";
	}

}
