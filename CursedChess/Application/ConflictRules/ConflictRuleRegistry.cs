using CursedChess.Domain.Rules;

namespace CursedChess.Application.ConflictRules;

/// <summary>
/// Реестр встроенных правил конфликта по строковому ключу.
/// </summary>
public sealed class ConflictRuleRegistry : IConflictRuleRegistry
{
    private readonly IReadOnlyDictionary<string, IConflictRule> _rules;

    /// <summary>
    /// Создаёт реестр с правилами Row и Diagonal.
    /// </summary>
    public ConflictRuleRegistry()
    {
        IConflictRule[] all =
        [
            new RowConflictRule(),
            new DiagonalConflictRule()
        ];
        _rules = all.ToDictionary(r => r.Key, StringComparer.Ordinal);
    }

    /// <inheritdoc />
    public IReadOnlyList<IConflictRule> Resolve(IEnumerable<string> keys)
    {
        var list = new List<IConflictRule>();
        foreach (var key in keys)
        {
            if (_rules.TryGetValue(key, out var rule))
            {
                list.Add(rule);
            }
        }

        return list;
    }
}
