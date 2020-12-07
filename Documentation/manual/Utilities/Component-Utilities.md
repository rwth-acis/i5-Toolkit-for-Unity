# Component Utilities

## Use Case

Scripts can fetch references using the method `GetComponent`.
However, this method should be used only if necessary.
Therefore, scripts should cache the references once they are fetched.
The method <xref:i5.Toolkit.Core.Utilities.ComponentUtilities.EnsureComponentReference*> guarantees that a given component reference is set.

Moreover, there are also cases where a script needs to access a component which is not yet attached to the GameObject.
Components can be added using `AddComponent`.
However, before adding a component to a GameObject, one should check if it already exists to avoid duplicates.
The method <xref:i5.Toolkit.Core.Utilities.ComponentUtilities.GetOrAddComponent*> provides a shortcut to do these steps.

## Usage

### Ensure Component Reference

In order to cache a reference to another component in a script, create a variable for the component.
There is no need to initialize the reference in `Awake` or `Start`.
Whenever you access the variable with the reference, call <xref:i5.Toolkit.Core.Utilities.ComponentUtilities.EnsureComponentReference*> just before it.
After calling this method, the variable is ensured to be initialized with the reference.

```[C#]
private OtherComponent referenceToOther;
...
private void Foo()
{
    ComponentUtilities.EnsureComponentReference(gameObject, ref referenceToOther, true);
    referenceToOther.DoSomething();
}
```

- The first argument of the function is the GameObject on which the component should be searched.
- The second argument is the reference variable.
  *Important:* Provide the variable as a [`ref`](https://docs.microsoft.com/de-de/dotnet/csharp/language-reference/keywords/ref).
  This way, the variable can be set by the <xref:i5.Toolkit.Core.Utilities.ComponentUtilities.EnsureComponentReference*>.
- If the third argument is set to true, a new component will automatically be added in case it is not found.
  This also means that <xref:i5.Toolkit.Core.Utilities.ComponentUtilities.EnsureComponentReference*> will definitely initialize the reference variable.
  If it is set to false and the component cannot be found, the reference variable remains `null`.

*Hint:*

Encapsulate the reference variable in a property with a getter and call <xref:i5.Toolkit.Core.Utilities.ComponentUtilities.EnsureComponentReference*> before returning the reference.
After that, only use the property in the script to access the reference.
This way, you do not need to make sure that the reference is set every time it is used.
```[C#]
private OtherComponent ReferenceToOther
{
    private OtherComponent referenceToOther;
    get
    {
        ComponentUtilities.EnsureComponentReference(gameObject, ref referenceToOther, true);
        return referenceToOther;
    }
    ...
    private void Foo()
    {
        ReferenceToOther.DoSomething();
    }
}
```

### Get or Add Component

<xref:i5.Toolkit.Core.Utilities.ComponentUtilities.GetOrAddComponent*> combines `GetComponent` and `AddComponent`.
First it tries to get the component using `GetComponent`.
If the component cannot be found, it adds the component to the GameObject and returns the added instance.
Therefore, it will always return a component instance and cannot return `null`

```[C#]
OtherComponent referenceToOther = ComponentUtilities.GetOrAddComponent<OtherComponent>(gameObject);
```