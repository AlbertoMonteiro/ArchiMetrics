// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TfsDefinition.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the TfsDefinition type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.Common
{
	using System.Xml.Serialization;

	public class TfsDefinition
	{
		[XmlAttribute]
		public string Definition { get; set; }
	}
}