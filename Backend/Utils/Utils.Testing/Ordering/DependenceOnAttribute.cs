namespace Utils.Testing.Ordering;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)] 
public sealed class DependenceOnAttribute: Attribute
{
    public string MethodName { get; }

    public DependenceOnAttribute(string methodName)
    {
        MethodName = methodName;
    }
}