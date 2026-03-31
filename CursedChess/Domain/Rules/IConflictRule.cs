using CursedChess.Domain.Entities;

namespace CursedChess.Domain.Rules;

/// <summary>
/// Правило конфликта между позициями агентов.
/// </summary>
public interface IConflictRule
{
    /// <summary>
    /// Ключ правила (идентификатор типа конфликта).
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Проверяет, конфликтует ли новая позиция агента с уже размещёнными.
    /// </summary>
    /// <param name="newAgentPosition">Новая позиция агента.</param>
    /// <param name="existingAgentPositions">Уже размещённые позиции агентов.</param>
    /// <returns>`true`, если есть конфликт; иначе `false`.</returns>
    bool HasConflict(Position newAgentPosition, IReadOnlyCollection<Position> existingAgentPositions);
}

