using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class clickbutton : MonoBehaviour {
	public TextAsset ta;
	public TextAsset ta2;
	DataManager dm;

	public List<string> objects;
	// Use this for initialization
	void Start () {


		dm = DataManager.Instance;

		dm.SetData(ta.text);
		dm.Load( "stagelist" );

		Debug.Log ("stagelist="+ dm.Count ("stagelist") +" / stage="+ dm.Count ("stage") +" / hoge="+ dm.Count ("hoge")  +" / stash="+ dm.Count ("_STASH_") );

		string str;
		str = dm.GetString ( new List<string>{"stagelist","1"} );
		Debug.Log ("str="+ str );
		/*
		str = dm.GetString ( new List<string>{"stage","unlocked_stage"} );
		Debug.Log ("str="+ str );

		dm.SearchAndUpdate( new List<string>{"stagelist","1"}, "hoge");

		str = dm.GetString ( new List<string>{"stagelist","1"} );
		Debug.Log ("str="+ str );
*/
		dm.SetData(ta2.text);
		/*
		str = dm.GetString ( new List<string>{"stagelist","1"} );
		Debug.Log ("str="+ str );
		str = dm.GetString ( new List<string>{"stage","unlocked_stage"} );
		Debug.Log ("str="+ str );
*/
		string hoge = dm.GetString ( new List<string>{"stagelist","1"} );
		hoge += "hoge";
		dm.SearchAndUpdate ( new List<string>{"stagelist","1"}, hoge );

		dm.Save( "stagelist" );

		PlayerPrefs.DeleteAll();
	}
}
