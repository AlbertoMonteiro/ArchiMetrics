// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISolutionEdgeItemsRepositoryConfig.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ISolutionEdgeItemsRepositoryConfig type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.Common.Structure
{
	using System.ComponentModel;

	public interface ISolutionEdgeItemsRepositoryConfig : INotifyPropertyChanged
	{
		string Path { get; set; }

		EdgeSource Source { get; set; }

		bool IncludeCodeReview { get; set; }
	}
}