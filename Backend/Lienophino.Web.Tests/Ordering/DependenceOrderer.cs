using Xunit.Abstractions;
using Xunit.Sdk;

namespace Lienophino.Web.Tests.Ordering;

public sealed class DependenceOrderer: ITestCaseOrderer
{
    private sealed class Vertex<TTestCase>
        where TTestCase : ITestCase
    {
        internal TTestCase TestCase;
        internal List<string> DependenceOnMethodNames;
        internal VertexState State = VertexState.NotVisited;
    }
    
    private enum VertexState
    {
        NotVisited,
        Visited,
        Added
    }
    
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) 
        where TTestCase : ITestCase
    {
        var vertexes = new Dictionary<string, Vertex<TTestCase>>();
        
        foreach (var testCase in testCases)
        {
            var newVertex = new Vertex<TTestCase>
            {
                TestCase = testCase,
                DependenceOnMethodNames = testCase.TestMethod.Method
                    .GetCustomAttributes(typeof(DependenceOnAttribute))
                    .Select(x => x.GetConstructorArguments().First() as string)
                    .ToList()
            };
            var methodName = testCase.TestMethod.Method.Name;
            if (vertexes.ContainsKey(methodName))
                throw new InvalidOperationException(
                    "Several method with same name are found." +
                    " This corner case is not supported by DependenceOrderer." +
                    $" Method name: {methodName}"
                );

            vertexes.Add(testCase.TestMethod.Method.Name, newVertex);
        }
        
        var resultOrderedList = new List<TTestCase>(vertexes.Count);
        foreach (var vertex in vertexes.Values)
        {
            void VisitVertex(Vertex<TTestCase> vertex)
            {
                if (vertex.State == VertexState.Added) return;
            
                if (vertex.State == VertexState.Visited)
                    // TODO: Add a cycle trail into the exception message
                    throw new InvalidOperationException("A cycle in the dependencies graph is found");
                
                vertex.State = VertexState.Visited;
                
                foreach (var dependenceOnMethodName in vertex.DependenceOnMethodNames)
                {
                    if (vertexes.TryGetValue(dependenceOnMethodName, out var dependenceOnVertex))
                        VisitVertex(dependenceOnVertex);
                    else
                        throw new InvalidOperationException(
                            // TODO: Add more info into the exception message
                            $"Method with name \"{dependenceOnMethodName}\" is not found"
                        );
                }

                resultOrderedList.Add(vertex.TestCase);
                vertex.State = VertexState.Added;
            }
            
            VisitVertex(vertex);
        }

        return resultOrderedList;
    }
}