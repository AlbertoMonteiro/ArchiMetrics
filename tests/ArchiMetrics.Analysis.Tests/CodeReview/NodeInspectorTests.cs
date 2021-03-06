﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeInspectorTests.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the NodeInspectorTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.Analysis.Tests.CodeReview
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.CodeReview;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Moq;
    using Xunit;

    public sealed class NodeInspectorTests
    {
        private NodeInspectorTests()
        {
        }

        public class GivenANodeInspector
        {
            private readonly Mock<ICodeEvaluation> _mockCodeEvaluation;
            private readonly NodeReviewer _reviewer;

            public GivenANodeInspector()
            {
                _mockCodeEvaluation = new Mock<ICodeEvaluation>();
                _mockCodeEvaluation.SetupGet(x => x.EvaluatedKind).Returns(SyntaxKind.ClassDeclaration);
                _mockCodeEvaluation.Setup(x => x.Evaluate(It.IsAny<SyntaxNode>())).Returns((EvaluationResult)null);
                _reviewer = new NodeReviewer(new[] { _mockCodeEvaluation.Object }, Enumerable.Empty<ISymbolEvaluation>());
            }

            [Fact]
            public async Task WhenEvaluatingCodeThenCallsCodeEvaluation()
            {
                var classDeclaration = SyntaxFactory.ClassDeclaration(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                    SyntaxFactory.Identifier("MyClass"),
                    SyntaxFactory.TypeParameterList(),
                    SyntaxFactory.BaseList(),
                    SyntaxFactory.List<TypeParameterConstraintClauseSyntax>(),
                    SyntaxFactory.List<MemberDeclarationSyntax>());
                await _reviewer.Inspect("name", string.Empty, classDeclaration, null, null).ConfigureAwait(false);

                _mockCodeEvaluation.Verify(x => x.Evaluate(classDeclaration));
            }
        }
    }
}
