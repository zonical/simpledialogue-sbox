using Sandbox;
using Sandbox.UI;

namespace SimpleDialogue;

[GameResource("Dialogue Panel Style", "dps", "Specifies the style of a dialogue panel.")]
public class DialoguePanelStyle : GameResource
{
	[Group( "Panel Dimensions" ),
	Description( "Sets the width of the panel in pixels. Use CSS for further control (e.g percentage)." )]
	public float? PanelWidth { get; set; }

	[Group( "Panel Dimensions" ),
	Description( "Sets the height of the panel in pixels. Use CSS for further control (e.g percentage)." )]
	public float? PanelHeight { get; set; }

	[Group( "Panel Positioning" ),
	Description( "Similar to CSS \"left\" property." )]
	public float? LeftOffset { get; set; }

	[Group( "Panel Positioning" ),
	Description( "Similar to CSS \"right\" property." )]
	public float? RightOffset { get; set; }

	[Group( "Panel Positioning" ),
	Description( "Similar to CSS \"top\" property." )]
	public float? TopOffset { get; set; }

	[Group( "Panel Positioning" ),
	Description( "Similar to CSS \"bottom\" property." )]
	public float? BottomOffset { get; set; }

	[Group( "Background" )]
	public Color? BackgroundColor { get; set; }

	[Group( "Border" )]
	public Color? BorderColor { get; set; }

	[Group( "Border" ),
	Description( "Sets the thickness of the border in pixels. Use CSS for further control (e.g percentage)." )]
	public float? BorderSize { get; set; }

	[Group( "Border" )]
	public float? BorderCornerRadius { get; set; }

	[Group( "Text Display" ), FontName]
	public string? FontFamily { get; set; }

	[Group( "Text Display" ),
	Description( "Sets the size of the text. Use CSS for further control (e.g percentage)." )]
	public float? FontSize { get; set; }

	[Group( "Text Display" )]
	public FontStyle? FontStyle { get; set; }

	[Group( "Text Display" )]
	public Color? TextColor { get; set; } = Color.White;

	[Hide]
	public void BuildStyle(Panel RootPanel, Panel DialoguePanel)
	{
		if ( PanelWidth is not null ) DialoguePanel.Style.Width = new Length() { Unit = LengthUnit.Pixels, Value = PanelWidth.Value };
		if ( PanelHeight is not null ) DialoguePanel.Style.Height = new Length() { Unit = LengthUnit.Pixels, Value = PanelHeight.Value };

		if ( LeftOffset is not null ) RootPanel.Style.Left = new Length() { Unit = LengthUnit.Pixels, Value = LeftOffset.Value };
		if ( RightOffset is not null ) RootPanel.Style.Right = new Length() { Unit = LengthUnit.Pixels, Value = RightOffset.Value };
		if ( TopOffset is not null ) RootPanel.Style.Top = new Length() { Unit = LengthUnit.Pixels, Value = TopOffset.Value };
		if ( BottomOffset is not null ) RootPanel.Style.Bottom = new Length() { Unit = LengthUnit.Pixels, Value = BottomOffset.Value };

		if ( BackgroundColor is not null ) DialoguePanel.Style.BackgroundColor = BackgroundColor;

		if ( BorderSize is not null ) DialoguePanel.Style.BorderWidth = new Length() { Unit = LengthUnit.Pixels, Value = BorderSize.Value };
		if ( BorderColor is not null ) DialoguePanel.Style.BorderColor = BorderColor;
		if ( BorderCornerRadius is not null ) DialoguePanel.Style.Set( "border-radius", BorderCornerRadius.ToString() );

		if ( FontFamily is not null ) DialoguePanel.Style.FontFamily = FontFamily;
		if ( FontSize is not null ) DialoguePanel.Style.FontSize = new Length() { Unit = LengthUnit.Pixels, Value = FontSize.Value };
	}
}
