namespace SharpQuest.Contracts;

public interface IItem
{
    string Id { get; }

    string DisplayName { get; }
}

public interface IItemStore
{
    int Count { get; }

    void Add(IItem item);

    IItem? FindById(string id);

    IReadOnlyList<IItem> All();

    bool Remove(string id);
}

public interface IItemMapper
{
    string ToJson(IItem item);

    IItem? FromJson(string json);
}

public interface ILab02Wiring
{
    IItemStore CreateStore();

    IItemMapper CreateMapper();

    IItem CreateSample(int seed);
}
