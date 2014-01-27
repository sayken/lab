using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/*
public class testlinq : MonoBehaviour {

	public List<RecordClass> record;

	private object Test()
	{

		var stage = record.FirstOrDefault(x => x.Key == "Stage1");
		if (stage == null)
			return null;

		var area = ((List<RecordClass>)stage.Value).FirstOrDefault(x=> x.Key == "Area1");

		area.Value = "hhhhhhhh";
		return area.Value;
	}


	void Start()
	{
		record = new List<RecordClass>();
		record.Add(new RecordClass{Key = "Stage0", Value = "2589"});

		List<RecordClass> record2 = new List<RecordClass>();
		record2.Add( new RecordClass{Key="Area1", Value = "xxxxx"});
		record.Add (new RecordClass{Key="Stage1", Value = record2});

		foreach ( RecordClass rc in record )
		{
			if ( rc.Value.GetType() == typeof(List<RecordClass>) )
			{
				foreach( RecordClass rc2 in (List<RecordClass>)rc.Value )
				{
					Debug.Log (rc.Key + " / "+ rc2.Key +" : "+ rc2.Value );
				}
				continue;
			}
			Debug.Log ( rc.Key +":"+ rc.Value );
		}


		var t = Test ();
		t = "uuuuuu";

		foreach ( RecordClass rc in record )
		{
			if ( rc.Value.GetType() == typeof(List<RecordClass>) )
			{
				foreach( RecordClass rc2 in (List<RecordClass>)rc.Value )
				{
					Debug.Log (rc.Key + " / "+ rc2.Key +" : "+ rc2.Value );
				}
				continue;
			}
			Debug.Log ( rc.Key +":"+ rc.Value );
		}
	}
}


public class RecordClass
{
	public object Key { get; set; }
	public object Value { get; set; }
}
*/