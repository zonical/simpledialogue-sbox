using Sandbox;
using Editor;
using System.Linq;

namespace SimpleDialogue;

public class DialogueDisplayWidget : Widget
{
	public DialoguePanelStyle Resource { get; set; }
	public Label DialogueTestLabel { get; set; }

	public DialogueDisplayWidget() : base( null )
	{
		FixedSize = 100;

		DialogueTestLabel = new Label(this);
		DialogueTestLabel.Text = 
			"Lorem ipsum dolor sit amet...";

		DialogueTestLabel.Alignment = TextFlag.LeftTop;
		DialogueTestLabel.WordWrap = true;
	}

	[EditorEvent.Frame]
	public void OnFrame()
	{
		var width = Resource.PanelWidth is not null ? (float)Resource.PanelWidth : 100.0f;
		var height = Resource.PanelHeight is not null ? (float)Resource.PanelHeight : 100.0f;
		FixedSize = new Vector2( width, height );

		var borderSize = Resource.BorderSize ?? 20.0f;
		var color = Resource.TextColor ?? Color.White;
		var textSize = (Resource.FontSize ?? 20);
		var font = Resource.FontFamily ?? "Poppins";

		DialogueTestLabel.Position = new Vector2( 20 + borderSize, 20 + borderSize );
		DialogueTestLabel.SetStyles( 
			$"font-family: \"{font}\"; " +
			$"font-size: {textSize}px; " +
			$"color: {color.Hex};" +
			$"white-space: nowrap;" );

		DialogueTestLabel.Size = new Vector2( width - borderSize - 60, height - borderSize - 60 );
	}

	protected override void OnPaint()
	{
		Paint.ClearBrush();
		Paint.ClearPen();

		// Border
		Paint.SetBrush( Resource.BorderColor ?? Color.White );
		Paint.DrawRect( new Rect( new Vector2( 0, 0 ), FixedSize ) );

		// Background inner color.
		var borderSize = Resource.BorderSize ?? 20.0f;
		Paint.SetBrush( Resource.BackgroundColor ?? Color.Black );
		Paint.DrawRect( new Rect( new Vector2( borderSize, borderSize ), FixedSize - ( borderSize * 2 ) ) );;
	}
}
