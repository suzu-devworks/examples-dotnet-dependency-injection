/**
 * DelegateExportDescriptorProvider.cs
 *
 * Copyright (c) 2017 Microsoft
 *
 * This software is released under the MIT License.
 * http://opensource.org/licenses/mit-license.php
 *
 * References from.
 * https://github.com/microsoftarchive/mef/blob/master/oob/demo/Microsoft.Composition.Fixtures.ExtendedPartTypes/Extension/
 *
 */
#pragma warning disable IDE0130
#pragma warning disable IDE0161
#nullable disable

using System.Composition.Hosting.Core;

namespace Microsoft.Composition.Demos.ExtendedPartTypes.Extension
{

    internal class DelegateExportDescriptorProvider : SinglePartExportDescriptorProvider
    {
        private readonly CompositeActivator _activator;

        public DelegateExportDescriptorProvider(Func<object> exportedInstanceFactory, Type contractType, string contractName, IDictionary<string, object> metadata, bool isShared)
            : base(contractType, contractName, metadata)
        {
            if (exportedInstanceFactory == null) throw new ArgumentNullException(nameof(exportedInstanceFactory));

            // Runs the factory method, validates the result and registers it for disposal if necessary.
            object Constructor(LifetimeContext c, CompositionOperation o)
            {
                var result = exportedInstanceFactory();
                if (result == null)
                    throw new InvalidOperationException("Delegate factory returned null.");

                if (result is IDisposable disposable)
                    c.AddBoundInstance(disposable);

                return result;
            }

            if (isShared)
            {
                var sharingId = LifetimeContext.AllocateSharingId();
                _activator = (c, o) =>
                {
                    // Find the root composition scope.
                    var scope = c.FindContextWithin(null);
                    if (scope == c)
                    {
                        // We're already in the root scope, create the instance
                        return scope.GetOrCreate(sharingId, o, Constructor);
                    }
                    else
                    {
                        // Composition is moving up the hierarchy of scopes; run
                        // a new operation in the root scope.
                        return CompositionOperation.Run(scope, (c1, o1) => c1.GetOrCreate(sharingId, o1, Constructor));
                    }
                };
            }
            else
            {
                _activator = Constructor;
            }
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (IsSupportedContract(contract))
                yield return new ExportDescriptorPromise(contract, "factory delegate", true, NoDependencies, _ => ExportDescriptor.Create(_activator, Metadata));
        }
    }

}