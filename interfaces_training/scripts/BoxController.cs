using Godot;

namespace InterfacesTraining
{
    public partial class BoxController : Sprite2D, ISelectable
    {
        public void Select()
        {
            GD.Print("Box Selected!");
        }
    }
}

