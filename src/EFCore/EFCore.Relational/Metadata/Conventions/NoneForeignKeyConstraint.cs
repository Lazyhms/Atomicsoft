namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
    public class NoneForeignKeyConstraint : IModelFinalizingConvention
    {
        public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
        {
            foreach (var conventionEntityType in modelBuilder.Metadata.GetEntityTypes())
            {
                foreach (var conventionForeignKey in conventionEntityType.GetForeignKeys())
                {
                    conventionForeignKey.Builder.HasConstraintName(null);
                }
            }
        }
    }
}
