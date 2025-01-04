using Editor;
using Editor.NodeEditor;
using SimpleDialogue;
public class DialogueGraphView : GraphView
{
	public DialogueGraphResource Resource { get; protected set; }

	public DialogueGraphView() : base (null)
	{
		Name = "Default";
		WindowTitle = "Default";
	}

	public DialogueGraphView( DialogueGraphResource resource) : base( null )
	{
		Resource = resource;
		Name = resource.ResourceName;
		WindowTitle = resource.ResourceName;
	}

	[EditorForAssetType( "dgraph" )]
	public static DialogueGraphView Open( Asset asset )
	{
		var resource = asset.LoadResource<DialogueGraphResource>();
		var path = resource.ResourcePath;

		var window = new MainWindow( EditorWindow );
		var view = new DialogueGraphView( resource );

		window.SetView( view );
		window.Show();

		var node = new BaseNode();


		// Add new view
		return view;
	}

	protected override void OnPaint()
	{
		base.OnPaint();

		// Stolen directly from ActionGraph. ;) - Make a nice background
		{
			var pixmap = new Pixmap( (int)GridSize, (int)GridSize );
			pixmap.Clear( Theme.WindowBackground );
			using ( Paint.ToPixmap( pixmap ) )
			{
				var h = pixmap.Size * 0.5f;

				Paint.SetPen( Theme.WindowBackground.Lighten( 0.3f ) );
				Paint.DrawLine( 0, new Vector2( 0, pixmap.Height ) );
				Paint.DrawLine( 0, new Vector2( pixmap.Width, 0 ) );
			}

			SetBackgroundImage( pixmap );
		}
	}
}
