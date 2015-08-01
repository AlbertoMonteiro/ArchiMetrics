// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoslynTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RoslynTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.Common.Tests
{
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.CodeAnalysis.MSBuild;
	using NUnit.Framework;

	public class RoslynTests
	{
		[Test]
		public async Task WhenLoadingSolutionThenHasProjects()
		{
#if NCRUNCH
            var directoryName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(GetOriginalProjectPath()), "bin", "Debug");
            System.IO.Directory.SetCurrentDirectory(directoryName); 
#endif
			var path = @"..\..\..\archimetrics.sln".GetLowerCaseFullPath();
			var workspace = MSBuildWorkspace.Create();
			var solution = await workspace.OpenSolutionAsync(path);

			Assert.True(solution.Projects.Any());
		}

#if NCRUNCH
        public static string GetOriginalProjectPath()
        {
            return System.Environment.GetEnvironmentVariable("NCrunch.OriginalProjectPath");
        } 
#endif
	}
}
