using Editor;
using Sandbox;
using static Sandbox.Gizmo;

namespace SimpleDialogue;

// Supply the file extension of the asset, cannot be more than 8 characters
[EditorForAssetType( "dps" )]
public class DialogueStyleEditor : Window, IAssetEditor
{
	// Return false if you want the have a Widget created for each Asset opened,
	// Return true if you want only one Widget to be made, calling AssetOpen on the open Widget
	public bool CanOpenMultipleAssets => false;

	Asset MyAsset;
	DialoguePanelStyle Resource;
	DialogueDisplayWidget Display;

	public DialogueStyleEditor()
	{
		// Cannot modify the Layout of a Window, instead we have a Canvas Widget
		Canvas = new Widget( null );
		Canvas.Layout = Layout.Column();
		Canvas.Layout.Spacing = 8;
		Canvas.Layout.Margin = 8;

		Show();
	}

	public void BuildUI()
	{
		var toolBar = Canvas.Layout.Add( new ToolBar( this ) );
		toolBar.AddOption( "Save File", "save", SaveAsset );
		//toolBar.AddOption( "Reset Panel", "restart_alt", OpenConfirmResetBox );

		toolBar.Size = new Vector2( FixedSize.x, 16 );

		Display = Canvas.Layout.Add( new DialogueDisplayWidget() { Resource = Resource } );

		var textEditor = new StringControlWidget( Display.DialogueTestLabel.GetSerialized().GetProperty("Text") );
		Canvas.Layout.Add( textEditor );

		var controlSheet = new ControlSheet();
		controlSheet.AddObject( Resource.GetSerialized() );
		Canvas.Layout.Add( controlSheet );
	}

	public void AssetOpen( Asset asset )
	{
		// Get the Resource from the Asset, from here you can get whatever info you want
		MyAsset = asset;
		Resource = MyAsset.LoadResource<DialoguePanelStyle>();
		
		BuildUI();
		Focus();

		WindowTitle = $"Dialogue Editor - {Resource.ResourcePath}";
	}

	public void OpenConfirmResetBox()
	{
	}


	// From IAssetEditor
	public void SelectMember( string memberName ) { }

	void SaveAsset()
	{
		// If we modify the Resource at all, we can save those changes with SaveToDisk
		MyAsset.SaveToDisk( Resource );
	}
}
