using UnityEngine;
using System.Collections;



public class ARCameraController : MonoBehaviour {
	// Modes
	// Modes will determine which code to run to increase efficiency of calculations
	// High level modes: Pregame, Play & Explore
	private const int PREGAME_MODE = 0;
	private const int PLAY_MODE = 1;
	private const int EXPLORE_MODE = 2;

	// GUI Low level modes: Init, Start, Instructions
	private const int INIT_MODE = 0;
	private const int START_MODE = 1;
	private const int INSTR_MODE = 2;

	// UPDATE Low level modes: Selection, Movement, Scaling Mode
	private const int SEL_MODE = 3;
	private const int MOVE_MODE = 4;
	private const int SCALE_MODE = 5; 


	// Low level modes are nested in high level modes
	// All modes are ENUM and their values are listed below
	public int highMode = "";
	public int lowMode = "";
	
	//GameObjects
	public GameObject ImageTarget;
	public GameObject Board;

	// Player Mode
	public bool p1;
	public bool p2;
	public GUIStyle turn;

	//Screen dimensions
	private int height = Screen.height;
	private int width = Screen.width;

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
	private bool showMenu;
	private bool menu;
	private bool showStart;

	// Use this for initialization
	void Start () {
		showStart = true;
		focusedObject = null;
		selectedObject = null;
		sysinfo.text = "Approach a game piece to select it.";
		p1 = false;
		p2 = false;
	}


	// Update is called once per frame
	void Update () {

		// Point Ray through center of camera viewport 
		focusRay = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		focusPoint = transform.position + focusRay.direction * focusDistance;

		// Debug
		// Draw ray during pause mode with sphere at the end
		Debug.DrawRay (transform.position, focusRay.direction * focusDistance);
		debugSphere = GameObject.FindGameObjectWithTag ("debug").transform;
		debugSphere.position = focusPoint;
		if (highMode == PREGAME_MODE{
			// DON'T DO ANYTHING!
		} else if (highMode == PLAY_MODE) {
			// PLAY MODE

			if(lowMode == SEL_MODE){
				// Setup variable to hold raycast information
				RaycastHit hit;

				if (Physics.Raycast (focusRay, out hit, focusDistance)) {
					if(hit.transform.tag == "piece"){
						// Highlight Piece that's focused on
						GamePieceScript pieceController = hit.transform.GetComponent<GamePieceScript>;
						pieceController.focus ();
						focusedObject = hit.transform;
					} else {
						if(focusedObject != null){
							GamePieceScript pieceController = focusedObject.GetComponent<GamePieceScript>;
							pieceController.resetColor();
							focusedObject = null;
						}
					}
				}

			} else if(lowMode == MOVE_MODE){
				selectedObject.transform.position = focusPoint;
			}


		} else if(highMode == EXPLORE_MODE){
			// EXPLORE MODE

			// Translation via Joystick

			if(lowMode == SCALE_MODE){

			}
		}
		
		//if the target is detected before game starts it will show menu
		DefaultTrackableEventHandler s = ImageTarget.GetComponent<DefaultTrackableEventHandler>();
		menu = s.startGame;
		
	}
	
	void OnGUI () {
		///////////////
		///Start Game //
		///////////////
		if (showStart) {
			if(menu){
				if (GUI.Button (new Rect (300, 110, 150, 100), "START")) {
					showMenu = true;
					showStart=false;
					p1 = true;
					GameScript b = Board.GetComponent<GameScript> ();
					b.makeBoard ();
				}
			}
		}
		//player message
		if (p1) {

			turn.normal.textColor = Color.magenta;
			GUI.Label((new Rect (((width/2)-50),0, 150, 50)),"YOUR TURN PLAYER 1", turn);
			p2 = false;
		}
		if (p2) {
			turn.normal.textColor = Color.magenta;
			GUI.Label((new Rect (((width/2)-50),0, 150, 50)),"YOUR TURN PLAYER 2", turn);
			p1 = false;
		}
		if (showMenu) {
			///////////////
			// Selection //
			///////////////
			
				/// These two buttons should be on top of each other;

			/// Pick Up
			/// Place
			if (GUI.Button (new Rect (350, (height-100), 150, 100), "Select Object")) {
				selectInsteadOfFocus = true;
			}
	
			// Deselection
			if (GUI.Button (new Rect (200,(height-100), 150, 100), "Deselect Object")) {
				deselectMode = true;
			}
			/////////////////
    		 // Translation //
			/////////////////
			if (GUI.RepeatButton (new Rect (100, (height-100), 100, 100), "Translate")) {
				translateMode = true;
			}
			/////////////////
			//   NewGame   //
			/////////////////
			if (GUI.RepeatButton (new Rect (0, (height-100), 100, 100), "New Game")) {
				//translateMode = true;
			}
			int number = 8;
			GUI.Label((new Rect (0, 0, 100, 100)),"Player 1");
			GUI.Label((new Rect (0, 50, 100, 100)),"Count: "+ number);
			GUI.Label((new Rect (0, 100, 100, 100)),"Score: "+ number);
			GUI.Label((new Rect ((width-50), 0, 100, 100)),"Player 2");
			GUI.Label((new Rect ((width-50), 50, 100, 100)),"Count: "+ number);
			GUI.Label((new Rect ((width-50), 100, 100, 100)),"Score: "+ number);

		}
		
		/*
		if (highMode == PREGAME_MODE){
			// PREGAME MODE
			if(lowMode == INIT_MODE){
			// INITIALIZE MODE
			// Prompt user to find image target

			}else if(lowMode == START_MODE){
			// START MODE
			// Prompt user to start game

			}else if(lowMode == INSTR_MODE){
			//

			}

		} else if (highMode == PLAY_MODE) {
			// PLAY MODE
			
			if(lowMode == SEL_MODE){
				// SELECTION MODE

			}
				
			} else if(lowMode == MOVE_MODE){
				// MOVEMENT MODE
				selectedObject.transform.position = focusPoint;
			}
			
			
		} else if(highMode == EXPLORE_MODE){
				// EXPLORE MODE
				
				// Translation via Joystick
				
				if(lowMode == SCALE_MODE){
					// SCALE MODE
					
				}
		}*/
	}
	
	void focusOnObject (Transform obj){
		if (focusedObject != null) {
			focusedObject.GetComponent<GamePieceScript>().resetColor();
		}
		print("I'm looking at " + obj.name);
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
