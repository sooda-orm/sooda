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

using Sooda.Schema;
using System;
using System.CodeDom;

namespace Sooda.CodeGen
{
    public class CodeDomClassStubGenerator : CodeDomHelpers
    {
        readonly ClassInfo classInfo;
        readonly SoodaProject options;
        public readonly string KeyGen;

        public CodeDomClassStubGenerator(ClassInfo ci, SoodaProject options)
        {
            this.classInfo = ci;
            this.options = options;
            string keyGen = "none";

            if (!ci.ReadOnly && ci.GetPrimaryKeyFields().Length == 1)
            {
                switch (ci.GetFirstPrimaryKeyField().DataType)
                {
                    case FieldDataType.Integer:
                        keyGen = "integer";
                        break;

                    case FieldDataType.Guid:
                        keyGen = "guid";
                        break;

                    case FieldDataType.Long:
                        keyGen = "long";
                        break;
                }
            }

            if (ci.KeyGenName != null)
                keyGen = ci.KeyGenName;

            this.KeyGen = keyGen;
        }

        private ClassInfo GetRootClass(ClassInfo ci)
        {
            if (ci.InheritsFromClass != null)
                return GetRootClass(ci.InheritsFromClass);
            else
                return ci;
        }

        public CodeMemberField Field_keyGenerator()
        {
            CodeMemberField field = new CodeMemberField("IPrimaryKeyGenerator", "keyGenerator");
            field.Attributes = MemberAttributes.Private | MemberAttributes.Static;

            switch (KeyGen)
            {
                case "guid":
                    field.InitExpression = new CodeObjectCreateExpression("Sooda.ObjectMapper.KeyGenerators.GuidGenerator");
                    break;

                case "integer":
                    field.InitExpression = new CodeObjectCreateExpression("Sooda.ObjectMapper.KeyGenerators.TableBasedGenerator",
                        new CodePrimitiveExpression(GetRootClass(classInfo).Name),
                        new CodeMethodInvokeExpression(
                        new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(options.OutputNamespace.Replace(".", "") + "." + "_DatabaseSchema"), "GetSchema"),
                        "GetDataSourceInfo",
                        new CodePrimitiveExpression(classInfo.GetSafeDataSourceName())));
                    break;
                case "long":
                    field.InitExpression = new CodeObjectCreateExpression("Sooda.ObjectMapper.KeyGenerators.TableBasedGeneratorBigint",
                        new CodePrimitiveExpression(GetRootClass(classInfo).Name),
                        new CodeMethodInvokeExpression(
                        new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(options.OutputNamespace.Replace(".", "") + "." + "_DatabaseSchema"), "GetSchema"),
                        "GetDataSourceInfo",
                        new CodePrimitiveExpression(classInfo.GetSafeDataSourceName())));
                    break;

