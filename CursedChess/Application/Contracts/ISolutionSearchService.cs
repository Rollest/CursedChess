using CursedChess.Domain.Entities;
using CursedChess.Domain.Rules;

namespace CursedChess.Application.Contracts;

/// <summary>
/// Сервис поиска решений для размещения агентов на доске.
/// </summary>
public interface ISolutionSearchService
{
    /// <summary>
    /// Находит решения и шаги поиска для заданных ограничений.
    /// </summary>
    /// <param name="board">Доска, для которой выполняется поиск.</param>
    /// <param name="conflictRules">Правила конфликтов между позициями агентов.</param>
    /// <param name="requireSameColor">Требует ли найденные позиции одного цвета.</param>
    /// <returns>
    /// Кортеж: решения и последовательность шагов поиска для построения трассировки.
    /// </returns>
    (IReadOnlyList<Solution> solutions, IReadOnlyList<SearchStep> steps) FindSolutions(
        Board board,
        IReadOnlyCollection<IConflictRule> conflictRules,
        bool requireSameColor);
}

