namespace CursedChess.Domain.Rules;

/// <summary>
/// Стандартные ключи правил конфликта, совпадающие с <see cref="IConflictRule.Key"/>.
/// </summary>
public static class KnownConflictRuleKeys
{
    /// <summary>
    /// Ключ правила конфликта по строке.
    /// </summary>
    public const string Row = "Row";

    /// <summary>
    /// Ключ правила конфликта по диагонали.
    /// </summary>
    public const string Diagonal = "Diagonal";

    /// <summary>
    /// Набор ключей по умолчанию для классической задачи N ферзей.
    /// </summary>
    public static IReadOnlyList<string> DefaultNQueens { get; } = new[] { Row, Diagonal };
}
