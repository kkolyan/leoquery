# About

Sugar facade for [LeoECS Lite](https://github.com/Leopotam/ecslite).

## Goals
* Reduce boilerplate (particularly related to pools, worlds and filters)
* Introduce static typed filters (as in [LeoECS Classic](https://github.com/Leopotam/ecs))
* Keep near-to-zero heap allocations
* Keep design simple

## Known issues 

### CPU Overhead
It copies structs more that LeoECS, so CPU load is higher. Though it may be improved for some amount 
in future, some performance sacrifice is a conscious choice. 

### Multi-worlds
LeoECS Lite requires manual world-entity distruibution management to reach maximum performance, because 
single-world approach may show performance degradation in projects with tons of single-component entities (aka "events").

This library is intended to keep world-entity management under the hood with agile semi-automatic configuration or fully automatic.

Current implementation doesn't bother about it at all, but future development is about two non-conflicting approaches:
* Attribute-based hinting on component level (semi-automatic, because attributes should be placed by user)
* Dynamic on-fly management (fully automatic)

# Installation

## As unity module
This repository can be installed as unity module directly from git url. In this way new line should be added to `Packages/manifest.json`:
```c#
"com.leopotam.ecslite": "https://github.com/Leopotam/ecslite.git",
"com.nplekhanov.csx.leoquery": "https://github.com/kkolyan/leoquery.git",
```

## As source
If you can't / don't want to use unity modules, code can be cloned.

# Usage

Note that this short guide may miss information regarding corner cases. Please read LeoECS README if you need better understanding what happens under the hood..


## Systems
Systems area C# class that implements `ISystem` and contain little bit of game logic. Supposed that whole game behavior is composed from a dozens of such classes
```c#
public class System1 : ISystem
{
    public void Act(IEntityStorage storage)
    {
        // any logic here
    }
}
```

## External services
If you want to call some logic that for some reason lives outside ECS systems, then yoy might use [DI](https://en.wikipedia.org/wiki/Dependency_injection) to access external objects
```c#
public class System1 : ISystem
{
    [Inject] private Service1 _service1; // initialized using reflection on startup
    
    public void Act(IEntityStorage storage)
    {
        // use _service1 here
    }
}
```

## Entry point
```c#
    
    ISystem _mainPipeline;
    IEntityStorage _storage;
    
    void Start()
    {
        // storage is container for entities and components
        _storage = new LeoLiteStorage();
        
        // reflection+attributes based DI agent. that's completely optional.
        Injector di = new Injector()
            .AddDependency(new Service1())
            .AddDependency(new Service2());
        
        _mainPipeline = new MulticastSystem()
            // add systems. order matters.
            .Add(new System1())
            .Add(new System2())
            // inject dependencies (optional step)
            .ForEach(di.InjectInto);
    }
    
    void Update()
    {
        // _mainPipeline is calls Act on each system inside
        _mainPipeline.Act(_storage);
    }
```

## Components

Components should be plain C# structs:
```c#
public struct Component1 {
    public int value;
}
```

## Entities

### Basic operations
```c#
// create empty entity
Entity a = storage.NewEntity();

// add a component
a.Add(new Comp1()); // fails if component of this type is already attached to entity.

// note that there is no double-logic methods like "create-or-update" or "delete if present" here.
// assumed that one can easily add them using "C# Extension Methods" if necessary.

// create entity and initialize it in fluent way
storage.NewEntity()
    .Add(new Comp1())
    .Add(new Comp2());
    
// modify component.
a.Get<Comp1>().value = 42; // fails if there is no such component

// delete component
a.Del<Comp1>(); // fails if there is no such component

// check presense of component
if (a.Has<Comp1>()) {
    
}

a.Destroy();
```

### Querying
```c#
foreach (Entity<Comp1> entity in storage.Query<Comp1>()) {
    // note that Entity is generified with the component that it guaranteed to have. 
    // this mandatory component is accessed in safe way using positional getter:
    entity.Get1().value = 42;
    
    // generic Entity<> also provide all other methods desribed in previous section:    
    if (entity.Has<Comp2>()) {
        entity.Get<Comp1>().value = 17;
    }
}

// entities that have both components (like SQL INNER JOIN)
foreach (Entity<Comp1, Comp2> entity in storage.Query<Comp1, Comp2>()) {
    // copy value from Comp1 to Comp2
    entity.Get2().value = entity.Get1().value * 2;
}


// skip entities that has Comp3 component. note that this feature is discouraged in well-designed programs.
// it exist as a hack for quick fixes and legacy programs maintaining.
foreach (Entity<Comp1, Comp2> entity in storage.Query<Component, Comp2>().Excluding<Comp3>()) {
    entity.Add<Comp3>();// will never fail here
}

```
