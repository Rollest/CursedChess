using System.Collections.Generic;

namespace CursedChess.Domain.Entities;

/// <summary>
/// Найденное решение для доски.
/// </summary>
public class Solution
{
    /// <summary>
    /// Уникальный идентификатор решения.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор доски, для которой найдено решение.
    /// </summary>
    public int BoardId { get; set; }

    /// <summary>
    /// Составные элементы решения (позиции агентов).
    /// </summary>
    public ICollection<SolutionItem> Items { get; set; } = new List<SolutionItem>();

    /// <summary>
    /// Общая стоимость решения.
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Указывает, соответствует ли решение требованию согласованности цвета.
    /// </summary>
    public bool IsColorConsistent { get; set; }
}

/// <summary>
/// Элемент решения: позиция агента внутри конкретного решения.
/// </summary>
public class SolutionItem
{
    /// <summary>
    /// Уникальный идентификатор элемента решения.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор решения, которому принадлежит элемент.
    /// </summary>
    public int SolutionId { get; set; }

    /// <summary>
    /// Индекс агента в решении.
    /// </summary>
    public int AgentId { get; set; }

    /// <summary>
    /// Строка позиции агента.
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Колонка позиции агента.
    /// </summary>
    public int Column { get; set; }
}

