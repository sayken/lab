using UnityEngine;
using System.Collections;

public class MyDebugFunction : MonoBehaviour {

	void Start()
	{
		Debug.Log ("start sitayo");
	}
	void ResetAllFlags ()
	{
		Debug.Log ("ResetAllFlags dayo");
		GameObject.Find ("ToolsForTester").GetComponent<ToolsForTester>().SetInfo("username","yamada");
	}
	
	void IncrementStage ()
	{
		Debug.Log ("IncrementStage dayo");
		GameObject.Find ("ToolsForTester").GetComponent<ToolsForTester>().SetInfo("username","sato");
	}
}