                default:
                    field.InitExpression = new CodeObjectCreateExpression(KeyGen);
                    break;
            }
            return field;
        }

        public CodeMemberMethod Method_TriggerFieldUpdate(FieldInfo fi, string methodPrefix)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = methodPrefix + "_" + fi.Name;
            if (fi.References != null)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(fi.References, "oldValue"));
                method.Parameters.Add(new CodeParameterDeclarationExpression(fi.References, "newValue"));
            }
            else
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "oldValue"));
                method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "newValue"));
            }
            method.Attributes = MemberAttributes.Family;
            method.Statements.Add(new CodeMethodInvokeExpression(This, methodPrefix, new CodePrimitiveExpression(fi.Name), Arg("oldValue"), Arg("newValue")));
            return method;
        }

        public CodeConstructor Constructor_Raw()
        {
            CodeConstructor ctor = new CodeConstructor();

            ctor.Attributes = MemberAttributes.Public;
            ctor.Parameters.Add(new CodeParameterDeclarationExpression("SoodaConstructor", "c"));
            ctor.BaseConstructorArgs.Add(Arg("c"));

            return ctor;
        }

        public CodeConstructor Constructor_Mini_Inserting()
        {
            CodeConstructor ctor = new CodeConstructor();

            ctor.Attributes = MemberAttributes.Public;
            ctor.Parameters.Add(new CodeParameterDeclarationExpression("SoodaTransaction", "tran"));
            ctor.BaseConstructorArgs.Add(Arg("tran"));

            return ctor;
        }

        public CodeMemberProperty Prop_LiteralValue(string name, object val)
        {
            CodeMemberProperty prop = new CodeMemberProperty();
            prop.Name = name;
            prop.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            prop.Type = new CodeTypeReference(classInfo.Name);
            //prop.CustomAttributes.Add(NoStepThrough());
            prop.GetStatements.Add(
                new CodeMethodReturnStatement(
                new CodeMethodInvokeExpression(
                LoaderClass(classInfo), "GetRef", new CodePrimitiveExpression(val))));

            return prop;
        }

        private CodeTypeReferenceExpression LoaderClass(ClassInfo ci)
        {
            if (options.LoaderClass)
                return new CodeTypeReferenceExpression(ci.Name + "Loader");
            else
                return new CodeTypeReferenceExpression(ci.Name + "_Stub");
        }

        public CodeTypeReference GetReturnType(PrimitiveRepresentation rep, FieldInfo fi)
        {
            switch (rep)
            {
                case PrimitiveRepresentation.Boxed:
                    return new CodeTypeReference(typeof(object));

                case PrimitiveRepresentation.SqlType:
                    Type t = fi.GetNullableFieldHandler().GetSqlType();
                    if (t == null)
                        return new CodeTypeReference(fi.GetNullableFieldHandler().GetFieldType());
                    else
                        return new CodeTypeReference(t);

                case PrimitiveRepresentation.RawWithIsNull:
                case PrimitiveRepresentation.Raw:
                    return new CodeTypeReference(fi.GetNullableFieldHandler().GetFieldType());

                case PrimitiveRepresentation.Nullable:
                    return new CodeTypeReference(fi.GetNullableFieldHandler().GetNullableType());

                default:
                    throw new NotImplementedException("Unknown PrimitiveRepresentation: " + rep);
            }
        }

        static CodeExpression GetFieldValueExpression(FieldInfo fi)
        {
            return new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression(typeof(Sooda.ObjectMapper.SoodaObjectImpl)),
                "GetBoxedFieldValue",
                new CodeThisReferenceExpression(),
                new CodePrimitiveExpression(fi.Table.OrdinalInClass),
                new CodePrimitiveExpression(fi.ClassUnifiedOrdinal));
        }

        static CodeExpression GetFieldIsNullExpression(FieldInfo fi)
        {
            return new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression(typeof(Sooda.ObjectMapper.SoodaObjectImpl)),
                "IsFieldNull",
                new CodeThisReferenceExpression(),
                new CodePrimitiveExpression(fi.Table.OrdinalInClass),
                new CodePrimitiveExpression(fi.ClassUnifiedOrdinal)
                );
        }

        static CodeMemberProperty _IsNull(FieldInfo fi)
        {
            CodeMemberProperty prop = new CodeMemberProperty();
            prop.Name = fi.Name + "_IsNull";
            prop.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            prop.Type = new CodeTypeReference(typeof(bool));

            prop.GetStatements.Add(
                new CodeMethodReturnStatement(
                GetFieldIsNullExpression(fi)));

            return prop;
        }

        static CodeExpression Box(CodeExpression expr)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(SoodaNullable)), "Box", expr);
        }

        static CodeMemberMethod _SetNull(FieldInfo fi)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = "_SetNull_" + fi.Name;
            method.Attributes = MemberAttributes.Final | MemberAttributes.Public;

            method.Statements.Add(
                new CodeAssignStatement(
                GetFieldValueExpression(fi), new CodePrimitiveExpression(null)));

            return method;
        }

        static CodeExpression GetTransaction()
        {
            return new CodeMethodInvokeExpression(This, "GetTransaction");
        }

        static CodeExpression GetFieldValueForRead(FieldInfo fi)
        {
            CodeExpression fieldValues = new CodeMethodInvokeExpression(
                new CodeThisReferenceExpression(),
                "Get" + fi.ParentClass.Name + "FieldValuesForRead",
                new CodePrimitiveExpression(fi.Table.OrdinalInClass));
            if (fi.ParentClass.GetDataSource().EnableDynamicFields)
            {
                return new CodeMethodInvokeExpression(
                    fieldValues,
                    "GetBoxedFieldValue",
                    new CodePrimitiveExpression(fi.ClassUnifiedOrdinal));
            }
            return new CodeFieldReferenceExpression(fieldValues, fi.Name);
        }

        static int GetFieldRefCacheIndex(ClassInfo ci, FieldInfo fi0)
        {
            int p = 0;

            foreach (FieldInfo fi in ci.LocalFields)
            {
                if (fi == fi0)
                    return p;
                if (fi.ReferencedClass != null)
                    p++;
            }

            return -1;
        }

        static int GetFieldRefCacheCount(ClassInfo ci)
        {
            int p = 0;

            foreach (FieldInfo fi in ci.LocalFields)
            {
                if (fi.ReferencedClass != null)
                    p++;
            }

            return p;
        }

        static CodeExpression RefCacheArray()
        {
            return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_refcache");
        }

        static CodeExpression RefCacheExpression(ClassInfo ci, FieldInfo fi)
        {
            return new CodeArrayIndexerExpression(RefCacheArray(),
                new CodePrimitiveExpression(GetFieldRefCacheIndex(ci, fi)));
        }

        static CodeExpression Factory(string className)
        {
            return new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(className + "_Factory"), "TheFactory");
        }

        CodeTypeReference GetCollectionPropertyType(string className)
        {
            if (options.WithSoql)
                return new CodeTypeReference(className + "List");
            else
                return new CodeTypeReference("System.Collections.Generic.IList", new CodeTypeReference(className));
        }

        CodeTypeReference GetCollectionWrapperType(string className)
        {
            if (options.WithSoql)
                return new CodeTypeReference(options.OutputNamespace + "." + className + "List");
            else
                return new CodeTypeReference("Sooda.ObjectMapper.SoodaObjectCollectionWrapperGeneric", new CodeTypeReference(className));
        }

