// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CyclomaticComplexityCounterTests.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the CyclomaticComplexityCounterTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ArchiMetrics.Analysis.Tests.Metrics
{
    using System.Linq;
    using System.Threading.Tasks;
    using ArchiMetrics.Analysis.Metrics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Xunit;
    using m = metrics;

    public sealed class CyclomaticComplexityCounterTests
    {
        private CyclomaticComplexityCounterTests()
        {
        }

        public class GivenACyclomaticComplexityAnalyzer
        {
            private readonly CyclomaticComplexityCounter _counter;

            public GivenACyclomaticComplexityAnalyzer()
            {
                _counter = new CyclomaticComplexityCounter();
            }

            [Theory]
            [InlineData("public abstract void DoSomething();", 1)]
            [InlineData("void DoSomething();", 1)]
            [InlineData("void DoSomething(){ var x = a && b; }", 2)]
            [InlineData(@"public void DoSomething(){
	try
	{
		var x = 1 + 2;
		var y = x + 2;
	}
	catch
	{
		throw new Exception();
	}
}", 2)]
            [InlineData(@"public void DoSomething(){
	try
	{
		var x = 1 + 2;
		var y = x + 2;
	}
	catch(ArgumentNullException ane)
	{
		throw new Exception();
	}
	catch(OutOfRangeException ane)
	{
		throw new Exception();
	}
}", 3)]
            [InlineData(@"public void DoSomething(){
	if(x == 1)
	{
		var y = x + 2;
	}
	else
	{		
		var y = 1 + 2;
	}
}", 2)]
            [InlineData(@"public int DoSomething(){
	switch(x){
		case ""a"": return 1;
		case ""b"": return 2;
		default: return 0;
	}
}", 3)]
            [InlineData(@"public int DoSomething(){
	var x = a > 2 ? 1 : 0;
	}
}", 2)]
            [InlineData(@"public int DoSomething(){
	var x = a ?? new object();
	}
}", 2)]
            [InlineData(@"public int DoSomething(){
		var numbers = new[] { 1, 2, 3 };
		var n = numbers.Where(n => n != 1).AsArray();
	}
}", 1)]
            [InlineData(@"public int DoSomething(){
		var numbers = new[] { 1, 2, 3 };
		var odds = numbers.Where(n => { if(n != 1) { return n %2 == 0; } else { return false; } }).AsArray();
	}
}", 3)]
            [InlineData(@"
namespace MyNs
{
	using System;
	using System.Threading.Tasks;

	public class MyClass
	{
		public void DoSomething()
		{
				var task = Task.Factory.StartNew(() => { Console.WriteLine(""blah""); });
		}
	}
}", 1)]
            public void MethodHasExpectedComplexity(string method, int expectedComplexity)
            {
                var tree = CSharpSyntaxTree.ParseText(method);
                var compilation = CSharpCompilation.Create(
                    "x",
                    syntaxTrees: new[] {tree},
                    references:
                    new MetadataReference[]
                    {
                        MetadataReference.CreateFromFile(typeof (object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof (Task).Assembly.Location)
                    });
                    //options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false, null, null, null, new string[] { "System", "System.Threading.Tasks" }));

                var model = compilation.GetSemanticModel(tree, true);
                var syntaxNode = tree
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .First();

                var metrics = new m.Metrics();
                var timer = metrics.Timer(
                    typeof(ProjectMetricTests),
                    "TestTimer",
                    m.TimeUnit.Milliseconds,
                    m.TimeUnit.Microseconds);

                var result = timer.Time(() => _counter.Calculate(syntaxNode, model));

                Assert.Equal(expectedComplexity, result);
            }

            [InlineData(@"namespace MyNs
{
	public class MyClass
	{
		private EventHandler _innerHandler;

		public event EventHandler MyEvent
		{
			add { _innerHandler += value; }
			remove { _innerHandler -= value; }
		}
	}
}", 1)]
            public void EventAddAccessorHasExpectedComplexity(string code, int expectedComplexity)
            {
                var tree = CSharpSyntaxTree.ParseText(code);
                var compilation = CSharpCompilation.Create(
                    "x",
                    syntaxTrees: new[] { tree },
                    references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location), MetadataReference.CreateFromFile(typeof(Task).Assembly.Location) },
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, usings: new[] { "System", "System.Threading.Tasks" }));

                var model = compilation.GetSemanticModel(tree, true);
                var syntaxNode = tree
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<AccessorDeclarationSyntax>()
                    .First();

                var result = _counter.Calculate(syntaxNode, model);

                Assert.Equal(expectedComplexity, result);
            }
        }
    }
}
