using CursedChess.Domain.Entities;

namespace CursedChess.Domain.Rules;

/// <summary>
/// Правило конфликта по строке (агенты не могут находиться в одной строке).
/// </summary>
public sealed class RowConflictRule : IConflictRule
{
    /// <summary>
    /// Ключ правила конфликта по строке.
    /// </summary>
    public string Key => "Row";

    /// <summary>
    /// Определяет, конфликтует ли новая позиция с уже размещёнными по строке.
    /// </summary>
    /// <param name="newAgentPosition">Новая позиция агента.</param>
    /// <param name="existingAgentPositions">Уже размещённые позиции агентов.</param>
    /// <returns>`true`, если строки совпадают; иначе `false`.</returns>
    public bool HasConflict(Position newAgentPosition, IReadOnlyCollection<Position> existingAgentPositions)
    {
        foreach (var pos in existingAgentPositions)
        {
            if (pos.Row == newAgentPosition.Row)
            {
                return true;
            }
        }

        return false;
    }
}

