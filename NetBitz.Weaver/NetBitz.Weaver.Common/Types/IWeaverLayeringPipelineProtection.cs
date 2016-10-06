using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBitz.Weaver.Common.Types
{
    public interface IWeaverLayeringPipelineProtection
    {
        /// <summary>
        /// A GUID for the protection
        /// </summary>
        string ProtectionGuid { get; }

        /// <summary>
        /// The name of the protection
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A description about the protection
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Specifies whether the protection must be run on a batch or whether it can be run individually
        /// on each module factory.
        /// </summary>
        bool RequiresBatchProtection { get; }

        /// <summary>
        /// Initializes the protection provider
        /// </summary>
        void Initialize();

        void RunProtection(LayeredModuleFactory factory);

        void RunProtection(LayeredModuleFactoryCollection factoryCollection);

        /// <summary>
        /// Allows the protection class to do cleanup
        /// </summary>
        void Unload();
    }
}
