using Godot;
using System.Collections.Generic;
using System;

public class DebugOverlay : Label
{
	private List<Stat> stats = new List<Stat>();

	public class Stat
	{
		public Stat(string _statName, Godot.Object _objectRef, string _statRef, bool _isMethod)
		{
			statName = _statName;
			objectRef = _objectRef;
			statRef = _statRef;
			isMethod = _isMethod;
		}
		public string statName;
		public Godot.Object objectRef;
		public string statRef;
		public bool isMethod;
	}

	public void AddStat(string statName, Godot.Object _object, string statRef, bool isMethod)
	{
		stats.Add(new Stat(statName, _object, statRef, isMethod));
	}
	public override void _Process(float delta)
	{
		string labelText = "";
		foreach (var stat in stats)
		{
			string value = "";
			if (stat.objectRef != null && WeakRef(stat.objectRef).GetRef() != null)
			{
				if ((bool)stat.isMethod)
				{
					value = (stat.objectRef).Call(stat.statRef).ToString();
				}
				else
				{
					value = (stat.objectRef).Get(stat.statRef).ToString();
				}
				labelText += (string.Format("{0}: {1}\n", stat.statName, value));
			}
		}
		Text = labelText;

	}
}
