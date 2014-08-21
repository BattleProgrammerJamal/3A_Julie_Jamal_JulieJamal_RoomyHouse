using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Toolkit : MonoBehaviour 
{
	public static void MessageBox(string message)
	{
		GUI.Box(new Rect(Screen.width * 0.33f, Screen.height * 0.25f, Screen.width * 0.33f, Screen.height * 0.25f), message);
	}

	public static void Log<T>(T data)
	{
		FileStream fstream = new FileStream("log_output.txt", FileMode.Append, FileAccess.Write);
		StreamWriter swriter = null;

		try
		{
			swriter = new StreamWriter(fstream);

			swriter.WriteLine(data);
		}
		catch(System.Exception ex)
		{
			Debug.Log(ex.StackTrace.ToString());
		}
		finally
		{
			if(swriter != null)
			{
				swriter.Close();
			}
		}
	}
}
