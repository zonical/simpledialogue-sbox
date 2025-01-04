using Sandbox;
using Sandbox.UI;

namespace SimpleDialogue;

public interface IDialoguePanel
{
	/// <summary>
	/// Fired when a character is displayed on the dialogue panel.
	/// </summary>
	/// <param name="PanelRef">Reference to the panel.</param>
	/// <param name="CharIndex">Index of the character in the raw dialogue string (including control codes)</param>
	/// <param name="Character">Character printed.</param>
	public delegate void OnCharacterPrinted( SimpleDialoguePanel PanelRef, int CharIndex, string Character );

	/// <summary>
	/// Fired when a control code is hit during parsing. This is called AFTER built-in control codes.
	/// </summary>
	/// <param name="PanelRef">Reference to the panel.</param>
	/// <param name="NewLabel">The new label that has been assigned with this control code.</param>
	/// <param name="ControlCode">The control code hit.</param>
	/// <param name="Data">Data from the control code.</param>
	public delegate void OnControlCodeHit( SimpleDialoguePanel PanelRef, Label NewLabel, string ControlCode, string Data );

	/// <summary>
	/// Fired when all of the characters in a dialogue string have been printed.
	/// </summary>
	/// <param name="PanelRef">Reference to the panel.</param>
	/// <param name="DialogueIndex">Index of the dialogue string.</param>
	/// <param name="Dialogue">Displayed dialogue string (excluding control codes).</param>
	public delegate void OnAllCharactersPrinted( SimpleDialoguePanel PanelRef, int DialogueIndex, string Dialogue );

	/// <summary>
	/// Fired when all of the dialogue has been displayed.
	/// </summary>
	/// <param name="PanelRef">Reference to the panel.</param>
	public delegate void OnAllDialogueDisplayed( SimpleDialoguePanel PanelRef );
}
