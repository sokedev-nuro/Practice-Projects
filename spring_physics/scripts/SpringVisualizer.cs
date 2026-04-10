using Godot;
using System;

public partial class SpringVisualizer : Node2D
{
    [Signal] public delegate void CompressionChangedEventHandler(float compressionRatio);

    // Constants
    private const int POINTS_PER_COIL = 20;
    private const float MIN_LENGTH = 20f;

    // Export groups
    [ExportGroup("Spring Visual")]
    [Export] private float _restLength = 200f;
    [Export] private float _coilAmplitude = 30f;
    [Export] private int _coilCount = 8;
    [Export] private float _lineWidth = 3f;
    [Export] private Color _springColor = new Color(0.2f, 1f, 0.4f);

    [ExportGroup("Physics")]
    [Export] private float _springStiffness = 150f;
    [Export] private float _dampingCoefficient = 8f;
    [Export] private float _mass = 1f;

    [ExportGroup("Mass Visual")]
    [Export] private float _massRadius = 15f;
    [Export] private Color _massColor = new Color(1f, 0.4f, 0.1f);
    [Export] private Color _anchorColor = Colors.White;

    // Private fields
    private float _currentLength;
    private float _velocity;
    private bool _isDragging;
    private float _dragOffsetY;
    private float _previousCompressionRatio;

    // Properties
    /// <summary>Current compression ratio: 1.0 = rest, below 1.0 = compressed, above 1.0 = extended.</summary>
    public float CompressionRatio => _currentLength / _restLength;

    // Built-in methods

    public override void _Ready()
    {
        _currentLength = _restLength;
        _previousCompressionRatio = CompressionRatio;

        GD.Print(Mathf.Pow(100, 0));
    }

    public override void _Process(double delta)
    {
        if (!_isDragging)
            SimulateSpring((float)delta);

        if (!Mathf.IsEqualApprox(CompressionRatio, _previousCompressionRatio))
        {
            EmitSignal(SignalName.CompressionChanged, CompressionRatio);
            _previousCompressionRatio = CompressionRatio;
        }

        QueueRedraw();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
        {
            if (mouseButton.Pressed)
                TryStartDrag(GetLocalMousePosition());
            else
                StopDrag();
        }
        else if (@event is InputEventMouseMotion && _isDragging)
        {
            _currentLength = Mathf.Max(MIN_LENGTH, GetLocalMousePosition().Y + _dragOffsetY);
            _velocity = 0f;
        }
    }

    public override void _Draw()
    {
        DrawAnchor();
        DrawSpringCoils();
        DrawMass();
    }

    // Private methods

    private void SimulateSpring(float delta)
    {
        float displacement = _currentLength - _restLength;
        float springForce = -_springStiffness * displacement;
        float dampingForce = -_dampingCoefficient * _velocity;
        float acceleration = (springForce + dampingForce) / _mass;

        _velocity += acceleration * delta;
        _currentLength = Mathf.Max(MIN_LENGTH, _currentLength + _velocity * delta);
    }

    private void TryStartDrag(Vector2 localPos)
    {
        Vector2 massCenter = new Vector2(0f, _currentLength);
        if (localPos.DistanceTo(massCenter) <= _massRadius * 2f)
        {
            _isDragging = true;
            _dragOffsetY = massCenter.Y - localPos.Y;
        }
    }

    private void StopDrag()
    {
        _isDragging = false;
    }

    private void DrawAnchor()
    {
        const float AnchorSize = 14f;
        DrawRect(new Rect2(-AnchorSize / 2f, -AnchorSize / 2f, AnchorSize, AnchorSize), _anchorColor);
    }

    private void DrawSpringCoils()
    {
        int totalPoints = _coilCount * POINTS_PER_COIL + 1;
        Vector2[] points = new Vector2[totalPoints];

        for (int i = 0; i < totalPoints; i++)
        {
            float t = (float)i / (totalPoints - 1);
            float x = Mathf.Sin(t * _coilCount * Mathf.Tau) * _coilAmplitude;
            float y = t * _currentLength;
            points[i] = new Vector2(x, y);
        }

        DrawPolyline(points, _springColor, _lineWidth, true);
    }

    private void DrawMass()
    {
        Vector2 massCenter = new Vector2(0f, _currentLength);
        DrawCircle(massCenter, _massRadius, _massColor);

        if (_isDragging)
            DrawArc(massCenter, _massRadius + 4f, 0f, Mathf.Tau, 32, Colors.White, 2f, true);
    }
}
