using UnityEngine;
using System.Collections;

public class ARCameraController : MonoBehaviour {
	//GameObjects
	public GameObject ImageTarget;
	public GameObject Board;
	
	// Debug
	public GUIText sysinfo;

	//public Transform debugSphere;
	public Color defaultColor = Color.white;
	

	// Focus	
	public int focusDistance;
	private Ray focusRay;
	private GameObject focusedObject;
	private Color focusColor = Color.magenta;
	private Vector3 focusPoint;
	
	// Selection
	private GameObject selectedObject;
	private Color selectedColor = Color.red;
	
	// Booleans for Modes
	private bool translateMode;
	private bool selectInsteadOfFocus;
	private bool deselectMode;
	//bools handeling menu
	private bool showMenu;
	private bool menu ;
	private bool showStart;

	// Use this for initialization
	void Start () {
		showStart = true;
		focusedObject = null;
		selectedObject = null;
		sysinfo.text = "Approach a game piece to select it.";
	}


	// Update is called once per frame
	void Update () {


		// Setup variable to hold raycast information
		RaycastHit hit;
		
		// Point Ray through center of camera viewport 
		focusRay = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		focusPoint = transform.position + focusRay.direction * focusDistance;
		
		// Debug
		// Draw ray during pause mode with sphere at the end
		Debug.DrawRay (transform.position, focusRay.direction * focusDistance);
		//debugSphere = GameObject.FindGameObjectWithTag ("SelectionTarget").transform;
		//debugSphere.position = transform.position + focusRay.direction * focusDistance;
		
		if (translateMode) {
			if(selectedObject != null){
				selectedObject.transform.position = focusPoint;
				translateMode = false;
			}
		}
		
		// Raycast for focusing
		if (Physics.Raycast (focusRay, out hit, focusDistance)) {
			// Selection Mode
			if(selectInsteadOfFocus){
				selectObject(hit.transform);
				selectInsteadOfFocus = false;
			}else {
				// Focus Mode
				// If no selected object + no focused object
				if(selectedObject == null){
					if(focusedObject == null){
						focusOnObject (hit.transform);
					}
				}
			}
		} else {
			if (deselectMode){
				deselectObject();
				deselectMode = false;
			}
			
			// If not focused on anything, remove focus color from previously focused object
			if (focusedObject != null){
				defocusObject();
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
					GameScript b = Board.GetComponent<GameScript> ();
					b.makeBoard ();
				}
			}
		}
		if (showMenu) {
			///////////////
			// Selection //
			///////////////
			if (GUI.Button (new Rect (590, 290, 150, 100), "Select Object")) {
				selectInsteadOfFocus = true;
			}
	
			// Deselection
			if (GUI.Button (new Rect (440, 290, 150, 100), "Deselect Object")) {
				deselectMode = true;
			}
			/////////////////
    		 // Translation //
			/////////////////
			if (GUI.RepeatButton (new Rect (340, 290, 100, 100), "Translate")) {
				translateMode = true;
			}
			/////////////////
			//   NewGame   //
			/////////////////
			if (GUI.RepeatButton (new Rect (240, 290, 100, 100), "New Game")) {
				//translateMode = true;
			}
			int number = 8;
			GUI.Label((new Rect (0, 0, 100, 100)),"Player 1");
			GUI.Label((new Rect (0, 50, 100, 100)),"Count: "+ number);
			GUI.Label((new Rect (0, 100, 100, 100)),"Score: "+ number);
			GUI.Label((new Rect (600, 0, 100, 100)),"Player 2");
			GUI.Label((new Rect (600, 50, 100, 100)),"Count: "+ number);
			GUI.Label((new Rect (600, 100, 100, 100)),"Score: "+ number);

		}
	}
	
	void focusOnObject (Transform obj){
		print("I'm looking at " + obj.name);
		obj.transform.renderer.material.color = focusColor;
		focusedObject = obj.gameObject;
		sysinfo.text = "Focused on object: " + focusedObject.transform.name;
	}
	
	void defocusObject (){
		Debug.Log ("DEFOCUS: " + focusedObject.transform.name);
		focusedObject.transform.renderer.material.color = defaultColor;
		focusedObject = null; 
		sysinfo.text = "Approach a game piece to select it.";
	}
	
	void selectObject (Transform obj){
		// Change object color to red
		Debug.Log ("Selecting Object: " + obj.transform.name);
		obj.transform.renderer.material.color = selectedColor;
		selectedObject = obj.gameObject;
		focusedObject = null;

		sysinfo.text = "Selected Object: " + selectedObject.transform.name;
	}
	
	void deselectObject (){
		selectedObject.transform.renderer.material.color = defaultColor;
		selectedObject = null;
		sysinfo.text = "Approach a game piece to select it.";
	}
}













