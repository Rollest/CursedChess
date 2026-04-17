using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CursedChess.Domain.Rules;

namespace CursedChess.Domain.Entities;

/// <summary>
/// Агента, размещённого на доске.
/// </summary>
public class Agent
{
    /// <summary>
    /// Уникальный идентификатор агента.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор доски, которой принадлежит агент.
    /// </summary>
    public int BoardId { get; set; }

    /// <summary>
    /// Индекс колонки, в которой находится агент.
    /// </summary>
    public int ColumnIndex { get; set; }

    [NotMapped]
    /// <summary>
    /// Текущая позиция агента на доске (используется только в логике, не мапится в БД).
    /// </summary>
    public Position? CurrentPosition { get; set; }

    /// <summary>
    /// Указывает, зафиксирован ли агент на своей позиции.
    /// </summary>
    public bool IsFixed { get; set; }

    /// <summary>
    /// Приоритет агента в логической цепочке взаимодействия.
    /// </summary>
    public int Priority { get; set; }

    [NotMapped]
    /// <summary>
    /// Ключи правил конфликта для агента.
    /// </summary>
    public List<string> ConflictRuleKeys { get; set; } = new();

    [NotMapped]
    public Agent? PreviousAgent { get; set; }

    /// <summary>
    /// Рекурсивно проверяет подходит ли позиция агента-кандидата.
    /// </summary>
    /// <param name="candidatePosition">Позиция агента-кандидата.</param>
    /// <param name="currentPositions">Позиция проверяющего агента.</param>
    /// <param name="resolveRules">Правила проверки.</param>
    /// <returns>Истина, если позиция агента-кандидата удовлетворяет всех агентов в цепочке.</returns>
    public bool ApprovesCandidate(
        Position candidatePosition,
        Position?[] currentPositions,
        Func<Agent, IReadOnlyCollection<IConflictRule>> resolveRules)
    {
        var ownPosition = currentPositions[ColumnIndex];
        if (ownPosition is not null)
        {
            var rules = resolveRules(this);
            foreach (var rule in rules)
            {
                if (rule.HasConflict(candidatePosition, new[] { ownPosition.Value }))
                {
                    return false;
                }
            }
        }

        return PreviousAgent?.ApprovesCandidate(candidatePosition, currentPositions, resolveRules) ?? true;
    }
}