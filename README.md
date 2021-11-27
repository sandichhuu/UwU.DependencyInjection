# UωU ❤❤❤ GOOD UNITY PLUGIN

![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) <strong>PRODUCT FROM VIETNAM</strong> ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true)

Support: Dependency injection, Object pool

Why use ?
- Dependency injection: Simple flow, multithreading support, easy to control, can binding all relevant types.
- Object pool: Deep clone object from memory make it fast as posible. (UnityEngine.Object not support clone from memory).
- Free, open source, allow for commerce purpose.

Any improve idea, please comment on <strong>Issues</strong> tab.

## A. Dependency injection

Support: Field injection, property injection, method inject.

#### 1. Setup

Create a class inherit DIContext then drag it into hierarchy.

```
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

Bind interface into class.

```
this.binder.BindCommand<InterfaceType, ClassType>(instance);
```
  
Bind class into class
```
this.binder.BindCommand<ClassType, ClassType>(instance);
```

Bind all relevants type
```
this.binder.BindRelevantsTypeCommand(instance, isIgnoreSystemType);
```

After register all binding command, we need to execute them.
```
this.binder.ExecuteBindingCommand();
```

#### 3. Injection

Use attribute <strong>[Inject]</strong> to inject dependency into a class or object.

Field injection
```
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
```
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
```
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

```
private void Awake()
{
    var objectA = new ClassA();
    var objectB = new ClassB(objectA);
}
```

#### 4. Multithread support

If your project use a lot of binding command before execute, you should enable multithreading feature.


## B. Object pool

Supper easy to use. Please follow a example bellow.

```
public class CubeBehaviour
{
}

public class CubePool : Pool<CubeBehaviour>
{
}

public class GameManager : DIContext
{
    [SerializeField] private CubeBehaviour cubePrefab = default;
    private CubePool cubePool;

    public override void Setup()
    {
        this.cubePool = new CubePool();
    }
    
    public override void Initialize()
    {
        var cubePoolPrespawnAmount = 50;
        this.cubePool.Initialize(this.cubePrefab, cubePoolPrespawnAmount);
        
        var instance = this.cubePool.Request(); // Get free item from pool
        instance.ReturnToPool();                // Return object into pool
    }
}
```

# (づ｡◕‿‿◕｡)づ 

## IF THESE HELP YOU FINISH PROJECT, PLEASE DONATE ME A COFFE CUP

PAYPAL: https://paypal.me/sandichhuu

ヾ(＠＾▽＾＠)ﾉ THANK YOU
