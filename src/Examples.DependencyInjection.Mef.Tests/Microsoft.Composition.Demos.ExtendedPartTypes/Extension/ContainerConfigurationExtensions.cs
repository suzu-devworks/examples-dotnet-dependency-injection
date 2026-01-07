/**
 * ContainerConfigurationExtensions.cs
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

using System.Composition.Hosting;

namespace Microsoft.Composition.Demos.ExtendedPartTypes.Extension
{

    internal static class ContainerConfigurationExtensions
    {
        public static ContainerConfiguration WithExport<T>(this ContainerConfiguration configuration, T exportedInstance, string contractName = null, IDictionary<string, object> metadata = null)
        {
            return WithExport(configuration, exportedInstance, typeof(T), contractName, metadata);
        }

        public static ContainerConfiguration WithExport(this ContainerConfiguration configuration, object exportedInstance, Type contractType, string contractName = null, IDictionary<string, object> metadata = null)
        {
            return configuration.WithProvider(new InstanceExportDescriptorProvider(
                exportedInstance, contractType, contractName, metadata));
        }

        public static ContainerConfiguration WithFactoryDelegate<T>(this ContainerConfiguration configuration, Func<T> exportedInstanceFactory, string contractName = null, IDictionary<string, object> metadata = null, bool isShared = false)
        {
            return WithFactoryDelegate(configuration, () => exportedInstanceFactory(), typeof(T), contractName, metadata, isShared);
        }

        public static ContainerConfiguration WithFactoryDelegate(this ContainerConfiguration configuration, Func<object> exportedInstanceFactory, Type contractType, string contractName = null, IDictionary<string, object> metadata = null, bool isShared = false)
        {
            return configuration.WithProvider(new DelegateExportDescriptorProvider(
                exportedInstanceFactory, contractType, contractName, metadata, isShared));
        }
    }

}