#if DOTNET35
        static CodeMemberProperty GetCollectionLinqQuery(CollectionBaseInfo coli, CodeExpression whereExpression)
        {
            string elementType = coli.GetItemClass().Name;
            CodeMemberProperty prop = new CodeMemberProperty();
            prop.Name = coli.Name + "Query";
            prop.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            prop.Type = new CodeTypeReference(new CodeTypeReference(typeof(System.Linq.IQueryable<>)).BaseType, new CodeTypeReference(elementType));

            prop.GetStatements.Add(
                new CodeMethodReturnStatement(
                new CodeObjectCreateExpression(
                    new CodeTypeReference(new CodeTypeReference(typeof(Sooda.Linq.SoodaQuerySource<>)).BaseType, new CodeTypeReference(elementType)),
                    new CodeMethodInvokeExpression(This, "GetTransaction"),
                    new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(elementType + "_Factory"), "TheClassInfo"),
                    whereExpression
                )));

            return prop;
        }
#endif

        public void GenerateProperties(CodeTypeDeclaration ctd, ClassInfo ci)
        {
            CodeMemberProperty prop;

            foreach (FieldInfo fi in classInfo.LocalFields)
            {
                if (fi.References != null)
                    continue;

                if (fi.IsNullable)
                {
                    if (options.NullableRepresentation == PrimitiveRepresentation.RawWithIsNull)
                    {
                        ctd.Members.Add(_IsNull(fi));
                        if (!ci.ReadOnly)
                        {
                            ctd.Members.Add(_SetNull(fi));
                        }
                    }
                }
                else
                {
                    if (options.NotNullRepresentation == PrimitiveRepresentation.RawWithIsNull)
                    {
                        if (!ci.ReadOnly)
                        {
                            // if it's read-only, not-null means not-null and there's no
                            // exception
                            ctd.Members.Add(_IsNull(fi));
                        }
                    }
                }
            }

            int primaryKeyComponentNumber = 0;

            foreach (FieldInfo fi in classInfo.LocalFields)
            {
                PrimitiveRepresentation actualNullableRepresentation = options.NullableRepresentation;
                PrimitiveRepresentation actualNotNullRepresentation = options.NotNullRepresentation;

                if (fi.GetNullableFieldHandler().GetSqlType() == null)
                {
                    if (actualNotNullRepresentation == PrimitiveRepresentation.SqlType)
                        actualNotNullRepresentation = PrimitiveRepresentation.Raw;

                    if (actualNullableRepresentation == PrimitiveRepresentation.SqlType)
                        actualNullableRepresentation = PrimitiveRepresentation.Raw;
                }

                CodeTypeReference returnType;

                //if (fi.Name == ci.PrimaryKeyFieldName)
                //{
                //  returnType = GetReturnType(PrimitiveRepresentation.Raw, fi.DataType);
                //}
                //else
                if (fi.References != null)
                {
                    returnType = new CodeTypeReference(fi.References);
                }
                else if (fi.IsNullable)
                {
                    returnType = GetReturnType(actualNullableRepresentation, fi);
                }
                else
                {
                    returnType = GetReturnType(actualNotNullRepresentation, fi);
                }

                prop = new CodeMemberProperty();
                prop.Name = fi.Name;
                prop.Attributes = MemberAttributes.Final | MemberAttributes.Public;
                prop.Type = returnType;
                //prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(null, "_FieldNames")));
                if (fi.Description != null)
                {
                    prop.Comments.Add(new CodeCommentStatement("<summary>", true));
                    prop.Comments.Add(new CodeCommentStatement(fi.Description, true));
                    prop.Comments.Add(new CodeCommentStatement("</summary>", true));
                }
                ctd.Members.Add(prop);

                if (fi.Size != -1)
                {
                    CodeAttributeDeclaration cad = new CodeAttributeDeclaration("Sooda.SoodaFieldSizeAttribute");
                    cad.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(fi.Size)));
                    prop.CustomAttributes.Add(cad);
                }

                if (fi.IsPrimaryKey)
                {
                    CodeExpression getPrimaryKeyValue = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "GetPrimaryKeyValue");

                    if (classInfo.GetPrimaryKeyFields().Length > 1)
                    {
                        getPrimaryKeyValue = new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(typeof(SoodaTuple)), "GetValue", getPrimaryKeyValue, new CodePrimitiveExpression(primaryKeyComponentNumber));
                    }

                    if (fi.References != null)
                    {
                        prop.GetStatements.Add(
                            new CodeMethodReturnStatement(
                            new CodeMethodInvokeExpression(
                                LoaderClass(fi.ReferencedClass),
                                "GetRef",
                                GetTransaction(),
                                new CodeCastExpression(
                                    GetReturnType(actualNotNullRepresentation, fi),
                                    getPrimaryKeyValue
                            ))));
                    }
                    else
                    {
                        prop.GetStatements.Add(
                            new CodeMethodReturnStatement(
                            new CodeCastExpression(
                            prop.Type,
                            getPrimaryKeyValue
                            )));
                    }

                    if (!classInfo.ReadOnly && !fi.ReadOnly)
                    {
                        if (classInfo.GetPrimaryKeyFields().Length == 1)
                        {
                            prop.SetStatements.Add(
                                new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(), "SetPrimaryKeyValue",
                                new CodePropertySetValueReferenceExpression())));
                        }
                        else
                        {
                            CodeExpression plainValue = new CodePropertySetValueReferenceExpression();
                            if (fi.References != null)
                                plainValue = new CodeMethodInvokeExpression(plainValue, "GetPrimaryKeyValue");
                            prop.SetStatements.Add(
                                new CodeExpressionStatement(
                                new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(), "SetPrimaryKeySubValue",
                                plainValue,
                                new CodePrimitiveExpression(primaryKeyComponentNumber),
                                new CodePrimitiveExpression(classInfo.GetPrimaryKeyFields().Length))));
                        }
                    }
                    primaryKeyComponentNumber++;
                    continue;
                }

                if (options.NullPropagation && (fi.References != null || fi.IsNullable) && actualNullableRepresentation != PrimitiveRepresentation.Raw)
                {
                    CodeExpression retVal = new CodePrimitiveExpression(null);

                    if (fi.References == null && actualNullableRepresentation == PrimitiveRepresentation.SqlType)
                    {
                        retVal = new CodePropertyReferenceExpression(
                            new CodeTypeReferenceExpression(fi.GetNullableFieldHandler().GetSqlType()), "Null");
                    }

                    prop.GetStatements.Add(
                        new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                        new CodeThisReferenceExpression(),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(null)),
                        new CodeStatement[]
                            {
                                new CodeMethodReturnStatement(retVal)
                            },
                        new CodeStatement[]
                            {
                            }));
                }

                if (fi.References != null)
                {
                    // reference field getter
                    //
                    CodeExpression pk = new CodeVariableReferenceExpression("pk");
                    Type pkType;
                    CodeExpression isFieldNotNull;
                    CodeExpression getRef;
                    if (fi.ParentClass.GetDataSource().EnableDynamicFields)
                    {
                        pkType = typeof(object);
                        isFieldNotNull = new CodeBinaryOperatorExpression(
                            pk,
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePrimitiveExpression(null));
                        getRef = new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(typeof(SoodaObject)),
                            "GetRefHelper",
                            GetTransaction(),
                            Factory(fi.References),
                            pk);
                    }
                    else
                    {
                        pkType = fi.GetNullableFieldHandler().GetSqlType();
                        isFieldNotNull = new CodeBinaryOperatorExpression(
                            new CodePropertyReferenceExpression(pk, "IsNull"),
                            CodeBinaryOperatorType.ValueEquality,
                            new CodePrimitiveExpression(false));
                        getRef = new CodeMethodInvokeExpression(
                            LoaderClass(fi.ReferencedClass),
                            "GetRef",
                            GetTransaction(),
                            new CodePropertyReferenceExpression(pk, "Value"));
                    }
                    prop.GetStatements.Add(
                            new CodeConditionStatement(
                                new CodeBinaryOperatorExpression(
                                    RefCacheExpression(ci, fi),
                                    CodeBinaryOperatorType.IdentityEquality,
                                    new CodePrimitiveExpression(null)),
                                new CodeStatement[]
                                {
                                new CodeVariableDeclarationStatement(pkType, "pk", GetFieldValueForRead(fi)),
                                new CodeConditionStatement(
                                    isFieldNotNull,
                                    new CodeStatement[]
                                    {
                                    new CodeAssignStatement(
                                        RefCacheExpression(ci, fi),
                                        getRef)
                                    })
                                }
                    ));


                    prop.GetStatements.Add(
                            new CodeMethodReturnStatement(
                                new CodeCastExpression(returnType,
                                    RefCacheExpression(ci, fi))));

                    // reference field setter
                    if (!classInfo.ReadOnly && !fi.ReadOnly)
                    {
                        prop.SetStatements.Add(
                                new CodeExpressionStatement(

                                    new CodeMethodInvokeExpression(
                                        new CodeTypeReferenceExpression(typeof(Sooda.ObjectMapper.SoodaObjectImpl)), "SetRefFieldValue",

                                        // parameters
                                        new CodeThisReferenceExpression(),
                                        new CodePrimitiveExpression(fi.Table.OrdinalInClass),
                                        new CodePrimitiveExpression(fi.Name),
                                        new CodePrimitiveExpression(fi.ClassUnifiedOrdinal),
                                        new CodeCastExpression(typeof(SoodaObject), new CodePropertySetValueReferenceExpression()),
                                        RefCacheArray(),
                                        new CodePrimitiveExpression(GetFieldRefCacheIndex(ci, fi)),
                                        Factory(returnType.BaseType)
                                        )));
                    }
                }
                else
                {
                    // plain field getter

                    CodeExpression fieldValue = GetFieldValueForRead(fi);
                    if (fi.ParentClass.GetDataSource().EnableDynamicFields)
                    {
                        switch (fi.IsNullable ? actualNullableRepresentation : actualNotNullRepresentation)
                        {
                            case PrimitiveRepresentation.Boxed:
                                break;

                            case PrimitiveRepresentation.SqlType:
                            case PrimitiveRepresentation.RawWithIsNull:
                            case PrimitiveRepresentation.Raw:
                                fieldValue = new CodeCastExpression(new CodeTypeReference(fi.GetNullableFieldHandler().GetFieldType()), fieldValue);
                                break;

                            case PrimitiveRepresentation.Nullable:
                                fieldValue = new CodeCastExpression(new CodeTypeReference(fi.GetNullableFieldHandler().GetNullableType()), fieldValue);
                                break;

                            default:
                                throw new NotImplementedException("Unknown PrimitiveRepresentation");
                        }
                    }
                    prop.GetStatements.Add(new CodeMethodReturnStatement(fieldValue));

                    if (!classInfo.ReadOnly && !fi.ReadOnly)
                    {
                        // plain field setter

                        CodeExpression beforeDelegate = new CodePrimitiveExpression(null);
                        CodeExpression afterDelegate = new CodePrimitiveExpression(null);

                        if (classInfo.Triggers)
                        {
                            beforeDelegate = new CodeDelegateCreateExpression(new CodeTypeReference(typeof(SoodaFieldUpdateDelegate)),
                                    new CodeThisReferenceExpression(), "BeforeFieldUpdate_" + fi.Name);
                            afterDelegate = new CodeDelegateCreateExpression(new CodeTypeReference(typeof(SoodaFieldUpdateDelegate)),
                                    new CodeThisReferenceExpression(), "AfterFieldUpdate_" + fi.Name);
                        }

                        prop.SetStatements.Add(
                                new CodeExpressionStatement(
                                    new CodeMethodInvokeExpression(
                                        new CodeTypeReferenceExpression(typeof(Sooda.ObjectMapper.SoodaObjectImpl)), "SetPlainFieldValue",

                                        // parameters
                                        new CodeThisReferenceExpression(),
                                        new CodePrimitiveExpression(fi.Table.OrdinalInClass),
                                        new CodePrimitiveExpression(fi.Name),
                                        new CodePrimitiveExpression(fi.ClassUnifiedOrdinal),
                                        Box(new CodePropertySetValueReferenceExpression()),
                                        beforeDelegate,
                                        afterDelegate
                                        )));
                    }
                }
            }


            if (classInfo.Collections1toN != null)
            {
                foreach (CollectionOnetoManyInfo coli in classInfo.Collections1toN)
                {
                    prop = new CodeMemberProperty();
                    prop.Name = coli.Name;
                    prop.Attributes = MemberAttributes.Final | MemberAttributes.Public;
                    prop.Type = GetCollectionPropertyType(coli.ClassName);

                    prop.GetStatements.Add(
                        new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                        new CodeFieldReferenceExpression(This, "_collectionCache_" + coli.Name),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(null)), new CodeStatement[]
                            {
                                new CodeAssignStatement(
                                new CodeFieldReferenceExpression(This, "_collectionCache_" + coli.Name),
                                new CodeObjectCreateExpression(GetCollectionWrapperType(coli.ClassName),
                                new CodeObjectCreateExpression(new CodeTypeReference(typeof(Sooda.ObjectMapper.SoodaObjectOneToManyCollection)),
                                new CodeExpression[] {
                                    new CodeMethodInvokeExpression(This, "GetTransaction"),
                                    new CodeTypeOfExpression(new CodeTypeReference(coli.ClassName)),
                                    new CodeThisReferenceExpression(),
                                    new CodePrimitiveExpression(coli.ForeignFieldName),
                                    new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(coli.ClassName + "_Factory"), "TheClassInfo"),
                                    new CodeFieldReferenceExpression(null, "_collectionWhere_" + coli.Name),
                                    new CodePrimitiveExpression(coli.Cache)
                            }))),
                    }, new CodeStatement[] { }));

                    prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(This, "_collectionCache_" + coli.Name)));
                    ctd.Members.Add(prop);

