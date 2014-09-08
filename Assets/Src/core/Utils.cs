using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace core
{
	public static class Utils
	{

		public static List<string> ReadLinesFromResourceFile(string filename)
		{
			List<string> result = null;

			try 
			{
				TextAsset textData = Resources.Load(filename) as TextAsset;
				
				if (textData != null)
				{
					result = new List<string>();
					
					StringReader strReader = new StringReader(textData.text);   
					
					while (true)
					{
						string str = strReader.ReadLine();
					
						if (str != null)
						{
							result.Add(str);
						}
						else
						{  // end
							break;
						}
					}
				}
			}
			catch(IOException ex)
			{}
			
			
			return result;
		}

	}
}