using Godot;
using System;

namespace Generics
{
    public partial class GameManager : Node
    {
        public override void _Ready()
        {
            Camera2D cam = this.FindChild<Camera2D>();
            GD.Print($"Camera: {cam}");

            var enemies = GetTree().Root.FindAllChildren<Enemy>();
            GD.Print($"Found enemies: {enemies.Count}");
        }
    }
}