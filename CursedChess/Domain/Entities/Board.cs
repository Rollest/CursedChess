using System.Collections.Generic;

namespace CursedChess.Domain.Entities;

/// <summary>
/// Доска с клетками, агентами и фиксированными позициями.
/// </summary>
public class Board
{
    /// <summary>
    /// Уникальный идентификатор доски.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Размер доски (количество строк и колонок).
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Клетки доски с их стоимостью и цветом.
    /// </summary>
    public ICollection<Cell> Cells { get; set; } = new List<Cell>();

    /// <summary>
    /// Агенты, связанные с этой доской.
    /// </summary>
    public ICollection<Agent> Agents { get; set; } = new List<Agent>();

    /// <summary>
    /// Фиксированные позиции агентов для этой доски.
    /// </summary>
    public ICollection<FixedPosition> FixedPositions { get; set; } = new List<FixedPosition>();
}

