use strict;

my @types = qw(DateTime TimeSpan Decimal Double Guid Int16 Int32 Int64 Single String Image);

my %arithmetic = map { $_ => 1 } qw(Decimal Double Int16 Int32 Int64 Single);

my $header = <<EOT;
//
// Copyright (c) 2003-2006 Jaroslaw Kowalski <jaak\@jkowalski.net>
// Copyright (c) 2006-2014 Piotr Fusik <piotr\@fusik.info>
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

EOT

for my $rawtype (@types)
{
    open(OUT, ">Soql${rawtype}WrapperExpression.cs");

print OUT $header;
if ($rawtype eq "Image") {
    print OUT "using System.Drawing;\n";
}
else {
    print OUT "using System;\n";
}
print OUT <<EOT;

namespace Sooda.QL.TypedWrappers
{
    public class Soql${rawtype}WrapperExpression : SoqlTypedWrapperExpression
    {
        public Soql${rawtype}WrapperExpression()
        {
        }

        public Soql${rawtype}WrapperExpression(SoqlExpression innerExpression) : base(innerExpression) { }

        public static implicit operator Soql${rawtype}WrapperExpression(${rawtype} v)
        {
            return new Soql${rawtype}WrapperExpression(new SoqlLiteralExpression(v));
        }

        public static implicit operator Soql${rawtype}WrapperExpression(SoqlParameterLiteralExpression v)
        {
            return new Soql${rawtype}WrapperExpression(v);
        }

        public static SoqlBooleanExpression operator ==(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.Equal); }
        public static SoqlBooleanExpression operator !=(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.NotEqual); }

        public SoqlBooleanExpression In(params Soql${rawtype}WrapperExpression[] inExpressions)
        {
            SoqlExpressionCollection rhs = new SoqlExpressionCollection();
            foreach (Soql${rawtype}WrapperExpression e in inExpressions)
            {
                rhs.Add(e);
            }
            return new SoqlBooleanInExpression(this, rhs);
        }

        public SoqlBooleanExpression In(params ${rawtype}[] inExpressions)
        {
            SoqlExpressionCollection rhs = new SoqlExpressionCollection();
            foreach (${rawtype} e in inExpressions)
            {
                rhs.Add(new SoqlLiteralExpression(e));
            }
            return new SoqlBooleanInExpression(this, rhs);
        }

        public override bool Equals(object o) { return object.ReferenceEquals(this, o); }
        public override int GetHashCode() { return base.GetHashCode(); }
EOT
    if ($rawtype ne "Guid")
    {
        print OUT <<EOT ;
        public static SoqlBooleanExpression operator <=(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.LessOrEqual); }
        public static SoqlBooleanExpression operator >=(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.GreaterOrEqual); }
        public static SoqlBooleanExpression operator <(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.Less); }
        public static SoqlBooleanExpression operator >(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Sooda.QL.SoqlBooleanRelationalExpression(left, right, Sooda.QL.SoqlRelationalOperator.Greater); }
EOT
    }
    if ($arithmetic{$rawtype})
    {
        print OUT <<EOT ;
        public static Soql${rawtype}WrapperExpression operator +(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Soql${rawtype}WrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Add)); }
        public static Soql${rawtype}WrapperExpression operator -(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Soql${rawtype}WrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Sub)); }
        public static Soql${rawtype}WrapperExpression operator *(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Soql${rawtype}WrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Mul)); }
        public static Soql${rawtype}WrapperExpression operator /(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Soql${rawtype}WrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Div)); }
        public static Soql${rawtype}WrapperExpression operator %(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Soql${rawtype}WrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Mod)); }
EOT
    }
    if ($rawtype eq "String")
    {
        print OUT <<EOT ;
        public static Soql${rawtype}WrapperExpression operator +(Soql${rawtype}WrapperExpression left, Soql${rawtype}WrapperExpression right) { return new Soql${rawtype}WrapperExpression(new Sooda.QL.SoqlBinaryExpression(left, right, Sooda.QL.SoqlBinaryOperator.Add)); }
        public SoqlBooleanExpression Like(SoqlStringWrapperExpression likeExpression) { return new Sooda.QL.SoqlBooleanRelationalExpression(this, likeExpression, Sooda.QL.SoqlRelationalOperator.Like); }
EOT

    }
print OUT <<EOT ;
    }

}
EOT
close(OUT);

    open(OUT, ">SoqlNullable${rawtype}WrapperExpression.cs");

print OUT <<EOT ;
$header
namespace Sooda.QL.TypedWrappers
{
    public class SoqlNullable${rawtype}WrapperExpression : Soql${rawtype}WrapperExpression
    {
        public SoqlNullable${rawtype}WrapperExpression() { }
        public SoqlNullable${rawtype}WrapperExpression(SoqlExpression innerExpression) : base(innerExpression) { }

        public SoqlBooleanExpression IsNull() { return new SoqlBooleanIsNullExpression(this, false); }
        public SoqlBooleanExpression IsNotNull() { return new SoqlBooleanIsNullExpression(this, true); }
    }
}
EOT
close(OUT);

}
