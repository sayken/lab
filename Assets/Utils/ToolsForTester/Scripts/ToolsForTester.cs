using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

//
// For Debug Function
// 2014 .1.23 Gen Nishijima
//
public class ToolsForTester : MonoBehaviour {

	public Vector2 ButtonPosition = new Vector2(0f, 0f);

	List<string> UnityReservedNames = new List<string>(){
		"Awake",
		"Start",
		"Update",
		"LastUpdate",
		"OnGUI"
	};

	List<string> FunctionNames;
	Dictionary<string, string> InfoTexts;

	List<Component> Components;

	int Page = 0;
	bool View = false;
	bool ViewInfoTexts = true;

	int ButtonWidth = 200;
	int ButtonHeight = 30;
	int Space = 10;
	
	void Awake()
	{
	// Destroy at Release Build
		if ( !Debug.isDebugBuild )
		{
			Destroy (gameObject);
		}

	// Set button size
		Space = Screen.width / 100 * 5;
		ButtonWidth = (Screen.width-Space*3) / 2;
		ButtonHeight = (Screen.height-Space*12) / 10;
		// Debug.Log("Screen.width="+Screen.width+" / Screen.height="+Screen.height +" / Space="+Space+" / ButtonWidth="+ButtonWidth+" / ButtonHeight="+ButtonHeight);

	// make FunctionNames List
		FunctionNames = new List<string>();

		Components = new List<Component>();

		// get debug function class from this gameobject
		Component[] comps = GetComponents(typeof(Component));
		foreach( Component c in comps )
		{
			if ( c.GetType () == typeof(Transform) || c.GetType() == typeof(ToolsForTester) )
			{
				continue;
			}

			Components.Add (c);
		}

		SetFunctionNames();
		// init InfoTexts
		InfoTexts = new Dictionary<string, string>();
	}

	void SetFunctionNames()
	{
		if ( Page >= Components.Count )
		{
			Page = 0;
		}

		FunctionNames = new List<string>();

		Type t = Type.GetType( Components[Page].GetType ().ToString () );
		MethodInfo[] methods = t.GetMethods( BindingFlags.Public |
		                                    BindingFlags.NonPublic |
		                                    BindingFlags.Instance |
		                                    BindingFlags.Static |
		                                    BindingFlags.DeclaredOnly );
		foreach (MethodInfo m in methods)
		{
			if ( UnityReservedNames.Contains( m.Name ) )
			{
				continue;
			}
			FunctionNames.Add (m.Name);
		}
	}

	public void SetInfo( string key, string val )
	{
		if ( !InfoTexts.ContainsKey(key) )
		{
			InfoTexts.Add (key, val);
			return;
		}
		InfoTexts[key] = val;
	}

	void OnGUI ()
	{
		if ( !View )
		{
		//++ hide debug ++//
			if ( GUI.Button ( new Rect(ButtonPosition.x,ButtonPosition.y, ButtonWidth/2, ButtonHeight), "Debug" ) )
			{
				View = true;
			}
		}
		else
		{
		//++ show debug ++//
			// background
			GUI.Box ( new Rect( 0, 0, Screen.width, Screen.height), "");

			// close button
			if ( GUI.Button ( new Rect(0,0, ButtonWidth/2, ButtonHeight), "Exit" ) )
			{
				View = false;
			}
			// switch button
			if ( GUI.Button ( new Rect( ButtonWidth/2, 0, ButtonWidth, ButtonHeight ), "["+ Components[Page].GetType ().ToString () +"] "+ (Page+1) +"/"+ Components.Count ) )
			{
				Page = (Page+1)%Components.Count;
				SetFunctionNames();
			}

			//++ debug function ++//

			// infotext on<>off
			if ( InfoTexts.Count > 0 )
			{
				if ( GUI.Button ( new Rect( ButtonWidth/2*3, 0, ButtonWidth/2, ButtonHeight ), ViewInfoTexts?"hide info":"show info" ) )
				{
					ViewInfoTexts = !ViewInfoTexts;
				}
			}

			// functions
			for ( int i=0 ; i<FunctionNames.Count ; i++ )
			{
				int x = (Space + i/10*(ButtonWidth+Space));
				int y = (Space + i%10*(ButtonHeight+Space)) + (Space + ButtonHeight);
				
				if ( GUI.Button ( new Rect(x,y, ButtonWidth, ButtonHeight), FunctionNames[i] ) )
				{
					gameObject.SendMessage (FunctionNames[i]);
				}
			}
			
			// show infotexts
			if ( ViewInfoTexts )
			{
				int i=0;
				foreach ( string value in InfoTexts.Values )
				{
					GUI.Label ( new Rect( Screen.width-140, (ButtonHeight+5) + i*(20+5), 140, 20 ), value );
					i++;
				}
			}
		}
	}
}
