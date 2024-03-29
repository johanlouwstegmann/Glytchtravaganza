﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
	GUIStyle debugLabelStyle;
	private bool _showDebug = false;
	private List<DebugEntry> debugEntries = new List<DebugEntry>();
	public void Awake()
	{
		debugLabelStyle = new GUIStyle();
		debugLabelStyle.fontSize = 30;
		debugLabelStyle.fontStyle = FontStyle.Bold;
		debugLabelStyle.normal.textColor = Color.white;

		BuildDebugMenu();
	}

	private void BuildDebugMenu()
	{
		debugEntries.Add(new DebugLabel("Frame Rate", () => { return Mathf.RoundToInt(1f / Time.deltaTime).ToString(); }));
		debugEntries.Add(new DebugLabel("Memory", () => { return String.Format("System {0} | Graphics {1}", SystemInfo.systemMemorySize, SystemInfo.graphicsMemorySize); }));
		debugEntries.Add(new DebugLabel("Window", () => { return String.Format("Width {0} | Height {1} | DPI {2}", Camera.main.pixelWidth, Camera.main.pixelHeight, Screen.dpi); }));
		debugEntries.Add(new DebugLabel("Platform", () => { return Application.platform.ToString(); }));
	}

	public void OnGUI()
	{
		GUILayout.BeginVertical();
		if (GUILayout.Button("Debug"))
		{
			_showDebug = !_showDebug;
		}

		if (_showDebug)
		{
			foreach (var entry in debugEntries)
			{
				entry.Draw(debugLabelStyle);
			}
		}
		GUILayout.EndVertical();
	}
}

public abstract class DebugEntry
{
	public abstract void Draw(GUIStyle style);
}

public class DebugLabel : DebugEntry
{
	public string Name;
	public Func<string> Func;

	public DebugLabel(string name, Func<string> func)
	{
		Name = name;
		Func = func;
	}

	public DebugLabel(string name, string text)
	{
		Name = name;
		Func = () => { return text; };
	}

	public override void Draw(GUIStyle style)
	{
		String s = String.Format("{0} : {1}", Name, Func());
		GUILayout.Label(s, style);
	}
}
