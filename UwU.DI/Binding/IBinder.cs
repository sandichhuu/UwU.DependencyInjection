using UnityEngine;

namespace UwU.DI.Binding
{
    public interface IBinder
    {
        /// <summary>
        /// Find component instance on scene then bind all relevant types.
        /// </summary>
        void BindComponentRelevantsCommand<FindComponentType>() where FindComponentType : Object;

        /// <summary>
        /// Find component instance on scene then bind all relevant types.
        /// Support ignore namespace.
        /// </summary>
        void BindComponentRelevantsCommand<FindComponentType>(string[] ignoreNamespaceList) where FindComponentType : Object;

        /// <summary>
        /// Find component instance on scene, then bind SourceType into it.
        /// SoureType can be interface or class.
        /// </summary>
        void BindComponentCommand<SourceType, FindComponentType>() where FindComponentType : Object;

        /// <summary>
        /// Find component instance on scene, then bind it self.
        /// </summary>
        void BindComponentCommand<FindComponentType>() where FindComponentType : Object;

        /// <summary>
        /// Find component instance on scene, then bind SourceType into it.
        /// SoureType can be interface or class.
        /// </summary>
        void BindComponentCommand<SourceType, FindComponentType>(bool includeInactive) where FindComponentType : Object;

        /// <summary>
        /// Find component instance on scene, then bind it self.
        /// </summary>
        void BindComponentCommand<FindComponentType>(bool includeInactive) where FindComponentType : Object;

        /// <summary>
        /// Bind relevant types of instance.
        /// </summary>
        void BindRelevantsTypeCommand(object instance);

        /// <summary>
        /// Bind relevant types of instance.
        /// Support ignore namespace.
        /// </summary>
        void BindRelevantsTypeCommand(object instance, string[] ignoreNamespaceList);

        /// <summary>
        /// Find gameObject with name on scene, then get component inside.
        /// Finally, bind component.
        /// </summary>
        void BindGameObjectRelevantsTypeCommand<GetComponentType>(string gameObjectName);

        /// <summary>
        /// Find gameObject with name on scene, then get component inside.
        /// Finally, bind component.
        ///
        /// Support ignore namespace.
        /// </summary>
        void BindGameObjectRelevantsTypeCommand<GetComponentType>(string gameObjectName, string[] ignoreNamespaceList);

        /// <summary>
        /// Bind interface/class to instance.
        /// </summary>
        void BindCommand<SourceType>(SourceType sourceType);

        /// <summary>
        /// Bind interface/class to relevant type's instance.
        /// </summary>
        void BindCommand<SourceType, TargetType>(TargetType instance);

        /// <summary>
        /// Find gameObject with name, then get component inside.
        /// Bind that component.
        /// </summary>
        void BindGameObjectCommand<GetComponentType>(string gameObjectName);

        /// <summary>
        /// Find gameObject with name, then get component inside.
        /// Bind interface/class to relevant type's instance.
        /// </summary>
        void BindGameObjectCommand<SourceType, GetComponentType>(string gameObjectName);

        /// <summary>
        /// Unbind the type
        /// </summary>
        void Unbind<TargetType>();

        /// <summary>
        /// Unbind the instance
        /// </summary>
        void Unbind<TargetType>(TargetType instance);

        /// <summary>
        /// All binding command will not executed, use this function to make binding work !
        /// </summary>
        void ExecuteBindingCommand();
    }
}