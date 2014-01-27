using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	public static bool keyLock = false;
	public bool lateInput = true;
	protected static bool skipLateInput;
	
	void Update () {
		if( keyLock )
		{
			return;
		}
		if( !lateInput )
		{
			input();
		}
	}
	
	void LateUpdate () {
		
		if( keyLock )
		{
			return;
		}
		if( lateInput && !skipLateInput )
		{
			input();
		}
	}
	
	protected virtual void input(){}
	
	protected void skipInput()
	{
		if( !lateInput )
		{
			skipLateInput = true;
		}
	}
}
