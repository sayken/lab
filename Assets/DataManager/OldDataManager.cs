using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldDataManager : MonoBehaviour {

	const string STASH = "_STASH_";
	Dictionary<string,object> Data;

	/**
	  * set data to dictionary
	  */
	public void SetData( string json )
	{
		if ( Data == null )
		{
			Data = new Dictionary<string, object>();
		}
		IDictionary jsondic = (IDictionary)MiniJSON.Json.Deserialize( json );

		//  check
		if ( ((Dictionary<string,object>)jsondic).ContainsKey("data") == false )
		{
			Debug.LogWarning("This json has no data key in top.");
			return;
		}
		if ( jsondic["data"].GetType () != typeof(IDictionary) && jsondic["data"].GetType () != typeof(Dictionary<string,object>) )
		{
			Debug.LogWarning("This jsons to object is not IDictionary or Dictionary<string,object>.\nThis object type is "+ jsondic["data"].GetType ());
			return;
		}

		// set data
		IDictionary res = (IDictionary)jsondic["data"];
		
		foreach ( DictionaryEntry entry in res )
		{
			string key = (string)entry.Key;
			if ( key == STASH )
			{
				Debug.LogWarning("You Cant use name "+ STASH +". This object skipped.");
				continue;
			}
			if ( Data.ContainsKey( key ) )
			{
				Data.Remove( key );
			}
			Data.Add( key, entry.Value );
		}
	}

	/**
	  * count the number of datas
	  */
	public int Count ( string keyName )
	{
		int count = -1;
		if ( !Data.ContainsKey(keyName) )
		{
			return count;
		}
		
		if ( Data[keyName].GetType() == typeof(List<object>) )
		{
			count = ((IList)Data[keyName]).Count;
		}
		else if( Data[keyName].GetType() == typeof(Dictionary<string,object>) )
		{
			count = ((IDictionary)Data[keyName]).Count;
		}
		return count;
	}

	/**
	 *   save data to playerplefs
	 */
	public void Save( string keyName )
	{
		string json = MiniJSON.Json.Serialize( (IDictionary)Data[ keyName ] );
		string saveStr = "{\"data\":{\""+ keyName +"\":"+ json +"}}";
		
//		Debug.Log("saveStr="+ saveStr);
		PlayerPrefs.SetString( keyName, saveStr );
		PlayerPrefs.Save();
	}

	/**
	  * load data from playerprefs
	  */
	public void Load( string keyName )
	{
		string json = PlayerPrefs.GetString( keyName, "");
		if ( json != "" )
		{
			SetData(json);
		}
	}

	/**
	 *  get object from data
	 */
	public object GetObject( string keyName, List<string> paths )
	{
		if ( !Data.ContainsKey(keyName) )
		{
			return null;
		}
		object obj = Data[keyName];
		if ( paths == null )
		{
			return Data[keyName];
		}
		foreach ( string path in paths )
		{
			obj = GetNextObject( Data[keyName], path );
			if ( obj==null )
			{
				return null;
			}
			if ( obj.GetType () == typeof(string) )
			{
				return obj;
			}
		}
		return null;
	}

	/**
	 *  get object from obj
	 */
	private object GetNextObject( object obj, string key )
	{
Debug.Log("GetNextObject key="+key);
		if ( obj.GetType() == typeof(List<object>) )
		{
Debug.Log("this Data is List.");
			IList ilist = (IList)obj;
			
			int num;
			bool flag = int.TryParse( key, out num );
			if ( !flag || num > ilist.Count )
			{
				Debug.LogError("Error:OutOfIndex of List -> "+ key);
				return null;
			}
			return ilist[num];
		}
		else if( obj.GetType() == typeof(Dictionary<string,object>) )
		{
Debug.Log("this Data is Dictionary.");
			
			Dictionary<string,object> idic = (Dictionary<string,object>)obj;
			if ( !idic.ContainsKey(key) )
			{
				Debug.LogError("Error:KeyNotFound from Dic -> "+ key);
				return null;	
			}
			return idic[key];
		}
		else if ( obj.GetType () == typeof(string) )
		{
Debug.Log("this Data is string.");
			return obj;
		}
		return null;
	}

	/**
	 *  get string from Data
	 */
	public string GetString( string keyName, List<string> paths )
	{
		object obj = GetObject ( keyName, paths );
		if ( obj==null || obj.GetType ()!=typeof(string) )
		{
			return null;
		}
		return obj.ToString ();
	}

	/**
	 *  Set Data to Stash
	 */
	public void SetStash( string keyName, List<string> paths )
	{
		if ( Data.ContainsKey(keyName) )
		{
			object obj = GetObject ( keyName, paths );
			Data[STASH] = obj;
		}
	}

	public void UpdateString( string keyName, List<string> paths, string value )
	{
		string str = GetString(keyName, paths );
		if ( str == null )
		{
			return;
		}
		Debug.Log("str="+ str);
		str = "hogehoge";
		Debug.Log( "str="+ GetString ( keyName, paths ) );
	}
}
