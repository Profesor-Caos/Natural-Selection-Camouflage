using Godot;
using System;

public class GridBox : PanelContainer
{
    private Viewport _viewport;
    public Viewport Viewport
    {
        get 
        { 
            if (_viewport == null)
                _viewport = GetNode<Viewport>($"{nameof(VBoxContainer)}/{nameof(HBoxContainer)}/{nameof(ViewportContainer)}/{nameof(Viewport)}");
            return _viewport; 
        }
    }

    private int _viewportWidth;
    [Export]
    public int ViewportWidth
    {
        get { return _viewportWidth; }
        set 
        { 
            _viewportWidth = value;
            Viewport.Size = new Vector2 ( _viewportWidth, Viewport.Size.y );
        }
    }

    private int _viewportHeight;
    [Export]
    public int ViewportHeight
    {
        get { return _viewportHeight; }
        set
        {
            _viewportHeight = value;
            Viewport.Size = new Vector2(Viewport.Size.x, _viewportHeight);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
