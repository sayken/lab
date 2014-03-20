using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/**
 * DataManager
 */
public class DataManager {

	private List<RecordClass> Data;

	private static DataManager instance;

	private DataManager()
	{
		Data = new List<RecordClass>();
	}

	public static DataManager Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = new DataManager();
			}
			return instance;
		}
	}

	public string GetString( List<string> keys )
	{
		object obj = GetObject( keys );
		if ( obj.GetType () == typeof(string) )
		{
			return obj.ToString ();
		}
		return "";
	}
	public object GetObject( List<string> keys )
	{
		return SearchAndUpdate ( keys, null );
	}
	public object SearchAndUpdate( List<string> keys, string value )
	{
		List<RecordClass> dat = Data;
		foreach ( string key in keys )
		{
			RecordClass rc = dat.FirstOrDefault(x => x.Key == key);
			if ( rc == null )
			{
				return null;
			}
			if ( rc.Value.GetType () == typeof(List<RecordClass>) )
			{
				dat = (List<RecordClass>)rc.Value;
				continue;
			}
			else if ( rc.GetType () == typeof(RecordClass) )
			{
				if ( value != null )
				{
					rc.Value = value;
				}
				return rc.Value.ToString ();
			}
		}
		return null;
	}
	public int Count( string key )
	{
		RecordClass dat = Data.FirstOrDefault(x => x.Key == key);
		if (dat == null)
		{
			return -1;
		}
		if ( dat.Value == null )
		{
			return -1;
		}
		return ((List<RecordClass>)dat.Value).Count;
	}

	public void SetData( string json )
	{
		IDictionary jsondic = (IDictionary)MiniJSON.Json.Deserialize( json );
		IDictionary res = (IDictionary)jsondic["data"];

		foreach ( DictionaryEntry entry in res )
		{
			string key = (string)entry.Key;

			var buf = Data.FirstOrDefault(x => x.Key == key);
			Data.Remove( buf );

			Data.Add( new RecordClass{ Key=key, Value=MakeList(entry.Value) } );
		}
	}

	List<RecordClass> MakeList( object dat )
	{
		List<RecordClass> recordClasses = new List<RecordClass>();

//Debug.Log ("dat.GetType()="+dat.GetType());
		if ( dat.GetType () == typeof(List<object>) )
		{
//Debug.Log ("list");
			IList list = (IList)dat;
			int i=0;
//Debug.Log ("i="+i);
			foreach ( var obj in list )
			{
				object val;
				if ( obj.GetType() == typeof(string) )
				{
//Debug.Log ("string:"+ obj.ToString ());

					val = obj.ToString();
				}
				else
				{
					val = MakeList (obj);
				}
				recordClasses.Add( new RecordClass{ Key=i.ToString (), Value=val } );
				i++;
			}
		}
		else if ( dat.GetType() == typeof(Dictionary<string, object>) )
		{
//Debug.Log ("dic");
			IDictionary dic = (IDictionary)dat;
			foreach ( var key in dic.Keys )
			{
//Debug.Log ("key:"+ key.ToString ());
				object val;
				if ( dic[key.ToString ()].GetType() == typeof(string) )
				{
//Debug.Log ("string:"+ dic[key.ToString ()].ToString());
					val = dic[key.ToString ()].ToString();
				}
				else
				{
					val = MakeList (dic[key.ToString ()]);
				}
				recordClasses.Add( new RecordClass{ Key=key.ToString(), Value=val } );
			}
		}
		return recordClasses;
	}

	public void Load( string key )
	{
		string json = PlayerPrefs.GetString( key, "");
		if ( json != "" )
		{
			SetData(json);
		}
	}
	public void Save( string key )
	{
		RecordClass dat = Data.FirstOrDefault(x => x.Key == key);

		string datJson = ToJson ((List<RecordClass>)dat.Value);

		if ( datJson != null )
		{
			string json = "{\"data\":{\""+ key +"\":"+ datJson +"}}";
Debug.Log ("save: "+ json);

			PlayerPrefs.SetString( key, json );
			PlayerPrefs.Save ();
		}
	}

	string ToJson( List<RecordClass> list )
	{
		if ( list==null )
		{
			return null;
		}

		string json = "{";
		string keyvalue = "";
		foreach( RecordClass rc in list )
		{
			if ( keyvalue.Length > 0 )
			{
				keyvalue += ",";
			}
			if ( rc.Value.GetType () == typeof(string) )
			{
				keyvalue += "\""+ rc.Key +"\":\""+ rc.Value +"\"";
			}
			else
			{
				keyvalue = ToJson ((List<RecordClass>)rc.Value);
			}
		}
		json += (keyvalue + "}");
		return json;
	}
}

public class RecordClass
{
	public string Key { get; set; }
	public object Value { get; set; }
}
