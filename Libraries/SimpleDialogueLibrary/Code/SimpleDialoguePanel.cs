using Sandbox;
using Sandbox.Razor;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDialogue;

public enum DialoguePrintStyle
{
	[Title( "Character By Character" )] 
	[Description( "Each character is printed one at a time with the specified delay." )]
	[Icon( "arrow_forward" )]
	CharacterByCharacter,

	[Title( "All At Once" )]
	[Description( "The entire string is printed all at once." )]
	[Icon( "double_arrow" )]
	AllAtOnce
}

public class SimpleDialoguePanel : Panel, IDialoguePanel
{
	// The labels we are displaying.
	public List<Label> Labels = new List<Label>();
	public Label ActiveLabel => Labels.Last();

	// This container holds all of the labels inside. This is so you can
	// manipulate the text specifically inside of your panel.
	public Panel DisplayContainer { get; protected set; }

	// ==========================================================

	// The print style of the text.
	public DialoguePrintStyle PrintStyle { get; set; }

	// The dialogue that needs to be displayed.
	public List<string> DialogueStrings { get; set; } = new();

	// The dialogue that is actively being parsed.
	public string ActiveDialogue { get; protected set; } = string.Empty;
	public int ActiveDialogueIndex { get; protected set; } = 0;

	// The index of the character we are going to process.
	private int Index = 0;

	// Dialogue currenlty being displayed with control codes removed.
	public string DisplayedDialogue => string.Join( "", Labels.Select( l => l.Text ) );

	// ==========================================================

	// Decide if we want to temporarily ignore user input (for dramatic purposes).
	public bool IgnoreUserInput = false;

	public bool WaitForTextCompletion = false;

	public bool SpeedUp = false;

	public string AdvanceCharacterKey { get; set; }
	public string? SpeedUpKey { get; set; }
	public float DelayAmountRemoved { get; set; }

	// ==========================================================

	public IDialoguePanel.OnCharacterPrinted OnCharacterPrintedAction { get; set; }
	public IDialoguePanel.OnControlCodeHit OnControlCodeHitAction { get; set; }
	public IDialoguePanel.OnAllCharactersPrinted OnAllCharactersPrintedAction { get; set; }
	public IDialoguePanel.OnAllDialogueDisplayed OnAllDialogueDisplayedAction { get; set; }

	// ==========================================================

	// Delay between each character getting printed.
	public float CharacterDelay = 0f;

	// A delay that is added alongside character delay.
	public float AdditiveCharacterDelay = 0f;

	// Default color of the text.
	public Color DefaultTextColor = Color.White;

	// A collection of sounds that will be played when a character is printed.
	// Selected randomly.
	public List<SoundFile> Sounds = new();

	// ==========================================================

	public SimpleDialoguePanel()
	{
		AcceptsFocus = true;
		Focus();

		DisplayContainer = new Panel();
		DisplayContainer.Parent = this;
		DisplayContainer.AddClass( "DisplayContainer" );

		if ( PrintStyle == DialoguePrintStyle.AllAtOnce )
		{
			CharacterDelay = 0;
		}
	}

	/// <summary>
	/// Special constructor to automatically assign dialogue for tests.
	/// </summary>
	/// <param name="dialogue"></param>
	public SimpleDialoguePanel( params string[] dialogue ) : this()
	{
		DialogueStrings = dialogue.ToList();
		ActiveDialogue = DialogueStrings.First();
	}

	/// <summary>
	/// Special function for unit tests.
	/// </summary>
	public void InitalizeUnitTest()
	{
		if ( !Application.IsUnitTest ) return;

		ClearDialogue();
		ActiveDialogue = DialogueStrings.First();
		CreateNewParagraphLabel();
	}

	/// <summary>
	/// Creates a new label set to the paragraph element. This needs to be set to paragraph
	/// instead of using label to ensure compatibility with the span element.
	/// </summary>
	/// <returns></returns>
	private Label CreateNewParagraphLabel()
	{
		var label = new Label();
		
		label.ElementName = "p";
		label.Parent = DisplayContainer;
		label.Style.FontColor = DefaultTextColor;

		Labels.Add( label );

		return label;
	}

