namespace UwU.DI.Binding
{
    public interface IBinder
    {
        void BindComponentRelevantsCommand<FindComponentType>();

        void BindComponentRelevantsCommand<FindComponentType>(string[] ignoreNamespaceList);

        void BindComponentCommand<SourceType, FindComponentType>();

        void BindComponentCommand<FindComponentType>();

        /// <summary>
        /// Binding clean, not bind "UnityEngine" and "System" component
        /// </summary>
        void BindRelevantsTypeCommand(object instance);

        void BindRelevantsTypeCommand(object instance, string[] ignoreNamespaceList);

        /// <summary>
        /// Binding clean, not bind "UnityEngine" and "System" component
        /// </summary>
        void BindGameObjectRelevantsTypeCommand<GetComponentType>(string gameObjectName);

        void BindGameObjectRelevantsTypeCommand<GetComponentType>(string gameObjectName, string[] ignoreNamespaceList);

        void BindCommand<SourceType>(SourceType sourceType);

        void BindCommand<SourceType, TargetType>(TargetType instance);

        void BindGameObjectCommand<GetComponentType>(string gameObjectName);

        void BindGameObjectCommand<SourceType, GetComponentType>(string gameObjectName);

        void Unbind<TargetType>();

        void Unbind<TargetType>(TargetType instance);

        void ExecuteBindingCommand();
    }
}