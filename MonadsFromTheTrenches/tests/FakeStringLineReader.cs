using System.Collections.Immutable;

namespace MonadsFromTheTrenches;

class FakeStringLineReader : IStringLineReader
{
    private readonly List<List<string>> fileContent = new();

    public void Add(List<string> oneLine)
    {
        fileContent.Add(item: oneLine);
    }

    public IEnumerable<IEnumerable<string>> Read()
    {
        var r = fileContent.Select(x => x.ToImmutableList()).ToImmutableList(); 
        return r;
    }
}