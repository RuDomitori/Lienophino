namespace Lienophino.Tests.Ordering;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)] 
public class DependenceOnAttribute: Attribute
{
    public string MethodName { get; }

    public DependenceOnAttribute(string methodName)
    {
        MethodName = methodName;
    }
}