using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetFromDataManager : MonoBehaviour {

	public List<string> Keys;
	public string Prefix = "";
	public string Suffix = "";

	void Start()
	{
		SetTo ();
	}

	void SetTo()
	{
		string str = Prefix + DataManager.Instance.GetString(Keys) + Suffix;

		if ( GetComponent<TextMesh>() != null )
		{
			GetComponent<TextMesh>().text = str;
		}
//		else if ( GetComponent<UILabel>() != null )
//		{
//			GetComponent<UILabel>().text = str;
//		}
//		else if ( GetComponent<UISprite>() != null )
//		{
//			GetComponent<UILabel>().spriteName = str;
//		}
//		else if ( GetComponent<UITexture>() != null )
//		{
//			
//		}
//		else if ( GetComponent<SysFontLabel>() != null )
//		{
//			GetComponent<SysFontLabel>().Text = str;
//		}
		else
		{
			Debug.LogWarning("Cannot Found Match DataType");
		}
	}
}
