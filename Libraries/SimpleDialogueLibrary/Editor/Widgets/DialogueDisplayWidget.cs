using Sandbox;
using Editor;
using System.Linq;
using SimpleDialoguePanelComponent = SimpleDialogue.SimpleDialoguePanelComponent;
using Editor.ShaderGraph.Nodes;

namespace SimpleDialogue;

public class DialogueDisplayWidget : Widget
{
	public Scene Scene;
	SceneRenderingWidget CanvasWidget;

	SimpleDialoguePanelComponent PanelComponent => Scene.GetAllComponents<SimpleDialoguePanelComponent>().First();
	WorldPanel WorldPanel => Scene.GetAllComponents<WorldPanel>().First();

	SceneFile File => ResourceLibrary.Get<SceneFile>( "scenes/debug.scene" );

	public DialogueDisplayWidget() : base( null )
	{
		CreateScene();

		Layout = Layout.Column();

		CanvasWidget = new SceneRenderingWidget( this );
		CanvasWidget.SetSizeMode( SizeMode.CanGrow, SizeMode.CanGrow );

		Layout.Add( CanvasWidget );

		FixedSize = new Vector2(1000, 500);
		SetSizeMode( SizeMode.CanGrow, SizeMode.CanGrow );
	}

	public override void OnDestroyed()
	{
		Scene.Destroy();
	}

	public void CreateScene()
	{
		Scene = new Scene();
		Scene.Load( File );

		using (Scene.Push())
		{
			var go = new GameObject( true, "PanelContainer" );
			var panel = go.AddComponent<ScreenPanel>();
			var comp = go.AddComponent<SimpleDialoguePanelComponent>();
			
			var camera = new GameObject( true, "Camera" ).AddComponent<CameraComponent>();
			camera.WorldPosition = new Vector3( 100, 0, 0 );
			camera.WorldRotation = new Angles( 0, -180, 0 );
			camera.Orthographic = true;
			camera.OrthographicHeight = 54;
			camera.IsMainCamera = true;
			camera.Tags.Add( "maincamera" );
		}
	}

	[EditorEvent.Frame]
	public void OnFrame()
	{
		CanvasWidget.Scene = Scene.Scene;
		Scene.EditorTick(RealTime.Now, RealTime.Delta);
	}

	[EditorEvent.Hotload]
	public void OnHotload()
	{
		Scene.Destroy();
		CreateScene();
	}

	protected override void OnPaint()
	{
	}
}
