using Godot;

namespace InterfacesTraining
{
    public partial class CircleController : Sprite2D, ISelectable
    {
        public void Select()
        {
            GD.Print("Circle Selected");
        }
    }
}