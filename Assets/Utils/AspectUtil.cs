using UnityEngine;
using System.Collections;

public class AspectUtil : MonoBehaviour {

	public Vector2 ModelAspect = new Vector2( 2f, 3f );

	void Start ()
	{
		float ModelAspectHeight = ModelAspect.y / ModelAspect.x;
		float ScreenAspectHeight = (float)Screen.height / (float)Screen.width;

		if( ModelAspectHeight < ScreenAspectHeight )
		{
Debug.Log ("Height is Longer");
			float aspect = 1f / ( ScreenAspectHeight / ModelAspectHeight );
			transform.localScale = new Vector3(aspect,aspect,aspect);
		}
		else
		{
Debug.Log ("Width is Longer");
			transform.localScale = new Vector3(1f,1f,1f);
		}
	}
}

