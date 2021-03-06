using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flick : InputManager {

	
	private Touch touch;
	
	private float touchedTime;
	private Vector2 touchBegan;
	private Vector3 beganPos;
	
	public float flicksTime = 0.3f;
	public float touchActionTime = 0.2f;
	public float flicksDistance = 0.2f;
	
	public float adjust = 80.0f;
	
	public float scrollRate = 2000f;		// フリックしたときの速さ（大きくすると遅くなる）
	public float round = 0.005f;		// フリック速度が遅くなった時の丸めこみ数値。小さいほど最後までフリック。
	public float flickSpeed = 90.0f;	// フリックの速度。小さいほど速い。
	
	public int pageXID = 0;
	public int pageYID = 0;
	public int pageXMax = 10;
	public int pageYMax = 0;
	
	public bool XFlick = true;
	public bool YFlick = false;
	
	public bool flicking;
	
	private Vector2 scrollSpeed;
	private int scrollDirectionX;
//	private int scrollDirectionY;
	private Vector2 pageCenter;
	
	public Vector2 basePosition = new Vector2( 0f, 0f );
	
	List<float> panelXList;
	List<float> panelYList;
	
	bool startEffect = true;
	public bool keyInput = false;
	
	Rect zone;
	
	void Start() {
		lateInput = false;
		
		scrollDirectionX = 0;
//		scrollDirectionY = 0;
		flicking = false;
		scrollSpeed = Vector2.zero;
		
		
		panelXList = gameObject.GetComponent<FlickPanelManager>().panelXList;
		panelYList = gameObject.GetComponent<FlickPanelManager>().panelYList;
		if( panelYList.Count==0 )
		{
			panelYList.Add(0f);
		}
		
		if( !XFlick )
		{
			basePosition.x = 0f;
		}
		if( !YFlick )
		{
			basePosition.y = 0f;
		}
		
		pageCenter = new Vector2( -panelXList[pageXID], -panelXList[pageYID] );
/*		
		Vector3 p = gameObject.transform.localPosition;
		p.x = pageCenter.x;
		p.y = pageCenter.y;
		gameObject.transform.localPosition = p;
*/		
		zone = GameObject.Find("FlickZone").guiTexture.pixelInset;
	}
	
	void OnEnable()
	{
		startEffect = true;
		Vector3 p = gameObject.transform.localPosition;
		p.x = 0f;
		p.y = 0f;
		gameObject.transform.localPosition = p;
	}
	
	bool touchedAtBegan = false;
	
	protected override void input () {
		
		Vector3 p = gameObject.transform.localPosition;
		
		int touchStatus = 0;
		
		if( Input.touchCount>0 )
		{
			Touch touch = Input.GetTouch(0);
			
			
			if( touch.phase==TouchPhase.Began )
			{
				startEffect = false;
				if( !zone.Contains( touch.position ) )
				{
					return;
				}
				if( flicking )
				{
					scrollSpeed = Vector2.zero;
				}
				touchedTime = Time.time;
				touchBegan = touch.position;
				beganPos = p;
				touchStatus = 1;
				touchedAtBegan = true;
			}
			else if( touch.phase==TouchPhase.Moved && touchedAtBegan )
			{
				Vector2 pos = touch.position;
				
				p.x = beganPos.x + ((float)pos.x-(float)touchBegan.x)/(float)Screen.width;
//				buttonManager.ignoreInput = true;
				touchStatus = 2;
			}
			else if( touch.phase==TouchPhase.Stationary && touchedAtBegan )
			{
				touchStatus = 3;
			}
			else if( touch.phase==TouchPhase.Ended && touchedAtBegan )
			{
				touchedAtBegan = false;
				float diffTime = Time.time-touchedTime;
				
				if( diffTime < flicksTime )
				{
					Vector2 diff = new Vector2(
					                     Mathf.Abs(touchBegan.x)-Mathf.Abs(touch.position.x), 
					                     Mathf.Abs(touchBegan.y)-Mathf.Abs(touch.position.y));
					if( Mathf.Abs(diff.x)>flicksDistance && XFlick)
					{
						if( diff.x!=0f )
						{
						//	Debug.Log("flick is exec!! ["+diff.x+"]");
							scrollSpeed.x = -( diff.x/scrollRate );
							flicking = true;
							scrollDirectionX = diff.x>0 ? 1:0;
//							buttonManager.ignoreInput = true;
						}
					}
/*					if( Mathf.Abs(diff.y)>flicksDistance && YFlick )
					{
						if( diff.y!=0f )
						{
							//Debug.Log("left flick is exec!! ["+diffY+"] scrollDistance="+scrollDistance);
							scrollSpeed.y = -( diff.y/scrollRate );
							flicking = true;
							scrollDirectionY = diff.y>0 ? 1:0;
						}
					}
*/				}
				touchStatus = 4;
			}
		}
		else if( Input.GetKeyDown( KeyCode.LeftArrow ) && pageXID>0 )
		{
			pageXID--;
			keyInput = true;
		}
		else if( Input.GetKeyDown( KeyCode.RightArrow) && pageXID<pageXMax )
		{
			pageXID++;
			keyInput = true;
		}
		
		p.x += scrollSpeed.x;
		p.y += scrollSpeed.y;
		scrollSpeed.x = deceleration(scrollSpeed.x, 0.0f, flickSpeed, round);
		scrollSpeed.y = deceleration(scrollSpeed.y, 0.0f, flickSpeed, round);
		
		// 速度が落ちた時のパネルを中央に合わせる処理
		if( !keyInput && !startEffect )
		{
			for( int i=panelXList.Count-1 ; i>=0 ; i-- )
			{
				if( -panelXList[i]>=p.x )
				{
					pageXID = i;
					break;
				}
			}
			if( flicking )
			{
				pageXID += scrollDirectionX;
			}
			else if( pageXID<panelXList.Count-1 )
			{
				if( Mathf.Abs(panelXList[pageXID]+p.x) > Mathf.Abs(panelXList[pageXID+1]+p.x) )
				{
					pageXID += 1;
				}
			}
		}
		pageXID = pageXID>pageXMax?pageXMax:pageXID;
		pageXID = pageXID<0?0:pageXID;
		pageYID = pageYID>pageYMax?pageYMax:pageYID;
		pageYID = pageYID<0?0:pageYID;
		if( scrollSpeed.x==0.0f )
		{
			pageCenter.x = -panelXList[pageXID];
		}
		if( scrollSpeed.y==0.0f )
		{
			pageCenter.y = -panelYList[pageYID];
		}
		
		
		
		if( pageCenter.x!=p.x && scrollSpeed.x==0f && scrollSpeed.y==0.0f && touchStatus==0 )
		{
			p.x = deceleration(p.x, pageCenter.x, adjust, 0.001f);
		}
		if( pageCenter.y!=p.y && scrollSpeed.x==0f && scrollSpeed.y==0.0f && touchStatus==0 )
		{
			p.y = deceleration(p.y, pageCenter.y, adjust, 0.001f);
		}
	//	Debug.Log("p.x="+p.x+"/scrollSpeed="+scrollSpeed);
		
		// 画面端に来たときの処理
		if( (p.x>basePosition.x || p.x<-panelXList[pageXMax]) && flicking )
		{
			scrollSpeed.x = 0.0f;
		}
		if( (p.y>basePosition.y || p.y<panelYList[pageYMax]) && flicking )
		{
			scrollSpeed.y = 0.0f;
		}
		
		// 動きが止まったときの処理
		if( scrollSpeed.x==0.0f && scrollSpeed.y==0.0f && pageCenter.x==p.x && pageCenter.y==p.y && touchStatus!=1 )
		{
			flicking = false;
			keyInput = false;
			startEffect = false;
//			buttonManager.ignoreInput = false;
		}
	
		gameObject.transform.localPosition = p;
	}
	
	public static float deceleration( float _pos, float pos, float sp ) {
		return deceleration(_pos,pos,sp,0f);
	}
	public static float deceleration( float _pos, float pos, float sp, float rnd ) {
		
		float dif = Mathf.Abs(_pos)-Mathf.Abs(pos);
		if( Mathf.Abs(dif)<rnd )
		{
			return pos;
		}
		return ( pos - ( pos - _pos ) * sp / 100.0f );
	}
}
