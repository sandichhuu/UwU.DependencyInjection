namespace UwU.DI.Binding
{
    public interface IBinder
    {
        /// <summary>
        /// Binding clean, not bind "UnityEngine" and "System" component
        /// </summary>
        void BindRelevantsTypeCommand(object instance);

        void BindRelevantsTypeCommand(object instance, string[] ignoreNamespaceList);

        /// <summary>
        /// Binding clean, not bind "UnityEngine" and "System" component
        /// </summary>
        void BindGameObjectRelevantsTypeCommand<T>(string gameObjectName);

        void BindGameObjectRelevantsTypeCommand<T>(string gameObjectName, string[] ignoreNamespaceList);

        void BindCommand<SourceType>(SourceType sourceType);

        void BindCommand<SourceType, TargetType>(TargetType instance);

        void BindGameObjectCommand<SourceType>(string gameObjectName);

        void BindGameObjectCommand<SourceType, TargetType>(string gameObjectName);

        void Unbind<T>();

        void Unbind<T>(T obj);

        void ExecuteBindingCommand();
    }
}