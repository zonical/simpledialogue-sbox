using Editor;
using Sandbox;

namespace SimpleDialogue;

[EditorApp( "Dialogue Editor", "pregnant_woman", "Inspect Butts" )]
public class DialogueEditorApp : Window
{
	public DialogueEditorApp()
	{
		WindowTitle = "Hello";
		MinimumSize = new Vector2( 1000, 700 );
		Size = new Vector2( 1000, 700 );

		var widget = new DialogueDisplayWidget();
		widget.Parent = this;
		widget.Position = new Vector2( 0, 0 );
	}
}
