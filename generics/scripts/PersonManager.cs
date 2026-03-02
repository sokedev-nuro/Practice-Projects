using Godot;

public partial class PersonManager : Node
{
    // public override void _Ready()
    // {
    //     Person<int> bob = new Person<int>(123, "Bob");
    //     Person<string> tom = new Person<string>("id3", "Tom");

    //     GD.Print($"Person Id: {bob.Id}, Person Name: {bob.Name}");
    //     GD.Print($"Person Id: {tom.Id}, Person Name: {tom.Name}");
    // }

    public override void _Ready()
    {
        for (int i = 1, j = 1; i < 10; i++, j++)
            GD.Print($"{i * j}");
    }
}
