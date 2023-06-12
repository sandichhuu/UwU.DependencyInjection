# UωU ![Alt text](https://github.com/vohuu/Assets/blob/main/vnico16.png?raw=true) UNITY DEPENDENCY INJECTION


# (づ｡◕‿‿◕｡)づ 

## 1. Getting Started
Download the latest .unitypackage from the releases page and open it within your project. Install it as you would for any other package.


## 2. Why and How
The traditional way references between objects instances in Unity project is drag and drop.
That way is easy to understand but the project come crazy if too big or some mistake make all references gones.
And that is why this project is exist.

The <strong>UwU.DependencyInjection</strong> module keep all registered references.
And provide references when the object is needed.
That's is how it work.

## 3. Binding
### 3.1 BindCommand
Binding the pure C# object.

```csharp
public class GameManager : UnityContext
{
    public override void Setup()
    {
        var instance = new PureCs();
        this.binder.BindCommand<PureCs>(instance);  // Bind the instance as PureCs type.
        this.binder.ExecuteBindingCommand();        // This line is procedure.
    }
}
```

### 3.2 BindComponentCommand
Binding the existing instance from hierarchy.

```csharp
public class GameManager : UnityContext
{
    public override void Setup()
    {
        this.binder.BindComponentCommand<MonoObject>();     // Find MonoObject from hierachy and bind that instance as MonoObject.
        this.binder.ExecuteBindingCommand();                // This line is procedure.
    }
}
```

### 3.3 BindComponentRelevantsCommand
Find the component from hierarchy, bind all type have references to the class.
This command is rarely to use, because it's not clear function.
This make developer have to remember or looking for the type, the implemented interfaces, ... to understand what are binded.
```csharp
public class ExampleClass : MonoBehaviour, ILoop {...}

public class GameManager : UnityContext
{
    public override void Setup()
    {
        /* 
            Bind the instance from hierarchy to these types: System.Object, UnityEngine.Object, 
            UnityEngine.Component, UnityEngine.MonoBehaviour, ILoop.
        */
        this.binder.BindComponentRelevantsCommand<ExampleClass>();
        this.binder.ExecuteBindingCommand(); // This line is procedure.
    }
}
```

On the case have unwanted binding type, we can filtered it out.
```csharp
public class ExampleClass : MonoBehaviour, ILoop {...}

public class GameManager : UnityContext
{
    public override void Setup()
    {
        /* 
            Bind the instance from hierarchy to these types: ILoop 
        */
        var filterNamespaces = new string[]
        {
            "System", "UnityEngine"
        };
        this.binder.BindComponentRelevantsCommand<ExampleClass>(filterNamespaces); 
        this.binder.ExecuteBindingCommand(); // This line is procedure.
    }
}
```

### 3.4 BindRelevantsTypeCommand
This function have logic the same as <strong>"BindComponentRelevantsCommand"</strong> but working with only pure C# object.
```csharp
public class PureCs : ILoop, IFixedLoop {...}

public class GameManager : UnityContext
{
    public override void Setup()
    {
        var instance = new PureCs();
    
        /* 
            Bind the instance from hierarchy to these types: System.Object, ILoop, IFixedLoop.
        */
        this.binder.BindRelevantsTypeCommand(instance);
        this.binder.ExecuteBindingCommand(); // This line is procedure.
    }
}
```

On the case have unwanted binding type, we can filtered it out.
```csharp
public class PureCs : ILoop, IFixedLoop {...}

public class GameManager : UnityContext
{
    public override void Setup()
    {
        var instance = new PureCs();
    
        /* 
            Bind the instance from hierarchy to these types: ILoop, IFixedLoop
        */
        var filterNamespaces = new string[]
        {
            "System"
        };
        this.binder.BindRelevantsTypeCommand(instance, filterNamespaces); 
        this.binder.ExecuteBindingCommand(); // This line is procedure.
    }
}
```

### 3.5 Unbind
On the case we destroy the object on scene, or swap scene and don't need the reference to the object anymore.
We can use this function to clear the references.

```csharp
public class GameManager : UnityContext
{
    public void OnSwapScene()
    {
        this.binder.Unbind<PureCs>(this.pureCsInstance);    
        this.binder.Unbind<MonoObject>();
    }
}
```

### 3.6 ExecuteBindingCommand
All command is just a command, it's not executed.
And this function will make all registered command executing.

```csharp
public class GameManager : UnityContext
{
    public override void Setup()
    {
        var instance = new PureCs();
        var objectOnScene = FindObjectOfType<HelloWorld>();
        
        this.binder.BindCommand<PureCs>(instance);
        this.binder.BindCommand<HelloWorld>(objectOnScene);
        this.binder.BindComponentCommand<GoodbyeWorld>();
        
        this.binder.ExecuteBindingCommand(); // After this line, 3 above command will be executed.
    }
}
```

## 4. Injection
After binding phase, the references is ready to use now.
After function ".Inject()" is called, the fields, properties have [Inject] attribute will be injected

```csharp
public class GameManager : UnityContext
{
    [Inject] private readonly ShapeManager shapeManager;

    private ILoop shapeLooper;

    public override void Setup()
    {
        this.binder.BindComponentCommand<ShapeManager>();   // Find component ShapeManager from hierarchy and bind.
        this.binder.ExecuteBindingCommand();                // Do all command above this line.

        this.Inject();                                      // Inject ShapeManager to GameManager (this instance).
    }

    public override void Initialize()
    {
        this.shapeManager.Inject();                         // Help shape manager have reference to box and sphere.
        this.shapeManager.Setup();                          // This function is replaced Start function.
        this.shapeLooper = this.shapeManager;
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        this.shapeLooper.Loop(deltaTime);
    }
}
```

## 5. Extra
This module support not only injection registered dependencies.
We can have references on scene but with the easier way.

```csharp
public class ShapeManager : MonoBehaviour, ILoop
{
    [GetComponent] private readonly ShapeManager self;
    [FindObjectOfType] private readonly BoxBehaviour box;
    [GetComponentInChildren] private readonly SphereBehaviour sphere;

    public void Setup()
    {
        this.box.transform.position = new Vector3(-5.5f, 0, 0);
        this.sphere.transform.position = new Vector3(+5.5f, 0, 0);

        this.box.Setup();
        this.sphere.Setup();
    }

    void ILoop.Loop(float dt)
    {
        this.box.Loop(dt);
        this.sphere.Loop(dt);
    }
}
```

## The package is shipped with extra packages: 
UwU.Pool<br />
UwU.ObserverSystem<br />
UwU.BezierSolver<br />
...

## ಠ_ಠ Use it but not donate, lucky -999 points.
PAYPAL: https://paypal.me/sandichhuu
