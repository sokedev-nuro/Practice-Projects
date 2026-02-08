using Godot;

namespace InterfacesTraining
{
    public partial class SelectionManager : Node2D
    {
        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent
                && mouseEvent.ButtonIndex == MouseButton.Left
                && mouseEvent.Pressed)
            {
                // Преобразуем экранные координаты в мировые
                Vector2 mouseWorldPosition = GetCanvasTransform().AffineInverse() * mouseEvent.Position;

                GD.Print($"[Debug] Клик в мировых координатах: {mouseWorldPosition}");

                CheckSelection(mouseWorldPosition);
            }
        }

        private void CheckSelection(Vector2 mousePosition)
        {
            var spaceState = GetWorld2D().DirectSpaceState;

            var query = new PhysicsPointQueryParameters2D
            {
                Position = mousePosition,
                CollideWithAreas = true,
                CollideWithBodies = false
            };

            var results = spaceState.IntersectPoint(query);

            GD.Print($"[Debug] Найдено пересечений: {results.Count}");

            foreach (var result in results)
            {
                GD.Print($"[Debug] Collider: {result["collider"].AsGodotObject()}");

                if (result["collider"].AsGodotObject() is ISelectable selectable)
                {
                    selectable.OnSelect();
                }
            }
        }
    }
}