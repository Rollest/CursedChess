namespace CursedChess.Domain.Entities;

/// <summary>
/// Фиксированная позиция агента на доске.
/// </summary>
public class FixedPosition
{
    /// <summary>
    /// Уникальный идентификатор записи фиксации.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор агента, которому соответствует фиксация.
    /// </summary>
    public int AgentId { get; set; }

    /// <summary>
    /// Идентификатор доски, на которой расположена фиксация.
    /// </summary>
    public int BoardId { get; set; }

    /// <summary>
    /// Номер строки фиксированной позиции.
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Номер колонки фиксированной позиции.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Агент, для которого задана фиксированная позиция.
    /// </summary>
    public Agent? Agent { get; set; }
}

