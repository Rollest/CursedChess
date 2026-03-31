using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
    /// Ключи правил конфликта для агента (резерв для будущей настройки).
    /// </summary>
    public ICollection<string> ConflictRuleKeys { get; set; } = new List<string>();
}

