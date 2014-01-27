using UnityEngine;
using System.Collections;

public class Config : MonoBehaviour {

	public enum SETTING_MODE : int {
		develop,
		staging,
		release
	};

	void OnGUI()
	{
		if ( !Debug.isDebugBuild )
		{
			return;
		}

		GUI.color = Color.black;
		GUI.Label ( new Rect( Screen.width-50+1, 0+1, 50, 30 ) , Mode.ToString () );
		GUI.color = Color.white;
		GUI.Label ( new Rect( Screen.width-50, 0, 50, 30 ) , Mode.ToString () );
	}

	public SETTING_MODE Mode;



}
