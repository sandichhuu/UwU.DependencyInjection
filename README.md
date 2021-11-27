# UωU ❤❤❤ UNITY DEPENDENCY INJECTION

![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) <strong>PRODUCT FROM VIETNAM ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) LOVE FROM VIETNAMESE</strong> ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true)

Why use ?
- Dependency injection: Simple flow, multithreading support, easy to control, can binding all relevant types.
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

Bind interface into class.
```csharp
this.binder.BindCommand<InterfaceType, ClassType>(instance);
```
  
Bind class into class
```csharp
this.binder.BindCommand<ClassType, ClassType>(instance);
```

Bind all relevants type
```csharp
this.binder.BindRelevantsTypeCommand(instance, isIgnoreSystemType);
```

After register all binding command, we need to execute them.
```csharp
this.binder.ExecuteBindingCommand();
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

#### 4. Multithread support

If your project use a lot of binding command before execute, you should enable multithreading feature.

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

![Alt text](https://github.com/vohuu/Assets/blob/main/UseMultiThreading.png?raw=true)

# (づ｡◕‿‿◕｡)づ 

## IF THESE HELP YOU FINISH PROJECT, PLEASE DONATE ME A COFFE CUP

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
