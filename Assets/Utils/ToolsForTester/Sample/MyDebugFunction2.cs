using UnityEngine;
using System.Collections;

public class MyDebugFunction2 : MonoBehaviour {

	void Start()
	{
	}
	void DeleteAllSaveData () {
		PlayerPrefs.DeleteAll();
	}
}
