using AppCore.Interfaces;
using AppCore.Models;
using Infrastructure.Memory;
using Xunit;

public class MemoryGenericRepositoryTest
{
    private readonly IGenericRepositoryAsync<Person> _repo = new MemoryGenericRepository<Person>();

    [Fact]
    public async Task AddPersonTestAsync()
    {
        var expected = new Person { FirstName = "Adam", LastName = "Nowak" };
        await _repo.AddAsync(expected);
        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual!.Id);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnAll()
    {
        await _repo.AddAsync(new Person { FirstName = "A", LastName = "1" });
        await _repo.AddAsync(new Person { FirstName = "B", LastName = "2" });

        var all = (await _repo.FindAllAsync()).ToList();
        Assert.True(all.Count >= 2);
    }

    [Fact]
    public async Task FindPagedAsync_ShouldReturnCorrectPage()
    {
        for (int i = 0; i < 25; i++)
            await _repo.AddAsync(new Person { FirstName = "P", LastName = i.ToString() });

        var page1 = await _repo.FindPagedAsync(1, 10);
        var page2 = await _repo.FindPagedAsync(2, 10);

        Assert.Equal(10, page1.Items.Count);
        Assert.Equal(10, page2.Items.Count);
        Assert.Equal(25, page1.TotalCount);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        var person = new Person { FirstName = "Adam", LastName = "Nowak" };
        await _repo.AddAsync(person);

        person.FirstName = "Ewa";
        await _repo.UpdateAsync(person);

        var actual = await _repo.FindByIdAsync(person.Id);
        Assert.Equal("Ewa", actual!.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_WhenMissing_ShouldThrow()
    {
        var person = new Person { FirstName = "X", LastName = "Y" };
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.UpdateAsync(person));
    }

    [Fact]
    public async Task RemoveByIdAsync_ShouldRemove()
    {
        var person = new Person { FirstName = "Jan", LastName = "Kowalski" };
        await _repo.AddAsync(person);

        await _repo.RemoveByIdAsync(person.Id);

        var actual = await _repo.FindByIdAsync(person.Id);
        Assert.Null(actual);
    }

    [Fact]
    public async Task RemoveByIdAsync_WhenMissing_ShouldThrow()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.RemoveByIdAsync(Guid.NewGuid()));
    }
}