using Microsoft.EntityFrameworkCore;
using PlatesOrganiser.Domain.Entities;
using PlatesOrganiser.Domain.Repositories;
using PlatesOrganiser.Infrastructure.Context;

namespace PlatesOrganiser.Infrastructure.Repositories;

internal class LabelRepository : ILabelRepository
{
    private readonly PlatesContext _context;

    public LabelRepository(PlatesContext context)
    {
        _context = context;
    }

    public void AddLabel(Label label)
    {
        _context.Labels.Add(label);
    }

    public async Task<Label?> GetLabelByName(string name)
    {
        return await _context.Labels.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IEnumerable<Label>> GetLabelsByName(string[] names)
    {
        return await _context.Labels.Join(names, l => l.Name, n => n, (l, n) => l).ToListAsync();
    }
}
