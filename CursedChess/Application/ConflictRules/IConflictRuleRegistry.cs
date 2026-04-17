using CursedChess.Domain.Rules;

namespace CursedChess.Application.ConflictRules;

/// <summary>
/// Сопоставляет ключи правил с экземплярами <see cref="IConflictRule"/>.
/// </summary>
public interface IConflictRuleRegistry
{
    /// <summary>
    /// Возвращает правила в порядке следования ключей; неизвестные ключи пропускаются.
    /// </summary>
    /// <param name="keys">Ключи правил, хранимые у агента.</param>
    /// <returns>Список разрешённых правил.</returns>
    IReadOnlyList<IConflictRule> Resolve(IEnumerable<string> keys);
}
