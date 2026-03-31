using System.Collections.Generic;

namespace CursedChess.Domain.Entities;

/// <summary>
/// Тип события, возникающего в процессе поиска решений.
/// </summary>
public enum SearchStepType
{
    /// <summary>
    /// Попытка размещения агента.
    /// </summary>
    TryPlace,
    /// <summary>
    /// Шаг проверки конфликтов.
    /// </summary>
    CheckConflicts,
    /// <summary>
    /// Обнаружение конфликта при размещении.
    /// </summary>
    ConflictFound,
    /// <summary>
    /// Успешное размещение агента.
    /// </summary>
    Placed,
    /// <summary>
    /// Откат (возврат) в процессе перебора.
    /// </summary>
    Backtrack,
    /// <summary>
    /// Найдено полное решение.
    /// </summary>
    SolutionFound
}

/// <summary>
/// Описывает единичный шаг трассировки поиска решений.
/// </summary>
public class SearchStep
{
    /// <summary>
    /// Индекс шага в трассировке.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Тип события шага.
    /// </summary>
    public SearchStepType Type { get; set; }

    /// <summary>
    /// Колонка, к которой относится активный агент.
    /// </summary>
    public int ActiveAgentColumn { get; set; }

    /// <summary>
    /// Номер строки размещения (или `null`, если неприменимо).
    /// </summary>
    public int? Row { get; set; }

    /// <summary>
    /// Номер колонки размещения (или `null`, если неприменимо).
    /// </summary>
    public int? Column { get; set; }

    /// <summary>
    /// Текстовый комментарий к шагу.
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Снимок позиций агентов на момент выполнения шага.
    /// </summary>
    public IReadOnlyCollection<Position> AgentPositionsSnapshot { get; set; } = new List<Position>();
}

