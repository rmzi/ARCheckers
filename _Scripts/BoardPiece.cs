using System;
using UnityEngine;

public class BoardPiece{
	public int i;
	public int j;
	public Transform objectTransform;
	public Color defaultColor;

	public BoardPiece(int iVal, int jVal, Transform obj, Color clr){
		i = iVal;
		j = jVal;
		objectTransform = obj;
		defaultColor = clr;
	}

	public Transform getTransform(){
		return objectTransform;
	}
}
