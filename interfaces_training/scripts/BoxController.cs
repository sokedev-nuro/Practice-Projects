using Godot;

namespace InterfacesTraining
{
    public partial class BoxController : Area2D, ISelectable
    {
        public void OnSelect()
        {
            GD.Print($"[Box] Selected: {Name}, position: {GlobalPosition}");
        }
    }
}