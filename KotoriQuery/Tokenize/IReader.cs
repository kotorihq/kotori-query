namespace KotoriQuery.Tokenize
{
    public interface IReader<TElement, TState> where TElement : struct {
        TState Start { get; }

        TElement? TryGet(ref TState state);
    }
}