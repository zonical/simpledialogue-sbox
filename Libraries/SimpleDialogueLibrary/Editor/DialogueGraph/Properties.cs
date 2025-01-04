using Editor;
using Sandbox;

public class Properties : Widget
{
	public MainWindow MainWindow { get; }

	public Properties( MainWindow mainWindow ) : base( null )
	{
		MainWindow = mainWindow;

		Name = "Properties";
		WindowTitle = "Properties";
		
		SetWindowIcon( "manage_search" );
		SetSizeMode( SizeMode.Default, SizeMode.CanShrink );
	}
}
