﻿using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
	void OnSceneGUI()
	{
		FieldOfView FOW = (FieldOfView)target;
		Handles.color = Color.white;
		Vector3 viewAngleA = FOW.DirFromAngle(-FOW.viewAngle / 2 + 90, false);
		Vector3 viewAngleB = FOW.DirFromAngle(FOW.viewAngle / 2 + 90, false);
        Handles.DrawWireArc(FOW.transform.position, Vector3.forward, viewAngleB, FOW.viewAngle, FOW.viewRadius);

		Handles.DrawLine(FOW.transform.position, FOW.transform.position + viewAngleA * FOW.viewRadius);
		Handles.DrawLine(FOW.transform.position, FOW.transform.position + viewAngleB * FOW.viewRadius);

		Handles.color = Color.red;
		if(FOW.target != null)
		{
			if(!Physics2D.Raycast(FOW.transform.position, (FOW.target.position - FOW.transform.position).normalized, Vector2.Distance((Vector2)FOW.target.transform.position, (Vector2)FOW.transform.position), FOW.obstacleMask))
				Handles.DrawLine(FOW.transform.position, FOW.target.position);
		}
	}
}
