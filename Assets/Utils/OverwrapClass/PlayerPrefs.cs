using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerPrefs
{	
	// Encrypted PlayerPrefs
	// Written by Sven Magnus
	// MD5 code by Matthew Wegner (from [url]http://www.unifycommunity.com/wiki/index.php?title=MD5[/url])
	
	// Modify this key in this file :
	private static string privateKey_checksum = "9ETrEsWaFRach3gexaDrchute";
	private static string privateKey_encrypt = "9ETrEsWaFRach3gexaDrchuteHdI845o";

	private static int incrementInt = 1337;

	// Add some values to this array before using EncryptedPlayerPrefs
	public static string[] keys = new string[]
	{
		"23Wrudre",
		"SP9DupHa",
		"frA5rAS3",
		"tHat2epr",
		"jaw3eDAs"
	};
	
	public static string Md5(string strToEncrypt)
	{
		UTF8Encoding ue = new UTF8Encoding();
		
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		
		return hashString.PadLeft(32, '0');
	}
	
	public static void SaveChecksum(string key, string type, string value)
	{
		int keyIndex = (int)Mathf.Floor(UnityEngine.Random.value * keys.Length);
		
		string secretKey = keys[keyIndex];
		
		string check = Md5(type + "_" + privateKey_checksum + "_" + secretKey + "_" + value);
		
		UnityEngine.PlayerPrefs.SetString(key + "_encryption_check", check);
		
		UnityEngine.PlayerPrefs.SetInt(key + "_used_key", keyIndex);
		
	}
	
	public static bool Checksum(string key, string type, string value)
	{
		int keyIndex = UnityEngine.PlayerPrefs.GetInt(key + "_used_key");
		
		string secretKey = keys[keyIndex];
		
		string check = Md5(type + "_" + privateKey_checksum + "_" + secretKey + "_" + value);
		
		if(!UnityEngine.PlayerPrefs.HasKey(key + "_encryption_check")) return false;
		
		string storedCheck = UnityEngine.PlayerPrefs.GetString(key + "_encryption_check");
		
		return storedCheck == check;
	}
	
	public static string Encrypt (string toEncrypt)
	{
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes (privateKey_encrypt);
		// 256-AES key
		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes (toEncrypt);
		RijndaelManaged rDel = new RijndaelManaged ();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		// http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
		rDel.Padding = PaddingMode.PKCS7;
		// better lang support
		ICryptoTransform cTransform = rDel.CreateEncryptor ();
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		return Convert.ToBase64String (resultArray, 0, resultArray.Length);
	}
	
	public static string Decrypt (string toDecrypt)
	{
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes (privateKey_encrypt);
		// AES-256 key
		byte[] toEncryptArray = Convert.FromBase64String (toDecrypt);
		RijndaelManaged rDel = new RijndaelManaged ();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		// http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
		rDel.Padding = PaddingMode.PKCS7;
		// better lang support
		ICryptoTransform cTransform = rDel.CreateDecryptor ();
		byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		return UTF8Encoding.UTF8.GetString (resultArray);
	}
	
	public static void SetInt(string key, int value)
	{
		int incrementedValue = value + incrementInt;
		string encryptedValue = Encrypt (incrementedValue.ToString ());
		
		UnityEngine.PlayerPrefs.SetString(key, encryptedValue);
		SaveChecksum(key, "string", encryptedValue);
	}
	
	public static void SetFloat(string key, float value)
	{
		float incrementedValue = value + (float)incrementInt;
		string encryptedValue = Encrypt(incrementedValue.ToString ());
		
		UnityEngine.PlayerPrefs.SetString(key, encryptedValue);
		SaveChecksum(key, "string", encryptedValue);
	}
	
	public static void SetString(string key, string value)
	{
		string encryptedValue = Encrypt (value);
		
		UnityEngine.PlayerPrefs.SetString(key, encryptedValue);
		SaveChecksum(key, "string", encryptedValue);
	}
	
	public static int GetInt(string key)
	{
		return GetInt(key, 0);
	}
	
	public static float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}
	
	public static string GetString(string key)
	{
		return GetString(key, "");
	}
	
	public static int GetInt(string key, int defaultValue)
	{
		string value = UnityEngine.PlayerPrefs.GetString(key);
		
		if (!Checksum (key, "string", value)) return defaultValue;
		
		return int.Parse (Decrypt (value)) - incrementInt;
	}
	
	public static float GetFloat(string key, float defaultValue)
	{
		string value = UnityEngine.PlayerPrefs.GetString(key);
		
		if (!Checksum (key, "string", value)) return defaultValue;
		
		return float.Parse (Decrypt (value)) - (float)incrementInt;
	}
	
	public static string GetString(string key, string defaultValue)
	{
		string value = UnityEngine.PlayerPrefs.GetString(key);
		
		if (!Checksum (key, "string", value)) return defaultValue;
		
		return Decrypt (value);
	}
	
	public static bool HasKey(string key) 
	{
		return UnityEngine.PlayerPrefs.HasKey(key);
	}
	
	public static void DeleteKey(string key)
	{
		UnityEngine.PlayerPrefs.DeleteKey(key);
		UnityEngine.PlayerPrefs.DeleteKey(key + "_encryption_check");
		UnityEngine.PlayerPrefs.DeleteKey(key + "_used_key");
	}

	public static void DeleteAll()
	{
		UnityEngine.PlayerPrefs.DeleteAll();
	}
	public static void Save()
	{
		UnityEngine.PlayerPrefs.Save();
	}
	
	/// <summary>
	/// Saves an object's instance as a string inside PlayerPrefs.
	/// The class must have the [System.Serialiable] attribute!
	/// </summary>
	public static void SaveObjectAsString (string key, object theObject)
	{
		BinaryFormatter bf = new BinaryFormatter ();
		MemoryStream ms = new MemoryStream ();
		bf.Serialize (ms, theObject);
		SetString (key, Convert.ToBase64String (ms.GetBuffer()));
	}
	
	/// <summary>
	/// Loads an object's instance from a string stored inside PlayerPrefs.
	/// The class must have the [System.Serialiable] attribute!
	/// </summary>
	public static T LoadObjectFromString<T> (string label) where T : class
	{
		return Deserialize (label) as T;
	}
	
	/// <summary>
	/// Loads a struct from a string stored inside PlayerPrefs.
	/// The struct must have the [System.Serialiable] attribute!
	/// </summary>
	public static T LoadStructFromString<T> (string label) where T : struct
	{
		return (T)Deserialize (label);
	}
	
	static object Deserialize (string label)
	{
		string data = GetString (label);
		object result = null;
		
		if (!string.IsNullOrEmpty (data))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			MemoryStream ms = new MemoryStream (Convert.FromBase64String (data));
			result = bf.Deserialize (ms);
		}
		
		return result;
	}
}


