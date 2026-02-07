using Godot;

namespace InterfacesTraining
{
    public partial class SelectionManager : Node2D
    {
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseButton)
            {
                if (mouseButton.Pressed)
                {
                    
                }
            }
        }
    }
}