#if DOTNET35
                    CodeExpression whereExpression = new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(Sooda.QL.Soql)),
                        "FieldEquals",
                        new CodePrimitiveExpression(coli.ForeignFieldName),
                        This);
                    if (!string.IsNullOrEmpty(coli.Where))
                    {
                        whereExpression = new CodeObjectCreateExpression(
                            typeof(Sooda.QL.SoqlBooleanAndExpression),
                            whereExpression,
                            new CodePropertyReferenceExpression(new CodeFieldReferenceExpression(null, "_collectionWhere_" + coli.Name), "WhereExpression"));
                    }
                    prop = GetCollectionLinqQuery(coli, whereExpression);
                    ctd.Members.Add(prop);
#endif
                }
            }

            if (classInfo.CollectionsNtoN != null)
            {
                foreach (CollectionManyToManyInfo coli in classInfo.CollectionsNtoN)
                {
                    RelationInfo relationInfo = coli.GetRelationInfo();
                    // FieldInfo masterField = relationInfo.Table.Fields[1 - coli.MasterField];

                    string relationTargetClass = relationInfo.Table.Fields[coli.MasterField].References;

                    prop = new CodeMemberProperty();
                    prop.Name = coli.Name;
                    prop.Attributes = MemberAttributes.Final | MemberAttributes.Public;
                    prop.Type = GetCollectionPropertyType(relationTargetClass);

                    prop.GetStatements.Add(
                        new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                        new CodeFieldReferenceExpression(This, "_collectionCache_" + coli.Name),
                        CodeBinaryOperatorType.IdentityEquality,
                        new CodePrimitiveExpression(null)), new CodeStatement[] {
                                                                                    new CodeAssignStatement(
                                                                                    new CodeFieldReferenceExpression(This, "_collectionCache_" + coli.Name),
                                                                                    new CodeObjectCreateExpression(GetCollectionWrapperType(relationTargetClass),
                                                                                    new CodeObjectCreateExpression(new CodeTypeReference(typeof(Sooda.ObjectMapper.SoodaObjectManyToManyCollection)),
                                                                                    new CodeExpression[] {
                                                                                                             new CodeMethodInvokeExpression(This, "GetTransaction"),
                                                                                                             new CodePrimitiveExpression(coli.MasterField),
                                                                                                             new CodeMethodInvokeExpression(This, "GetPrimaryKeyValue"),
                                                                                                             new CodeTypeOfExpression(relationInfo.Name + "_RelationTable"),
                                                                                                             new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(relationInfo.Name + "_RelationTable"), "theRelationInfo")

                                                                                                         }))
                                                                                    ),
                    }
                        , new CodeStatement[] { }));

                    prop.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(This, "_collectionCache_" + coli.Name)));

                    ctd.Members.Add(prop);

