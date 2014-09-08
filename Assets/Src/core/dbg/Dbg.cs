using UnityEngine;
using System.Collections.Generic;

namespace core
{
	namespace dbg
	{
		public static class Dbg
		{
			private static bool UseLogService { get; set; }

			public static void Log(string message)
			{
				Debug.Log (message);
			
				if (UseLogService)
				{
					CrittercismAndroid.LeaveBreadcrumb("Time.time: " + message);
				}
			}
		}
	}
}