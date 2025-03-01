using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Globalization;
using System.Text;

namespace Microsoft.EntityFrameworkCore.Infrastructure.Internal;

public class EntityFrameworkCoreDbContextOptionsExtension : IDbContextOptionsExtension
{
    private bool _enableForeignKeyIndex;
    private bool _enableForeignKeyConstraint;
    private SoftDeleteOptions _softDeleteOptions;
    private DbContextOptionsExtensionInfo? _info;
    private IEnumerable<string> _xPathDocumentPath;

    public EntityFrameworkCoreDbContextOptionsExtension()
    {
        _xPathDocumentPath = [];
        _softDeleteOptions = new SoftDeleteOptions();
    }

    protected EntityFrameworkCoreDbContextOptionsExtension(EntityFrameworkCoreDbContextOptionsExtension copyFrom)
    {
        _softDeleteOptions = copyFrom._softDeleteOptions;
        _xPathDocumentPath = copyFrom._xPathDocumentPath;
    }

    public DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this);

    protected virtual EntityFrameworkCoreDbContextOptionsExtension Clone()
        => new(this);

    public virtual EntityFrameworkCoreDbContextOptionsExtension WithForeignKeyIndex(bool enable = false)
    {
        var clone = Clone();
        clone._enableForeignKeyIndex = enable;
        return clone;
    }

    public virtual EntityFrameworkCoreDbContextOptionsExtension WithForeignKeyConstraint(bool enable = false)
    {
        var clone = Clone();
        clone._enableForeignKeyConstraint = enable;
        return clone;
    }

    public virtual EntityFrameworkCoreDbContextOptionsExtension WithSoftDelete(bool enable, string name, string? comment)
    {
        var clone = Clone();
        clone._softDeleteOptions = new SoftDeleteOptions(name, comment) { Enabled = true };
        return clone;
    }

    public virtual EntityFrameworkCoreDbContextOptionsExtension WithXmlCommentPath(IEnumerable<string> filePath)
    {
        var clone = Clone();
        clone._xPathDocumentPath = [.. clone._xPathDocumentPath, .. filePath];
        return clone;
    }

    public virtual bool EnableForeignKeyIndex
        => _enableForeignKeyIndex;

    public virtual bool EnableForeignKeyConstraint
        => _enableForeignKeyConstraint;

    public virtual SoftDeleteOptions SoftDeleteOptions
        => _softDeleteOptions;

    public virtual IEnumerable<string> XmlCommentPath
        => _xPathDocumentPath;

    public virtual void ApplyServices(IServiceCollection services)
    {
        services.AddEntityFrameworkCoreServices();

        var serviceDescriptor = services.FirstOrDefault(f => f.ServiceType == typeof(IQueryTranslationPreprocessorFactory));
        if (serviceDescriptor is not null && serviceDescriptor.ImplementationType is not null)
        {
            services.Add(new ServiceDescriptor(serviceDescriptor.ImplementationType, serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime));
            services.Replace(new ServiceDescriptor(serviceDescriptor.ServiceType, typeof(QueryTranslationPreprocessorFactory<>).MakeGenericType(serviceDescriptor.ImplementationType), serviceDescriptor.Lifetime));
        }
    }

    public virtual void Validate(IDbContextOptions options)
    {
    }

    protected sealed class ExtensionInfo(EntityFrameworkCoreDbContextOptionsExtension extension) : DbContextOptionsExtensionInfo(extension)
    {
        private int? _serviceProviderHash;

        private new EntityFrameworkCoreDbContextOptionsExtension Extension
            => (EntityFrameworkCoreDbContextOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider
            => false;

        public override string LogFragment => "Atomicsoft.EntityFrameworkCore";

        public override int GetServiceProviderHashCode()
        {
            if (_serviceProviderHash == null)
            {
                var hashCode = new HashCode();
                hashCode.Add(Extension._softDeleteOptions);
                hashCode.Add(Extension._xPathDocumentPath);
                hashCode.Add(Extension._enableForeignKeyIndex);

                _serviceProviderHash = hashCode.ToHashCode();
            }

            return _serviceProviderHash.Value;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            if (Extension._softDeleteOptions.Enabled)
            {
                debugInfo[$"MetioCore:{nameof(Extension.WithSoftDelete)}"] =
                    Extension._softDeleteOptions.GetHashCode().ToString(CultureInfo.InvariantCulture);
                debugInfo[$"MetioCore:{nameof(Extension.WithForeignKeyIndex)}"] =
                    Extension._enableForeignKeyIndex.GetHashCode().ToString(CultureInfo.InvariantCulture);
            }
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => other is ExtensionInfo otherInfo
                && Extension._softDeleteOptions == otherInfo.Extension._softDeleteOptions
                && Extension._enableForeignKeyIndex == otherInfo.Extension._enableForeignKeyIndex;
    }
}
