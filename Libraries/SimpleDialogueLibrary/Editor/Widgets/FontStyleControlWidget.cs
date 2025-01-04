using Editor;
using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;
namespace SimpleDialogue;

[CustomEditor( typeof( FontStyle ) )]
public class FontStyleWidget : DropdownControlWidget<LengthUnit>
{
	public FontStyleWidget( SerializedProperty property ) : base( property )
	{
	}

	protected override IEnumerable<object> GetDropdownValues()
	{
		return Enum.GetNames(typeof(FontStyle));
	}
}
