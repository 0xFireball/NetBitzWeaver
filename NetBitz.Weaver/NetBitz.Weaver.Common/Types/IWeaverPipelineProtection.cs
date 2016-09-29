namespace NetBitz.Weaver.Types
{
    /// <summary>
    /// A protection for the NetBitz Weaver pipeline. Possibly, a different kind of protection will be added
    /// in the future.
    /// </summary>
    public interface IWeaverPipelineProtection
    {
        /// <summary>
        /// A GUID for the protection
        /// </summary>
        string Guid { get; }

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

        void RunProtection(ProtectedModuleFactory factory);

        void RunProtection(ProtectedModuleFactoryCollection factoryCollection);

        /// <summary>
        /// Allows the protection class to do cleanup
        /// </summary>
        void Unload();
    }
}