using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GeneralInitDefaultAppParamsScript : MonoBehaviour 
{
	[SerializeField]
	private string _iniUrl = "param.ini";
	public string IniUrl
	{
		get { return _iniUrl; }
		set { _iniUrl = value; }
	}
	
	[SerializeField]
	private int _messageTimeOut = 5;
	public int MessageTimeOut
	{
		get { return _messageTimeOut; }
		set { _messageTimeOut = value; }
	}
	
	private bool error_occured = false;
	private string error_message = string.Empty;
	
	void Awake()
	{
		try
		{
			Dictionary<string, string> dc = readIni(IniUrl);
			int w = Screen.width, h = Screen.height;
			bool change_size = false, fullscreen_mode = false;
			
			foreach(KeyValuePair<string, string> entry in dc)
			{
				if(entry.Key.Equals("Debug"))
				{
					GeneralCreateMenuGuiScript script = GetComponent<GeneralCreateMenuGuiScript>();

					if(entry.Value.Equals("True"))
					{
						script.DebugMode = true;
					}
					else
					{
						script.DebugMode = false;
					}
				}

				if(entry.Key.Equals("IsClient"))
				{
					GeneralCreateMenuGuiScript script = GetComponent<GeneralCreateMenuGuiScript>();

					if(!script.DebugMode)
					{
						if(entry.Value.Equals("True"))
						{
							script.IsClient = true;
						}
						else
						{
							script.IsClient = false;
						}
					}
				}

				if(entry.Key.Equals("Fullscreen"))
				{
					if(entry.Value.Equals("True"))
					{
						fullscreen_mode = true;
					}
					else
					{
						fullscreen_mode = false;
					}
				}

				if(entry.Key.Equals("Width"))
				{
					change_size = true;
					w = int.Parse(entry.Value);
				}

				if(entry.Key.Equals("Height"))
				{
					h = int.Parse(entry.Value);
					change_size = true;
				}

				if(entry.Key.Equals("Didactitiel"))
				{
					if(entry.Value.Equals("True"))
					{
						GeneralCreateMenuGuiScript.Didactitiel = true;
					}
					else
					{
						GeneralCreateMenuGuiScript.Didactitiel = false;
					}
				}

				if(entry.Key.Equals("Chapeaux"))
				{
					string[] chapeaux = entry.Value.Split(new char[] { ';' });
					GeneralCreateMenuGuiScript.Chapeau_Id = int.Parse(chapeaux[0].ToString());
				}
			}

			if(change_size)
			{
				Screen.SetResolution(w, h, Screen.fullScreen);
			}

			Screen.fullScreen = fullscreen_mode;
		}
		catch(System.Exception ex)
		{
			Debug.Log("ERROR : File not found");
			Debug.Log(ex.ToString());
			
			error_message = ex.Message;
			
			RaiseError();
			Invoke("KillError", MessageTimeOut);
		}
	}
	
	void RaiseError()
	{
		error_occured = true;
	}
	
	void KillError()
	{
		error_occured = false;
	}
	
	void OnGUI()
	{
		if(error_occured)
		{
			float w = Screen.width, h = Screen.height;
			GUIStyle style = new GUIStyle();
			style.fontSize = 30;
			style.normal.textColor = Color.white;
			style.wordWrap = true;
			GUI.Box(new Rect(w * 0.10f, h * 0.10f, w * 0.70f, h * 0.70f), "");
			GUI.Label(new Rect(w * 0.25f, h * 0.25f, w * 0.50f, h * 0.50f), "ERROR : " + error_message, style);
		}
	}
	
	public static Dictionary<string, string> readIni(string url)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        string line = string.Empty, section = string.Empty, data = string.Empty;
        int cpt = 0;
        
        StreamReader reader = new StreamReader(url);
        while ((line = reader.ReadLine()) != null)
        {
            if(line != "")
            {
                if(line[0].Equals('['))
                {
                    string tmp = string.Empty;
                    for(int i = 1; i < line.Length - 1; ++i)
                    {
                        tmp += line[i];
                    }
                    
                    section = tmp;
                    ++cpt;
                }
                else
                {
                    data = line;
                    ++cpt;
                }
                
                if(cpt >= 2)
                {
                    result.Add(section, data);
                    cpt = 0;
                }
            }
        }

        return(result);
    }
}