	private void StyleLabel( Label label, string style, string data )
	{
		switch ( style )
		{
			// Basic styling commands.
			case "color": label.Style.FontColor = Color.Parse( data ); break;
			case "font": label.Style.FontFamily = data; break;
			case "size": label.Style.FontSize = Length.Parse( data ); break;

			default:
			{
				throw new ArgumentException( $"Style \"{style}\" not supported." );
			}
		}
	}

	// Control code formatting: [[{cmd}:{data}]]
	// Control codes can be split with a ; to execute multiple commands on one span.

	// Valid control codes:
	//		Change text color: [[color:red]]
	//		Change text font: [[font:Arial]]
	//		Change text size: [[size:50]]
	//		Delay text printing: [[wait:5]]
	private void ExecuteControlCode( string code )
	{
		var label = Labels.Where( l => l.ElementName == "p" ).Last();
		
		// If we have multiple commands, split them up.
		var commands = code.Split( ';' );
		foreach ( var command in commands )
		{
			var split = code.Split( ':' );

			// Some commands don't require a data value.
			if ( split.Length == 1 )
			{
				var key = split[0];
				switch ( key )
				{
					case "break":
					case "reset":
					case "b":
					case "r":
					{
						CreateNewParagraphLabel();
						continue;
					}
				}
			}
			else if ( split.Length >= 2 )
			{
				var key = split[0];
				var data = split[1];

				// Special case condition for the wait control code.
				if ( key == "wait" )
				{
					AdditiveCharacterDelay += float.Parse( data );
					continue;
				}
				if ( key == "delay" )
				{
					CharacterDelay = float.Parse( data );
					continue;
				}

				var prevLabel = Labels.Where( l => l.ElementName == "p" ).Last();
				label = CreateNewParagraphLabel();

				label.ElementName = "span";
				label.Parent = prevLabel;

				StyleLabel( label, key, data );
			}
		}
	}

	/// <summary>
	/// Takes in a character, and checks to see if we're executing a control code.
	/// </summary>
	/// <param name="character">Incoming character.</param>
	/// <param name="index">Index of the incoming character from the dialogue string.</param>
	private void ParseCharacter( char character, int index )
	{
		switch ( character )
		{
			case '[':
			{
				// If the next character in this string is also a '[', we have entered
				// into a control code sequence.
				if ( index + 1 >= ActiveDialogue.Count() ) break;

				var nextChar = ActiveDialogue.ElementAt( index + 1 );
				if ( nextChar == '[')
				{
					// We are now in a control code. Find the next two closing brackets characters ']]'
					// so we can close the control code and set the rendering index to be afterwards.
					var subStr = ActiveDialogue.Substring( index + 2 );
					var endIndex = subStr.IndexOf( "]]" );

					ExecuteControlCode( subStr.Substring( 0, endIndex ) );
					Index += endIndex + 4; // 2 for '[[', plus 2 for ']]'

					CallNextCharacterParse();
					return;
				}

				break;
			}
		}

		Index++;
		ActiveLabel.Text += character;

		// If we are executing this function as part of a unit test,
		// we do not need to call anything else related to characters,
		// and CallNextCharacterParse() will be called manually
		// from the test code.
		if ( Application.IsUnitTest ) return;

		OnCharacterDisplayed( character, index );
		CallNextCharacterParse();
	}

	/// <summary>
	/// Initalizes the panel on creation.
	/// </summary>
	/// <param name="firstTime"></param>
	protected override void OnAfterTreeRender( bool firstTime )
	{
		base.OnAfterTreeRender( firstTime );
		if ( firstTime )
		{
			ClearDialogue();
			ActiveDialogue = DialogueStrings.First();
			CreateNewParagraphLabel();

			switch ( PrintStyle )
			{
				// Call the first character print.
				case DialoguePrintStyle.CharacterByCharacter:
				{
					CallNextCharacterParse();
					break;
				}
				case DialoguePrintStyle.AllAtOnce:
				{
					ParseAllAtOnce();
					PlayRandomSound();
					break;
				}
			}
		}
	}

