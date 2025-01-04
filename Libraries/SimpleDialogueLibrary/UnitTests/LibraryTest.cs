using Sandbox;
using SimpleDialogue;

[TestClass]
public partial class LibraryTests
{
	[TestMethod]
	public void ParseSimpleText()
	{
		var panel = new SimpleDialoguePanel( "Test" );
		panel.InitalizeUnitTest();
		foreach ( var character in panel.ActiveDialogue )
		{
			panel.CallNextCharacterParse();
		}
		
		Log.Info( $"[ControlCodeParsingNoText] Currently displayed dialogue: \'{panel.DisplayedDialogue}\'" );
		Assert.IsTrue( panel.DisplayedDialogue == "Test" );
	}
	[TestMethod]
	public void ControlCodeParsingNoText()
	{
		var panel = new SimpleDialoguePanel( "[[color:red]]" );
		panel.InitalizeUnitTest();
		foreach ( var character in panel.ActiveDialogue )
		{
			panel.CallNextCharacterParse();
		}

		Log.Info( $"[ControlCodeParsingNoText] Currently displayed dialogue: \'{panel.DisplayedDialogue}\'" );
		Assert.IsTrue( panel.DisplayedDialogue == string.Empty );
	}

	[TestMethod]
	public void ControlCodeParsingWithText()
	{
		var panel = new SimpleDialoguePanel( "Human, I remember your [[color:red]]genocides." );
		panel.InitalizeUnitTest();
		foreach ( var character in panel.ActiveDialogue )
		{
			panel.CallNextCharacterParse();
		}

		Log.Info( $"[ControlCodeParsingWithText] Currently displayed dialogue: \'{panel.DisplayedDialogue}\'" );
		Assert.IsTrue( panel.DisplayedDialogue == "Human, I remember your genocides." );
	}

	[TestMethod]
	public void ControlCodeParsingWithMultipleCodes()
	{
		var panel = new SimpleDialoguePanel( "[[color:red]]Test[[color:blue]]Test[[color:yellow]]Test" );
		panel.InitalizeUnitTest();
		foreach ( var character in panel.ActiveDialogue )
		{
			panel.CallNextCharacterParse();
		}

		Log.Info( $"[ControlCodeParsingWithText] Currently displayed dialogue: \'{panel.DisplayedDialogue}\'" );
		Assert.IsTrue( panel.DisplayedDialogue == "TestTestTest" );
	}
}
