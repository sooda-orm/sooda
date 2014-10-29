//
// Copyright (c) 2003-2006 Jaroslaw Kowalski <jaak@jkowalski.net>
// Copyright (c) 2006-2014 Piotr Fusik <piotr@fusik.info>
//
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// * Redistributions of source code must retain the above copyright notice,
//   this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//

// automatically generated by makewrappers.pl - do not modify

using System;
using System.Drawing;

namespace Sooda.QL.TypedWrappers
{
    public class SoqlSingleWrapperExpression : SoqlTypedWrapperExpression
    {
        public SoqlSingleWrapperExpression()
        {
        }

        public SoqlSingleWrapperExpression(SoqlExpression innerExpression) : base(innerExpression) { }

        public static implicit operator SoqlSingleWrapperExpression(Single v)
        {
            return new SoqlSingleWrapperExpression(new SoqlLiteralExpression(v));
        }

        public static implicit operator SoqlSingleWrapperExpression(SoqlParameterLiteralExpression v)
        {
            return new SoqlSingleWrapperExpression(v);
        }

        public static SoqlBooleanExpression operator ==(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.Equal); }
        public static SoqlBooleanExpression operator !=(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.NotEqual); }

        public SoqlBooleanExpression In(params SoqlSingleWrapperExpression[] inExpressions)
        {
            SoqlExpressionCollection rhs = new SoqlExpressionCollection();
            foreach (SoqlSingleWrapperExpression e in inExpressions)
            {
                rhs.Add(e);
            }
            return new SoqlBooleanInExpression(this, rhs);
        }

        public SoqlBooleanExpression In(params Single[] inExpressions)
        {
            SoqlExpressionCollection rhs = new SoqlExpressionCollection();
            foreach (Single e in inExpressions)
            {
                rhs.Add(new SoqlLiteralExpression(e));
            }
            return new SoqlBooleanInExpression(this, rhs);
        }

        public override bool Equals(object o) { return Object.ReferenceEquals(this, o); }
        public override int GetHashCode() { return base.GetHashCode(); }
        public static SoqlBooleanExpression operator <=(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.LessOrEqual); }
        public static SoqlBooleanExpression operator >=(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.GreaterOrEqual); }
        public static SoqlBooleanExpression operator <(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.Less); }
        public static SoqlBooleanExpression operator >(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.Greater); }
        public static SoqlSingleWrapperExpression operator +(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new SoqlSingleWrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Add)); }
        public static SoqlSingleWrapperExpression operator -(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new SoqlSingleWrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Sub)); }
        public static SoqlSingleWrapperExpression operator *(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new SoqlSingleWrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Mul)); }
        public static SoqlSingleWrapperExpression operator /(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new SoqlSingleWrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Div)); }
        public static SoqlSingleWrapperExpression operator %(SoqlSingleWrapperExpression left, SoqlSingleWrapperExpression right) { return new SoqlSingleWrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Mod)); }
    }

}
