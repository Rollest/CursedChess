using CursedChess.Domain.Entities;

namespace CursedChess.Application.Contracts;

/// <summary>
/// Репозиторий для сохранения и получения найденных решений.
/// </summary>
public interface ISolutionRepository
{
    /// <summary>
    /// Асинхронно сохраняет решения для доски.
    /// </summary>
    /// <param name="solutions">Коллекция решений для сохранения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Задача завершения операции сохранения.</returns>
    Task SaveSolutionsAsync(IEnumerable<Solution> solutions, CancellationToken cancellationToken = default);

    /// <summary>
    /// Асинхронно получает решения для указанной доски.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список решений для доски.</returns>
    Task<IReadOnlyList<Solution>> GetSolutionsForBoardAsync(int boardId, CancellationToken cancellationToken = default);
}

