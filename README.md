# UωU ❤❤❤ UNITY DEPENDENCY INJECTION

![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) <strong>PRODUCT FROM VIETNAM ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) LOVE FROM VIETNAMESE</strong> ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true)

Why use ?
- Dependency injection: Simple flow, easy to control, can binding all relevant types.
- Free, open source, allow for commerce purpose.

Any improve idea, please comment on <strong>Issues</strong> tab.

## Dependency injection

Support: Field injection, property injection, method inject.

#### 1. Setup

Create a class inherit DIContext then drag it into hierarchy.

```csharp
public class GameManager : DIContext
{
    public override void Setup()
    {
      // This function is used for binding dependency
    }
  
    public override void Initialize()
    {
      // Finish setup, start game here
    }
}
```

#### 2. Binding

##### Binding dependency not create new instance ! not automatic inject dependency into instance !

Bind interface/class into class.
```csharp
this.binder.BindCommand<ClassType>(ClassType sourceType);
this.binder.BindCommand<ClassType, ClassType>(instance);
this.binder.BindCommand<InterfaceType, ClassType>(instance);
```

Bind to gameObject component
```csharp
this.binder.BindToGameObjectCommand<SourceType>(string gameObjectName);
this.binder.BindToGameObjectCommand<SourceType, TargetType>(string gameObjectName);
```

After register all binding command, we need to execute them.
```csharp
this.binder.ExecuteBindingCommand();
```

Bind all relevants type

```csharp
var ignoreNamespaceList = new[] 
{
    "System",
    "UnityEngine"
};

this.binder.BindRelevantsTypeCommand(instance, ignoreNamespaceList);
this.binder.BindGameObjectRelevantsTypeCommand<T>(string gameObjectName, string[] ignoreList);
```

Need more ? All binding feature here.
```csharp
namespace UwU.DI.Binding
{
    public interface IBinder
    {
        /// <summary>
        /// Find component instance on scene then bind all relevant types.
        /// </summary>
        void BindComponentRelevantsCommand<FindComponentType>();

        /// <summary>
        /// Find component instance on scene then bind all relevant types.
        /// Support ignore namespace.
        /// </summary>
        void BindComponentRelevantsCommand<FindComponentType>(string[] ignoreNamespaceList);

        /// <summary>
        /// Find component instance on scene, then bind SourceType into it.
        /// SoureType can be interface or class.
        /// </summary>
        void BindComponentCommand<SourceType, FindComponentType>();

        /// <summary>
        /// Find component instance on scene, then bind it self.
        /// </summary>
        void BindComponentCommand<FindComponentType>();

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
```

#### 3. Injection

Use attribute <strong>[Inject]</strong> to inject dependencies into a instance object.

Use <strong>this.Inject()</strong> to inject dependencies into current class.

Field injection
```csharp
public class FieldInjectionSample
{
    [Inject] private readonly UnityEngine.Transform transform;

    public void Initialize()
    {
        this.Inject();
    }
}
```

Property injection
```csharp
public class PropertyInjectionSample
{
    [Inject] private UnityEngine.Transform transform { get; set; }

    public void Initialize()
    {
        this.Inject();
    }
}
```

Method injection
```csharp
public class MethodInjectionSample
{
    private UnityEngine.Transform transform;
    
    [Inject]
    public void AnyMethodName(UnityEngine.Transform transform)
    {
        this.transform = transform;
    }

    public void Initialize()
    {
        this.Inject();
    }
}
```

Why not support <strong>constructor injection</strong> ?

+ On Unity, game objects, components are usually using <strong>UnityEngine.Object.Instantiate</strong> to create new instance.
+ Constructor inside MonoBehaviour is not common.
+ This plugin <strong>NOT</strong> support genegrating/creating/cloning object automatically ! All object need to be create normally.
+ Object can be create manually and pass references into constructor.

```csharp
private void Awake()
{
    var objectA = new ClassA();
    var objectB = new ClassB(objectA);
}
```

# (づ｡◕‿‿◕｡)づ 

## IF THESE HELP YOU FINISH PROJECT, PLEASE DONATE ME A COFFEE CUP

PAYPAL: https://paypal.me/sandichhuu

ヾ(＠＾▽＾＠)ﾉ THANK YOU

.

.

.

.

.

.

.

.

###### ಠ_ಠ DONATE OR LUCKY -1000