#if DOTNET35
                    CodeExpression whereExpression = new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(Sooda.QL.Soql)),
                        "CollectionFor",
                        new CodeMethodInvokeExpression(
                                new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(classInfo.Name + "_Factory"), "TheClassInfo"),
                                "FindCollectionManyToMany",
                                new CodePrimitiveExpression(coli.Name)),
                        This);
                    prop = GetCollectionLinqQuery(coli, whereExpression);
                    ctd.Members.Add(prop);
#endif
                }
            }
        }

        CodeMemberField GetCollectionCache(CollectionBaseInfo coli)
        {
            CodeMemberField field = new CodeMemberField(GetCollectionPropertyType(coli.GetItemClass().Name), "_collectionCache_" + coli.Name);
            field.Attributes = MemberAttributes.Private;
            field.InitExpression = new CodePrimitiveExpression(null);
            return field;
        }

        public void GenerateFields(CodeTypeDeclaration ctd, ClassInfo ci)
        {
            if (GetFieldRefCacheCount(ci) > 0)
            {
                CodeMemberField field = new CodeMemberField(new CodeTypeReference(new CodeTypeReference("SoodaObject"), 1), "_refcache");
                field.Attributes = MemberAttributes.Private;
                field.InitExpression = new CodeArrayCreateExpression(
                    new CodeTypeReference(typeof(SoodaObject)), new CodePrimitiveExpression(GetFieldRefCacheCount(ci)));
                ctd.Members.Add(field);
            }

            if (classInfo.Collections1toN != null)
            {
                foreach (CollectionOnetoManyInfo coli in classInfo.Collections1toN)
                {
                    ctd.Members.Add(GetCollectionCache(coli));
                }
                foreach (CollectionOnetoManyInfo coli in classInfo.Collections1toN)
                {
                    CodeMemberField field = new CodeMemberField("Sooda.SoodaWhereClause", "_collectionWhere_" + coli.Name);
                    field.Attributes = MemberAttributes.Static | MemberAttributes.Private;
                    if (!string.IsNullOrEmpty(coli.Where))
                    {
                        field.InitExpression = new CodeObjectCreateExpression(
                            "Sooda.SoodaWhereClause",
                            new CodePrimitiveExpression(coli.Where));
                    }
                    else
                    {
                        field.InitExpression = new CodePrimitiveExpression(null);
                    }
                    ctd.Members.Add(field);
                }
            }

            if (classInfo.CollectionsNtoN != null)
            {
                foreach (CollectionManyToManyInfo coli in classInfo.CollectionsNtoN)
                {
                    ctd.Members.Add(GetCollectionCache(coli));
                }
            }
        }
    }
}
