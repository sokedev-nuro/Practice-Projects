using Godot;

namespace InterfacesTraining
{
    public partial class CircleController : Area2D, ISelectable
    {
        public void OnSelect()
        {
            GD.Print($"[Box] Selected: {Name}, position: {GlobalPosition}");
        }
    }
}