using Editor;
using Sandbox.Services;
using System.Collections.Generic;
using System.Linq;

public class MainWindow : DockWindow
{
	public MainWindow( Window parent )
	{
		Parent = parent;

		WindowTitle = "Hello";
		Size = new Vector2( 1280, 720 );
		DeleteOnClose = true;
		WindowFlags = WindowFlags.Dialog | WindowFlags.Customized | WindowFlags.CloseButton | WindowFlags.WindowSystemMenuHint | WindowFlags.WindowTitle | WindowFlags.MaximizeButton;

		// Add properties to dock.
		var properties = new Properties( this );
		DockManager.AddDock( null, properties, DockArea.Left, DockManager.DockProperty.HideCloseButton, 1.0f );
	}

	public void SetView( DialogueGraphView view )
	{
		DockManager.AddDock( null, view, DockArea.RightOuter, DockManager.DockProperty.HideCloseButton, 1.0f );
		DockManager.Update();
	}
}