	/// <summary>
	/// Called when a character is displayed on the screen.
	/// </summary>
	private void OnCharacterDisplayed( char character, int index )
	{
		// Reset additive delay. It is only present for one character.
		AdditiveCharacterDelay = 0;

		// Do not play a sound on space characters.
		if ( character != ' ' && Sounds.Any() && PrintStyle == DialoguePrintStyle.CharacterByCharacter )
		{
			PlayRandomSound();
		}

		OnCharacterPrintedAction?.Invoke( this, index, character.ToString() );
	}

	/// <summary>
	/// Plays a random dialogue sound.
	/// </summary>
	private void PlayRandomSound()
	{
		var sound = Random.Shared.FromList( Sounds );
		// Sanity null check because a user can put an empty sound file into the list
		// and throw a NRE, which stops character printing execution.
		if ( sound is not null && sound.IsValid )
		{
			Sound.PlayFile( Random.Shared.FromList( Sounds ) );
		}
	}

	/// <summary>
	/// Resets dialogue related variables.
	/// </summary>
	private void ClearDialogue()
	{
		foreach ( var label in Labels )
		{
			label.Delete();
		}
		Labels.Clear();

		Index = 0;
		AdditiveCharacterDelay = 0;
		ActiveDialogue = string.Empty;
		CancelInvoke( "ParseCharacter" );
	}

	/// <summary>
	/// Duh.
	/// </summary>
	private void AdvanceDialogue()
	{
		ActiveDialogueIndex++;
		if ( ActiveDialogueIndex < DialogueStrings.Count() )
		{
			ActiveDialogue = DialogueStrings[ActiveDialogueIndex];
		}
	}

	/// <summary>
	/// Calls InvokeOnce with a bow on top.
	/// </summary>
	public void CallNextCharacterParse()
	{
		// Start drawing.
		var delay = SpeedUp ? CharacterDelay - DelayAmountRemoved : CharacterDelay;
		InvokeOnce( "ParseCharacter", delay + AdditiveCharacterDelay, () =>
		{
			//Log.Info( $"parse - index: {Index}, dialogueindex: {ActiveDialogueIndex}, str: {DialogueStrings[ActiveDialogueIndex]}" );
			if ( Index < ActiveDialogue.Length )
			{
				if ( Game.IsPaused ) return;
				ParseCharacter( ActiveDialogue[Index], Index );
			}
		} );
	}

	/// <summary>
	/// Parses all of the dialogue at once.
	/// </summary>
	private void ParseAllAtOnce()
	{
		// Don't know if a while loop is the safest here, but it's here!
		while ( Index < ActiveDialogue.Length )
		{
			ParseCharacter( ActiveDialogue[Index], Index );
		}
	}

	/// <summary>
	/// Input handling for advancing dialogue.
	/// </summary>
	/// <param name="e"></param>
	public override void OnButtonEvent( ButtonEvent e )
	{
		base.OnButtonEvent( e );
		Log.Info( e.Button );

		if ( IgnoreUserInput ) return;

		if ( e.Button == AdvanceCharacterKey && e.Pressed )
		{
			ClearDialogue();
			CreateNewParagraphLabel();
			AdvanceDialogue();

			switch ( PrintStyle )
			{
				case DialoguePrintStyle.CharacterByCharacter:
				{
					CallNextCharacterParse();
					break;
				}
				case DialoguePrintStyle.AllAtOnce:
				{
					ParseAllAtOnce();
					PlayRandomSound();
					break;
				}
			}
		}
		SpeedUp = (e.Button == SpeedUpKey);
	}

	[ActionGraphNode( "simpledialogue.newpanel" )]
	[Title( "Create Dialogue Panel" ), Group( "Simple Dialogue" ), Icon( "exposure_plus_1" )]
	public static SimpleDialoguePanel CreateNewDialoguePanel()
	{
		return new SimpleDialoguePanel();
	}
}
