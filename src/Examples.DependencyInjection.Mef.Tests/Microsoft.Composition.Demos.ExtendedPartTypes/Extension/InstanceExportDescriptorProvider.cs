/**
 * InstanceExportDescriptorProvider.cs
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

    // This one-instance-per-provider design is not efficient for more than a few instances;
    // we're just aiming to show the mechanics here.
    internal class InstanceExportDescriptorProvider : SinglePartExportDescriptorProvider
    {
        private readonly object _exportedInstance;

        public InstanceExportDescriptorProvider(object exportedInstance, Type contractType, string contractName, IDictionary<string, object> metadata)
            : base(contractType, contractName, metadata)
        {
            _exportedInstance = exportedInstance ?? throw new ArgumentNullException(nameof(exportedInstance));
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (IsSupportedContract(contract))
                yield return new ExportDescriptorPromise(contract, _exportedInstance.ToString(), true, NoDependencies, _ =>
                    ExportDescriptor.Create((c, o) => _exportedInstance, Metadata));
        }
    }

}