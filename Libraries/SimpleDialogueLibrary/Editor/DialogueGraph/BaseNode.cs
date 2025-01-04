using Editor;
using Editor.NodeEditor;
using Sandbox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

public class BaseNode : INode
{
	public event Action Changed;

	[JsonIgnore, Hide, Browsable( false )]
	public string Identifier { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public DisplayInfo DisplayInfo { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public bool CanClone { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public bool CanRemove { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public Vector2 Position { get; set; }
	[JsonIgnore, Hide, Browsable( false )]
	public Vector2 ExpandSize { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public bool AutoSize { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public IEnumerable<IPlugIn> Inputs { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public IEnumerable<IPlugOut> Outputs { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public string? ErrorMessage { get; }
	[JsonIgnore, Hide, Browsable( false )]
	public bool IsReachable { get; }

	public Pixmap? Thumbnail { get; }
	public void OnPaint( Rect rect )
	{

	}
	public void OnDoubleClick( MouseEvent e )
	{

	}
	public bool HasTitleBar { get; }

	public NodeUI CreateUI( GraphView view )
	{
		return new NodeUI( view, this );
	}
	public Color GetPrimaryColor( GraphView view )
	{
		return Color.Green;
	}
	public Menu? CreateContextMenu( NodeUI node )
	{
		var menu = new Menu( "Dialogue Menu" );
		return menu;
	}
}
