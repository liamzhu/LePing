using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class IOSTencentMap : MonoBehaviour 
{
	[DllImport("__Internal")]    
	private static extern void locateIos();

	private static IOSTencentMap _instance;
	public static IOSTencentMap Instance
	{
		get
		{
			return _instance;
		}
	}

	public Action<string> OnGetAddress;

	private void Awake()
	{
		_instance = this;

	}

	public void StartGetAddr()
	{
		locateIos ();
	}

	public void ReceiveLocation(string location)
	{
		OnGetAddress (location);
	}
}
