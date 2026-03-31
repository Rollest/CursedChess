namespace CursedChess.Domain.Entities;

/// <summary>
/// Клетка доски с координатами, стоимостью и цветом.
/// </summary>
public class Cell
{
    /// <summary>
    /// Уникальный идентификатор клетки.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор доски, которой принадлежит клетка.
    /// </summary>
    public int BoardId { get; set; }

    /// <summary>
    /// Номер строки клетки.
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Номер колонки клетки.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Стоимость размещения агента в этой клетке.
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// Цвет клетки (используется для проверки согласованности решений).
    /// </summary>
    public string Color { get; set; } = "#FFFFFF";

    /// <summary>
    /// Указывает, занята ли клетка агентом.
    /// </summary>
    public bool IsOccupied { get; set; }
}

