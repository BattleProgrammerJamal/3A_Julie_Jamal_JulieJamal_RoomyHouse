using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GeneralSettingsPanel : MonoBehaviour 
{
	[SerializeField]
	private string _iniUrl = "param.ini";
	public string IniUrl
	{
		get { return _iniUrl; }
		set { _iniUrl = value; }
	}

	[SerializeField]
	private List<string> _paramNames;
	public List<string> ParamNames
	{
		get { return _paramNames; }
		set { _paramNames = value; }
	}
	
	[SerializeField]
	private List<string> _params;
	public List<string> Params
	{
		get { return _params; }
		set { _params = value; }
	}
	
	private Vector2 scrollPosition = Vector2.zero;
	private Rect _windowBox;

	void Start()
	{
		float w = Screen.width, h = Screen.height;
		_windowBox = new Rect(w * 0.05f, h * 0.05f, w * 0.90f, h * 0.90f);
		prepareParams();
	}

	void OnGUI()
	{
		GUI.Window(10, _windowBox, WindowHandler, "Settings");
	}

	void WindowHandler(int id)
	{
		float w = Screen.width, h = Screen.height;

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(_windowBox.width * 0.95f), GUILayout.Height(_windowBox.height * 0.85f));
			
			for(int i = 0; i < ParamNames.Count; ++i)
			{
				GUILayout.BeginVertical();
					GUILayout.BeginHorizontal();
						GUILayout.Label(ParamNames[i]);
						if(Params[i].Equals("True") || Params[i].Equals("False"))
						{
							bool result = (Params[i].Equals("True")) ? true : false;
							Params[i] = (GUILayout.Toggle(result, string.Empty)).ToString();
						}
						else
						{
							if(ParamNames[i].Equals("Chapeaux"))
							{
								string[] chapeaux = Params[i].Split(new char[] { ';' });
								string[] chapeauxx = new string[chapeaux.Length - 1];
								int result = int.Parse(chapeaux[0]);

								for(int k = 1; k < chapeaux.Length; ++k)
								{
									chapeauxx[k - 1] = chapeaux[k];
								}

								result = GUILayout.SelectionGrid(result, chapeauxx, 3);

								string str_res = result.ToString() + ";";
								
								for(int k = 0; k < chapeauxx.Length; ++k)
								{
									if(k == chapeauxx.Length - 1) { str_res += chapeauxx[k]; } else { str_res += chapeauxx[k] + ";"; }
								}

								Params[i] = str_res;
							}
							else
							{
								Params[i] = GUILayout.TextField(Params[i], 35);
							}
						}
					GUILayout.EndHorizontal();
				GUILayout.EndVertical();
				
			}

			GUILayout.BeginHorizontal();
				if(GUILayout.Button("Save"))
				{
					saveIni(IniUrl, ParamNames, Params);
					Application.LoadLevel("Menu");
				}

				if(GUILayout.Button("Cancel"))
				{
					Application.LoadLevel("Menu");
				}
			GUILayout.EndHorizontal();
				

		GUILayout.EndScrollView();

		if(Event.current.type == EventType.Repaint)
		{
			_windowBox = new Rect(w * 0.05f, h * 0.05f, w * 0.90f, h * 0.90f);
		}
	}

	public void prepareParams()
	{
		Dictionary<string, string> dc = readIni(IniUrl);
		this.ParamNames.Clear();
		this.Params.Clear();

		foreach(KeyValuePair<string, string> entry in dc)
		{
			this.ParamNames.Add(entry.Key);
			this.Params.Add(entry.Value);
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

	public static void saveIni(string url, List<string> names, List<string> values)
	{
		if(names.Count == values.Count)
		{
			StreamWriter writer = new StreamWriter(url);

			for(int i = 0; i < names.Count; ++i)
			{
				writer.WriteLine("[" + names[i] + "]");
				writer.WriteLine(values[i] + "\n");
			}

			writer.Close();
		}
	}
}
