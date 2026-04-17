using System.Text.Json;
using CursedChess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CursedChess.Infrastructure;

/// <summary>
/// Контекст Entity Framework Core для хранения досок и решений.
/// </summary>
public class BoardDbContext : DbContext
{
    /// <summary>
    /// Создаёт контекст с заданными параметрами.
    /// </summary>
    /// <param name="options">Параметры конфигурации контекста.</param>
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Набор досок.
    /// </summary>
    public DbSet<Board> Boards => Set<Board>();
    /// <summary>
    /// Набор клеток.
    /// </summary>
    public DbSet<Cell> Cells => Set<Cell>();
    /// <summary>
    /// Набор агентов.
    /// </summary>
    public DbSet<Agent> Agents => Set<Agent>();
    /// <summary>
    /// Набор фиксированных позиций.
    /// </summary>
    public DbSet<FixedPosition> FixedPositions => Set<FixedPosition>();
    /// <summary>
    /// Набор решений.
    /// </summary>
    public DbSet<Solution> Solutions => Set<Solution>();
    /// <summary>
    /// Набор элементов решений.
    /// </summary>
    public DbSet<SolutionItem> SolutionItems => Set<SolutionItem>();

    /// <summary>
    /// Настраивает модель БД и связи между сущностями.
    /// </summary>
    /// <param name="modelBuilder">Сборщик конфигурации модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>()
            .HasMany(b => b.Cells)
            .WithOne()
            .HasForeignKey(c => c.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Board>()
            .HasMany(b => b.Agents)
            .WithOne()
            .HasForeignKey(a => a.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Agent>()
            .Property(a => a.ConflictRuleKeys)
            .HasConversion(
                v => SerializeConflictRuleKeys(v),
                v => DeserializeConflictRuleKeys(v));

        modelBuilder.Entity<Agent>()
            .HasIndex(a => new { a.BoardId, a.ColumnIndex })
            .IsUnique();

        modelBuilder.Entity<Board>()
            .HasMany(b => b.FixedPositions)
            .WithOne()
            .HasForeignKey(fp => fp.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FixedPosition>()
            .HasOne(fp => fp.Agent)
            .WithMany()
            .HasForeignKey(fp => fp.AgentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FixedPosition>()
            .HasIndex(fp => new { fp.BoardId, fp.AgentId })
            .IsUnique();

        modelBuilder.Entity<FixedPosition>()
            .HasIndex(fp => new { fp.BoardId, fp.Column })
            .IsUnique();

        modelBuilder.Entity<Cell>()
            .HasIndex(c => new { c.BoardId, c.Row, c.Column })
            .IsUnique();

        modelBuilder.Entity<Solution>()
            .HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(i => i.SolutionId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static readonly JsonSerializerOptions ConflictRuleKeysJsonOptions = new();

    private static string SerializeConflictRuleKeys(List<string> value) =>
        JsonSerializer.Serialize(value, ConflictRuleKeysJsonOptions);

    private static List<string> DeserializeConflictRuleKeys(string value) =>
        string.IsNullOrEmpty(value)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(value, ConflictRuleKeysJsonOptions) ?? new List<string>();
}

