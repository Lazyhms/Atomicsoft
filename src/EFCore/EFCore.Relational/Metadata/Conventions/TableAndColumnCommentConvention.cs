using System.Xml.XPath;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

internal sealed class TableAndColumnCommentConvention : IModelFinalizingConvention
{
    private readonly List<XmlDocumentationComments> _xmlDocumentationComments = [];
    private readonly IEntityFrameworkCoreSingletonOptions _entityFrameworkCoreSingletonOptions;

    public TableAndColumnCommentConvention(IEntityFrameworkCoreSingletonOptions entityFrameworkCoreSingletonOptions)
    {
        _entityFrameworkCoreSingletonOptions = entityFrameworkCoreSingletonOptions;

        foreach (var xmlFile in _entityFrameworkCoreSingletonOptions.XmlCommentPath)
        {
            using var stream = new FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            _xmlDocumentationComments.Add(new XmlDocumentationComments(new XPathDocument(stream)));
        }
    }

    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var conventionEntityType in modelBuilder.Metadata.GetEntityTypes())
        {
            if (!conventionEntityType.ClrType.IsDefined<HardDeleteAttribute>())
            {
                ProcessSoftDeleteModelFinalizing(conventionEntityType);
            }

            ProcessCommentModelFinalizing(conventionEntityType);
            ProcessColumnOrderModelFinalizing(conventionEntityType);
        }
    }

    private void ProcessSoftDeleteModelFinalizing(IConventionEntityType conventionEntityType)
    {
        var softDeleteOptions = _entityFrameworkCoreSingletonOptions.SoftDeleteOptions;
        if (conventionEntityType.ClrType.GetCustomAttribute<SoftDeleteAttribute>() is SoftDeleteAttribute softDeleteAttribute)
        {
            softDeleteOptions = new SoftDeleteOptions { Name = softDeleteAttribute.Name, Comment = softDeleteAttribute.Comment, Order = softDeleteAttribute.Order, Enabled = softDeleteAttribute.Enable };
        }

        var conventionProperty = conventionEntityType.FindProperty(softDeleteOptions.Name) ?? conventionEntityType.AddProperty(softDeleteOptions.Name, typeof(bool));
        if (softDeleteOptions.Enabled && null != conventionProperty && conventionProperty.ClrType == typeof(bool))
        {
            conventionEntityType.SetOrRemoveSoftDelete(softDeleteOptions.Name);

            conventionProperty.SetDefaultValue(false, fromDataAnnotation: true);
            conventionProperty.SetComment(softDeleteOptions.Comment, fromDataAnnotation: true);
            conventionProperty.SetColumnOrder(softDeleteOptions.Order, fromDataAnnotation: true);

            var queryFilterExpression = conventionEntityType.GetQueryFilter();
            var parameterExpression = queryFilterExpression?.Parameters[0] ?? Expression.Parameter(conventionEntityType.ClrType, "filter");
            var methodCallExpression = Expression.Call(typeof(EF), nameof(EF.Property), [typeof(bool)], parameterExpression, Expression.Constant(conventionProperty.GetColumnName()));

            conventionEntityType.SetQueryFilter(queryFilterExpression == null
                ? Expression.Lambda(Expression.Equal(methodCallExpression, Expression.Constant(false)), parameterExpression)
                : Expression.Lambda(Expression.AndAlso(queryFilterExpression.Body, Expression.Equal(methodCallExpression, Expression.Constant(false))), parameterExpression));
        }
    }

    private void ProcessColumnOrderModelFinalizing(IConventionEntityType conventionEntityType)
    {
        var index = 0;
        foreach (var conventionProperty in conventionEntityType.GetProperties().OrderBy(o => o.GetColumnOrder()))
        {
            if (!conventionEntityType.IsOwned())
            {
                if (!conventionProperty.GetColumnOrder().HasValue && !conventionProperty.IsShadowProperty())
                {
                    conventionProperty.SetColumnOrder(index++, true);
                }
            }
            else
            {
                var conventionNavigations = conventionEntityType.FindOwnership()?.PrincipalEntityType.GetNavigations().Where(w => w.TargetEntityType.IsOwned()) ?? [];
                var conventionIndex = conventionNavigations.Select((x, index) => x.TargetEntityType == conventionEntityType ? index : -1).Where(w => w != -1).DefaultIfEmpty(-1).First();
                if (conventionIndex >= 0 && !conventionProperty.GetColumnOrder().HasValue && !conventionProperty.IsShadowProperty())
                {
                    conventionProperty.SetColumnOrder(50 + (conventionIndex * 10) + index++, true);
                }
            }
        }
    }

    private void ProcessCommentModelFinalizing(IConventionEntityType conventionEntityType)
    {
        var xmlCommentsDocument = _xmlDocumentationComments.Find(xmlCommentsDocument => xmlCommentsDocument.FindAssemblyXPathNavigatoryForType(conventionEntityType.ClrType) is not null);
        if (xmlCommentsDocument is not null)
        {
            if (!conventionEntityType.IsOwned() && string.IsNullOrWhiteSpace(conventionEntityType.GetComment()))
            {
                conventionEntityType.SetComment(xmlCommentsDocument.GetMemberNameForType(conventionEntityType.ClrType), true);
            }

            foreach (var conventionProperty in conventionEntityType.GetProperties())
            {
                if (!conventionProperty.IsShadowProperty() && string.IsNullOrWhiteSpace(conventionProperty.GetComment()))
                {
                    conventionProperty.SetComment(xmlCommentsDocument.GetMemberNameForFieldOrProperty(conventionProperty.PropertyInfo!), fromDataAnnotation: true);
                }
            }
        }
    }
}
