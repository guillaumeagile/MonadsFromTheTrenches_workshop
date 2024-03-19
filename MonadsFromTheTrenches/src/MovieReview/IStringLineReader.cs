namespace MonadsFromTheTrenches;

public interface IStringLineReader
{
    public IEnumerable<IEnumerable<string>> Read();

}