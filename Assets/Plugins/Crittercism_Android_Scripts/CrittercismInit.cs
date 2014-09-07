using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrittercismInit : MonoBehaviour {


	private const string CrittercismAppID = "540c7855bb94757ba4000003";
	private const bool bDelaySendingAppLoad = false;
	private const bool bShouldCollectLogcat = false;
	private const string CustomVersionName = "";
	void Awake ()
	{

#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2
#else
		CrittercismAndroid.Init(CrittercismAppID, bDelaySendingAppLoad, bShouldCollectLogcat, CustomVersionName);
#endif

#if UNITY_3_3 || UNITY_3_4 || UNITY_3_4_1 || UNITY_3_4_2
#else
		Destroy(this);
#endif

		CrittercismAndroid.SetUsername(SystemInfo.deviceUniqueIdentifier);
		
		
		// Create a list then convert them to arrays to pass them through.
		List<string> arrayOfKeys = new List<string>();
		List<string> arrayOfValues = new List<string>();
		
		arrayOfKeys.Add("DeviceModel");
		arrayOfKeys.Add("DeviceName");
		arrayOfKeys.Add("DeviceType");
		arrayOfKeys.Add("Os");
		arrayOfValues.Add(SystemInfo.deviceModel);
		arrayOfValues.Add(SystemInfo.deviceName);
		arrayOfValues.Add(SystemInfo.deviceType.ToString());
		arrayOfValues.Add(SystemInfo.operatingSystem);
		
		CrittercismAndroid.SetMetadata(arrayOfKeys.ToArray(), arrayOfValues.ToArray());
	}
	
	void Update() {
		CrittercismAndroid.Update();
	}
}
