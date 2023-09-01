namespace PlatesOrganiser.Domain.Entities;

public class Label : IEntity
{
    public Label(string name)
    {
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<Plate> Plates { get; set; }
}
