
namespace Microsoft.EntityFrameworkCore.Migrations;

public class MigrationsSqlGenerator<TIMigrationsSqlGenerator>(
    MigrationsSqlGeneratorDependencies dependencies, TIMigrationsSqlGenerator migrationsSqlGenerator)
    : MigrationsSqlGenerator(dependencies), IMigrationsSqlGenerator where TIMigrationsSqlGenerator : IMigrationsSqlGenerator
{
    public override IReadOnlyList<MigrationCommand> Generate(IReadOnlyList<MigrationOperation> operations, IModel? model = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
    {
        foreach (var item in operations.OfType<CreateTableOperation>())
        {
            item.ForeignKeys.Clear();
        }
        return migrationsSqlGenerator.Generate(operations, model, options);
    }
}
