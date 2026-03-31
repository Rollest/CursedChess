using CursedChess.Domain.Entities;

namespace CursedChess.Domain.Rules;

/// <summary>
/// Правило конфликта по диагонали (агенты не могут стоять на одной диагонали).
/// </summary>
public sealed class DiagonalConflictRule : IConflictRule
{
    /// <summary>
    /// Ключ правила конфликта по диагонали.
    /// </summary>
    public string Key => "Diagonal";

    /// <summary>
    /// Определяет, конфликтует ли новая позиция с уже размещёнными по диагонали.
    /// </summary>
    /// <param name="newAgentPosition">Новая позиция агента.</param>
    /// <param name="existingAgentPositions">Уже размещённые позиции агентов.</param>
    /// <returns>`true`, если диагонали совпадают; иначе `false`.</returns>
    public bool HasConflict(Position newAgentPosition, IReadOnlyCollection<Position> existingAgentPositions)
    {
        foreach (var pos in existingAgentPositions)
        {
            if (Math.Abs(pos.Row - newAgentPosition.Row) == Math.Abs(pos.Column - newAgentPosition.Column))
            {
                return true;
            }
        }

        return false;
    }
}

