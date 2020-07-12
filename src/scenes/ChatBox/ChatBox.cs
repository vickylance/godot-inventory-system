using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ChatBox : Control
{
	[Export]
	// public string WebsocketUrl = "ws://localhost:3000/socket.io/?EIO=3&transport=websocket";
	public string websocketUrl = "ws://localhost:3000";

	struct ChatGroup
	{
		private readonly string name;
		private readonly string color;

		public ChatGroup(string name, string color)
		{
			this.name = name;
			this.color = color;
		}

		public string Name { get { return name; } }
		public string Color { get { return color; } }
	}

	private RichTextLabel chatLog;
	private Label inputLabel;
	private LineEdit inputField;
	private readonly IList<ChatGroup> chatGroups = new ReadOnlyCollection<ChatGroup>(
		new[]{
			new ChatGroup("Private", "#d62dc0"),
			new ChatGroup("Party", "#4287f5"),
			new ChatGroup("Local", "#25b847"),
			new ChatGroup("Global", "#e8f016"),
		});
	private int groupIndex = 0;
	private string username = "Vickylance";
	private WebSocketClient ws = new WebSocketClient();

	// virtual methods

	public override void _Ready()
	{
		chatLog = FindNode("ChatLog") as RichTextLabel;
		inputLabel = FindNode("ChatGroup") as Label;
		inputField = FindNode("ChatInput") as LineEdit;
		inputField.Connect("text_entered", this, nameof(_ChatInputEntered));
		ChangeGroup(0);

		ws.Connect("connection_closed", this, nameof(_ConnectionClosed));
		ws.Connect("connection_error", this, nameof(_ConnectionClosed));
		ws.Connect("connection_established", this, nameof(_ConnectionEstablished));
		ws.Connect("data_received", this, nameof(_OnDataReceived));

		// Initiate connection to the given URL.
		var err = ws.ConnectToUrl(websocketUrl);
		if (err != Error.Ok)
		{
			GD.Print("Unable to connect");
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey inputKey)
		{
			if (inputKey.IsPressed() && (KeyList)inputKey.Scancode == KeyList.Enter)
			{
				inputField.GrabFocus();
			}
			if (inputKey.IsPressed() && (KeyList)inputKey.Scancode == KeyList.Escape)
			{
				inputField.ReleaseFocus();
			}
			if (inputKey.IsPressed() && (KeyList)inputKey.Scancode == KeyList.Tab)
			{
				ChangeGroup(1);
			}
		}
	}

	public override void _Process(float delta)
	{
		if (ws.GetConnectionStatus() == WebSocketClient.ConnectionStatus.Connecting || ws.GetConnectionStatus() == WebSocketClient.ConnectionStatus.Connected)
		{
			ws.Poll();
		}
		if (ws.GetPeer(1).IsConnectedToHost())
		{
			GD.Print("cnn");
			if (ws.GetPeer(1).GetAvailablePacketCount() > 0)
			{
				GD.Print("cnn ava");
				var message = ws.GetPeer(1).GetVar();
				if (message != null)
				{
					GD.Print($"Receive: {message}");
				}
				// var message = System.Text.Encoding.UTF32.GetString((byte[])ws.GetPeer(1).GetVar());
				// GD.Print($"Receive: {message}");
				// chatLog.BbcodeText += message;
			}
		}
	}

	// public methods

	public void ChangeGroup(int value)
	{
		groupIndex += value;
		if (groupIndex > chatGroups.Count - 1)
		{
			groupIndex = 0;
		}
		else if (groupIndex < 0)
		{
			groupIndex = chatGroups.Count - 1;
		}
		inputLabel.Text = $"[{chatGroups[groupIndex].Name}]";
		inputLabel.Set("custom_colors/font_color", new Color(chatGroups[groupIndex].Color));
	}

	public void AddMessage(string username, string text, int group = 0)
	{
		if (ws.GetPeer(1).IsConnectedToHost())
		{
			chatLog.BbcodeText += $"[color={chatGroups[group].Color}][{username}]: {text}[/color]\n";
		}
	}

	// private methods

	public void _ChatInputEntered(string text)
	{
		if (text != "")
		{
			inputField.Text = "";
			Error err = ws.GetPeer(1).PutVar(text);
			if (err == Error.Ok)
			{
				AddMessage(username, text, groupIndex);
			}
			else
			{
				GD.PrintErr("Send chat text failed");
			}
		}
	}

	private void _ConnectionClosed(bool wasClean)
	{
		GD.Print("Disconnected: ", wasClean);
	}

	private void _ConnectionEstablished(string proto)
	{
		GD.Print("Connected with protocol: ", proto);
	}

	private void _OnDataReceived()
	{
		// GD.Print("Got data from server: ", ws.GetPeer(1).GetVar());
		// GD.Print("Got data from server: ", System.Text.Encoding.UTF8.GetString(ws.GetPeer(1).GetPacket()).TrimEnd('\0'));

		var message = ws.GetPeer(1).GetVar().ToString();
		AddMessage(username, message);
	}
}
