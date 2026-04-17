using System.Collections.Generic;
using CursedChess.Application.ConflictRules;
using CursedChess.Application.Contracts;
using CursedChess.Domain.Entities;
using CursedChess.Domain.Rules;

namespace CursedChess.Application.Services;

/// <summary>
/// Реализация поиска решений с учётом правил конфликтов.
/// </summary>
public sealed class SolutionSearchService : ISolutionSearchService
{
    private readonly IConflictRuleRegistry _ruleRegistry;

    /// <summary>
    /// Создаёт сервис поиска с реестром правил конфликта.
    /// </summary>
    /// <param name="ruleRegistry">Реестр правил по ключам агента.</param>
    public SolutionSearchService(IConflictRuleRegistry ruleRegistry)
    {
        _ruleRegistry = ruleRegistry;
    }

    /// <summary>
    /// Находит решения и формирует список шагов поиска по заданной доске и ограничениям.
    /// </summary>
    /// <param name="board">Доска, для которой выполняется поиск.</param>
    /// <param name="requireSameColor">Требует ли согласованность по цвету у найденных позиций.</param>
    /// <returns>Кортеж: найденные решения и шаги поиска.</returns>
    public (IReadOnlyList<Solution> solutions, IReadOnlyList<SearchStep> steps) FindSolutions(
        Board board,
        bool requireSameColor)
    {
        var size = board.Size;
        var boardAgents = board.Agents.Count > 0
            ? board.Agents
            : Enumerable.Range(0, size).Select(i => new Agent
            {
                BoardId = board.Id,
                ColumnIndex = i,
                Priority = i,
                IsFixed = false,
                ConflictRuleKeys = new List<string>(KnownConflictRuleKeys.DefaultNQueens)
            }).ToList();
        var orderedAgents = boardAgents
            .OrderByDescending(a => a.IsFixed)
            .ThenBy(a => a.Priority)
            .ThenBy(a => a.ColumnIndex)
            .ToList();
        var fixedByColumn = board.FixedPositions
            .GroupBy(fp => fp.Column)
            .ToDictionary(g => g.Key, g => g.First().Row);

        var currentPositions = new Position?[size];
        var steps = new List<SearchStep>();
        var solutions = new List<Solution>();
        var stepIndex = 0;

        /// <summary>
        /// Добавляет запись шага в трассировку поиска.
        /// </summary>
        /// <param name="type">Тип шага поиска.</param>
        /// <param name="col">Колонка активного агента.</param>
        /// <param name="row">Строка агента (или `null`, если неприменимо).</param>
        /// <param name="comment">Краткое текстовое описание шага.</param>
        /// <param name="snapshot">Снимок позиций агентов на момент шага.</param>
        void AddStep(SearchStepType type, int col, int? row, string comment, IReadOnlyCollection<Position> snapshot)
        {
            steps.Add(new SearchStep
            {
                Index = stepIndex++,
                Type = type,
                ActiveAgentColumn = col,
                Row = row,
                Column = col,
                Comment = comment,
                AgentPositionsSnapshot = snapshot
            });
        }

        /// <summary>
        /// Формирует снапшот текущих позиций агентов.
        /// </summary>
        /// <returns>Непустая коллекция позиций текущих агентов.</returns>
        IReadOnlyCollection<Position> BuildSnapshot()
        {
            var list = new List<Position>();
            for (var c = 0; c < size; c++)
            {
                if (currentPositions[c] is { } p)
                {
                    list.Add(p);
                }
            }

            return list;
        }

        /// <summary>
        /// Проверяет, что все позиции на доске принадлежат одному цвету.
        /// </summary>
        /// <param name="positions">Коллекция позиций, которые нужно проверить.</param>
        /// <returns>`true`, если цвет всех клеток совпадает; иначе `false`.</returns>
        bool IsColorConsistent(IReadOnlyCollection<Position> positions)
        {
            string? color = null;
            foreach (var position in positions)
            {
                var cell = board.Cells.First(c => c.Row == position.Row && c.Column == position.Column);
                if (color is null)
                {
                    color = cell.Color;
                }
                else if (!string.Equals(color, cell.Color, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Вычисляет суммарную стоимость набора позиций.
        /// </summary>
        /// <param name="positions">Коллекция позиций, для которых суммируется стоимость.</param>
        /// <returns>Суммарная стоимость позиций.</returns>
        decimal ComputeCost(IReadOnlyCollection<Position> positions)
        {
            decimal sum = 0;
            foreach (var position in positions)
            {
                var cell = board.Cells.First(c => c.Row == position.Row && c.Column == position.Column);
                sum += cell.Cost;
            }

            return sum;
        }

        /// <summary>
        /// Проверяет подходит ли позиция под правила размещенных агентов.
        /// </summary>
        /// <param name="candidateOrderIndex">Индекс агента в очереди.</param>
        /// <param name="candidatePos">Позиция агента.</param>
        /// <returns>`true`, если есть конфликт по правилу агента; иначе `false`.</returns>
        bool IsAcceptedByPlacedAgents(int candidateOrderIndex, Position candidatePos)
        {
            for (var placedOrderIndex = 0; placedOrderIndex < candidateOrderIndex; placedOrderIndex++)
            {
                var placedAgent = orderedAgents[placedOrderIndex];
                var placedPosition = currentPositions[placedAgent.ColumnIndex];
                if (placedPosition is null)
                {
                    continue;
                }

                var placedAgentRules = _ruleRegistry.Resolve(placedAgent.ConflictRuleKeys ?? []);

                foreach (var rule in placedAgentRules)
                {
                    if (rule.HasConflict(candidatePos, new[] { placedPosition.Value }))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Рекурсивно перебирает размещения агентов по колонкам.
        /// </summary>
        /// <param name="agentOrderIndex">Индекс агента в порядке логического взаимодействия.</param>
        /// <returns>Ничего не возвращает; заполняет список `solutions` через вычисление вариантов.</returns>
        void Backtrack(int agentOrderIndex)
        {
            if (agentOrderIndex == orderedAgents.Count)
            {
                var snapshot = BuildSnapshot();
                var cost = ComputeCost(snapshot);
                var isColorOk = !requireSameColor || IsColorConsistent(snapshot);

                var solution = new Solution
                {
                    BoardId = board.Id,
                    TotalCost = cost,
                    IsColorConsistent = isColorOk,
                    Items = orderedAgents
                        .Select(agent => new SolutionItem
                        {
                            AgentId = agent.Id != 0 ? agent.Id : agent.ColumnIndex,
                            Row = currentPositions[agent.ColumnIndex]?.Row ?? 0,
                            Column = agent.ColumnIndex
                        })
                        .ToList()
                };

                AddStep(SearchStepType.SolutionFound, orderedAgents[^1].ColumnIndex, null, "Найдено полное решение", snapshot);
                solutions.Add(solution);
                return;
            }

            var agent = orderedAgents[agentOrderIndex];
            var column = agent.ColumnIndex;

            if (fixedByColumn.TryGetValue(column, out var fixedRow))
            {
                var fixedPos = new Position(fixedRow, column);

                AddStep(SearchStepType.TryPlace, column, fixedRow, "Попытка установить агента в фиксированную позицию", BuildSnapshot());

                if (!IsAcceptedByPlacedAgents(agentOrderIndex, fixedPos))
                {
                    AddStep(SearchStepType.ConflictFound, column, fixedRow,
                        "Фиксированная позиция отклонена ранее размещённым агентом", BuildSnapshot());
                    return;
                }

                currentPositions[column] = fixedPos;
                AddStep(SearchStepType.Placed, column, fixedRow, "Агент установлен в фиксированную позицию", BuildSnapshot());

                Backtrack(agentOrderIndex + 1);

                AddStep(SearchStepType.Backtrack, column, fixedRow, "Откат с фиксированной позиции", BuildSnapshot());
                currentPositions[column] = null;

                return;
            }

            for (var row = 0; row < size; row++)
            {
                var pos = new Position(row, column);

                AddStep(SearchStepType.TryPlace, column, row, "Попытка установить агента", BuildSnapshot());

                if (!IsAcceptedByPlacedAgents(agentOrderIndex, pos))
                {
                    AddStep(SearchStepType.ConflictFound, column, row,
                        "Позиция отклонена ранее размещённым агентом", BuildSnapshot());
                    continue;
                }

                currentPositions[column] = pos;
                AddStep(SearchStepType.Placed, column, row, "Агент успешно установлен", BuildSnapshot());

                Backtrack(agentOrderIndex + 1);

                AddStep(SearchStepType.Backtrack, column, row, "Откат к предыдущему агенту", BuildSnapshot());
                currentPositions[column] = null;
            }
        }

        Backtrack(0);

        if (requireSameColor)
        {
            solutions = solutions
                .Where(s => s.IsColorConsistent)
                .ToList();
        }

        return (solutions, steps);
    }
}

