using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;

public sealed class ControllerDebugHUDVR : Component
{
	[Property] public Sandbox.WorldPanel DebugPanel;

	Dictionary<string, Panel> panelMap = new Dictionary<string, Panel>();

	protected override void OnStart()
	{
		if (DebugPanel == null) {
			throw new Exception("Cannot use ControllerDebugHUDVR without a WorldPanel!");
		}
	}

	public void AddText(string key)
	{
		var label = new Label();
		label.Text = "";
		label.Style.FontColor = Color.White;
		label.Style.FontWeight = 500;
		label.Parent = DebugPanel.GetPanel();
		panelMap.Add(key, label);	
	}
	
	public void SetText(string key, string text) {
		if (panelMap.ContainsKey(key)) {
			(panelMap[key] as Label).Text = text;
		}
	}

	protected override void OnUpdate()
	{

	}
}
