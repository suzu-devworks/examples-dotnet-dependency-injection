/**
 * SinglePartExportDescriptorProvider.cs
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

    internal abstract class SinglePartExportDescriptorProvider : ExportDescriptorProvider
    {
        private readonly Type _contractType;
        private readonly string _contractName;

        protected SinglePartExportDescriptorProvider(Type contractType, string contractName, IDictionary<string, object> metadata)
        {
            _contractType = contractType ?? throw new ArgumentNullException(nameof(contractType));
            _contractName = contractName;
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        protected bool IsSupportedContract(CompositionContract contract)
        {
            if (contract.ContractType != _contractType ||
                contract.ContractName != _contractName)
                return false;

            if (contract.MetadataConstraints != null)
            {
                var subsetOfConstraints = contract.MetadataConstraints.Where(c => Metadata.ContainsKey(c.Key)).ToDictionary(c => c.Key, c => Metadata[c.Key]);
                var constrainedSubset = new CompositionContract(contract.ContractType, contract.ContractName,
                    subsetOfConstraints.Count == 0 ? null : subsetOfConstraints);

                if (!contract.Equals(constrainedSubset))
                    return false;
            }

            return true;
        }

        protected IDictionary<string, object> Metadata { get; }
    }

}