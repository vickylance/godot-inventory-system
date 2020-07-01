using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class ChatBox : Control
{

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


	public override void _Ready()
	{
		chatLog = FindNode("ChatLog") as RichTextLabel;
		inputLabel = FindNode("ChatGroup") as Label;
		inputField = FindNode("ChatInput") as LineEdit;
		inputField.Connect("text_entered", this, nameof(_ChatInputEntered));
		ChangeGroup(0);
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
		chatLog.BbcodeText += $"[color={chatGroups[group].Color}][{username}]: {text}[/color]\n";
	}

	public void _ChatInputEntered(string text)
	{
		if (text != "")
		{
			inputField.Text = "";
			AddMessage(username, text, groupIndex);
		}
	}
}
