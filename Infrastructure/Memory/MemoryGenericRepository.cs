using AppCore.Dto;
using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T> where T : EntityBase
{
    protected readonly Dictionary<Guid, T> _data = new();

    public Task<T?> FindByIdAsync(Guid id)
    {
        _data.TryGetValue(id, out var value);
        return Task.FromResult(value);
    }

    public Task<IEnumerable<T>> FindAllAsync()
        => Task.FromResult<IEnumerable<T>>(_data.Values.ToList());

    public Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 20;

        var items = _data.Values
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult(new PagedResult<T>(items, _data.Count, page, pageSize));
    }

    public Task<T> AddAsync(T entity)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        if (_data.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity with id {entity.Id} already exists.");

        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity)
    {
        if (entity.Id == Guid.Empty)
            throw new ArgumentException("Entity Id cannot be empty.", nameof(entity));

        if (!_data.ContainsKey(entity.Id))
            throw new KeyNotFoundException($"Entity with id {entity.Id} not found.");

        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task RemoveByIdAsync(Guid id)
    {
        if (!_data.Remove(id))
            throw new KeyNotFoundException($"Entity with id {id} not found.");

        return Task.CompletedTask;
    }
}