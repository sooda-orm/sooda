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

using Sooda.ObjectMapper;
using Sooda.ObjectMapper.FieldHandlers;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Sooda.Schema
{

    [XmlTypeAttribute(Namespace = "http://www.sooda.org/schemas/SoodaSchema.xsd")]
    [Serializable]
    public class FieldInfo : ICloneable
    {
        [XmlAttribute("name")]
        public string Name;

        private string dbcolumn;

        [XmlAttribute("type")]
        public FieldDataType DataType;

        [XmlElement("description")]
        public string Description;

        [XmlAttribute("size")]
        [System.ComponentModel.DefaultValueAttribute(-1)]
        public int Size = -1;

        [XmlAttribute("precision")]
        [System.ComponentModel.DefaultValueAttribute(-1)]
        public int Precision = -1;

        [XmlAttribute("references")]
        public string References;

        [XmlAttribute("precommitValue")]
        public string PrecommitValue;

        [XmlIgnore]
        [NonSerialized]
        public object PrecommitTypedValue;

        [XmlAttribute("primaryKey")]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsPrimaryKey = false;

        [XmlAttribute("nullable")]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IsNullable = false;

        [XmlAttribute("readOnly")]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ReadOnly = false;

        [XmlAttribute("forceTrigger")]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool ForceTrigger = false;

        [XmlAttribute("onDelete")]
        [System.ComponentModel.DefaultValueAttribute(DeleteAction.Nothing)]
        public DeleteAction DeleteAction = DeleteAction.Nothing;

        [XmlAnyAttribute()]
        [NonSerialized]
        public System.Xml.XmlAttribute[] Extensions;

        [XmlAttribute("label")]
        [DefaultValue(false)]
        public bool IsLabel = false;

        [XmlAttribute("prefetch")]
        [DefaultValue(0)]
        public int PrefetchLevel = 0;

        [XmlAttribute("find")]
        [DefaultValue(false)]
        public bool FindMethod = false;

        [XmlAttribute("findList")]
        [DefaultValue(false)]
        public bool FindListMethod = false;

        [XmlAttribute("dbcolumn")]
        public string DBColumnName
        {
            get
            {
                return dbcolumn ?? Name;
            }
            set
            {
                dbcolumn = value;
            }
        }

        public FieldInfo Clone()
        {
            return DoClone();
        }

        object ICloneable.Clone()
        {
            return DoClone();
        }

        [XmlIgnore()]
        [NonSerialized]
        public int OrdinalInTable;

        [XmlIgnore()]
        [NonSerialized]
        public int ClassLocalOrdinal;

        [XmlIgnore()]
        [NonSerialized]
        public int ClassUnifiedOrdinal;

        [XmlIgnore]
        [NonSerialized]
        public TableInfo Table;

        [XmlIgnore]
        [NonSerialized]
        public ClassInfo ReferencedClass;

        [XmlIgnore]
        [NonSerialized]
        public ClassInfo ParentClass;

        [XmlIgnore]
        [NonSerialized]
        internal string NameTag;

        [XmlIgnore]
        [NonSerialized]
        public RelationInfo ParentRelation;

        public FieldInfo DoClone()
        {
            FieldInfo fi = new FieldInfo();

            fi.Name = this.Name;
            fi.dbcolumn = this.dbcolumn;
            fi.DataType = this.DataType;
            fi.Description = this.Description;
            fi.Size = this.Size;
            fi.Precision = this.Precision;
            fi.References = this.References;
            fi.PrecommitValue = this.PrecommitValue;
            fi.IsPrimaryKey = this.IsPrimaryKey;
            fi.IsNullable = this.IsNullable;
            fi.ReadOnly = this.ReadOnly;
            fi.ForceTrigger = this.ForceTrigger;
            fi.DeleteAction = this.DeleteAction;
            fi.IsLabel = this.IsLabel;
            fi.PrefetchLevel = this.PrefetchLevel;
            if (this.Extensions != null)
                fi.Extensions = (System.Xml.XmlAttribute[])this.Extensions.Clone();

            return fi;
        }

        public StringCollection GetBackRefCollections(SchemaInfo schema)
        {
            return schema.GetBackRefCollections(this);
        }

        public SoodaFieldHandler GetNullableFieldHandler()
        {
            return FieldHandlerFactory.GetFieldHandler(DataType);
        }

        public SoodaFieldHandler GetFieldHandler()
        {
            return FieldHandlerFactory.GetFieldHandler(DataType, IsNullable);
        }

        internal void Resolve(TableInfo parentTable, string parentName, int ordinal)
        {
            this.Table = parentTable;
            this.OrdinalInTable = ordinal;
            this.NameTag = parentTable.NameToken + "/" + ordinal;
        }

        internal void ResolvePrecommitValues()
        {
        }

        public override string ToString()
        {
            return String.Format("{0}.{1} ({2} ref {3})", ParentClass != null ? "class " + ParentClass.Name : "???", Name, DataType, References);
        }

        internal void Merge(FieldInfo merge)
        {
            this.DataType = merge.DataType;
            if (merge.Description != null)
                this.Description = (this.Description != null ? this.Description + "\n" : "") + merge.Description;
            if (merge.Size != -1)
                this.Size = merge.Size;
            if (merge.Precision != -1)
                this.Precision = merge.Size;
            if (merge.References != null)
                this.References = merge.References;
            if (merge.PrecommitValue != null)
                this.PrecommitValue = merge.PrecommitValue;
            this.IsPrimaryKey = merge.IsPrimaryKey;
            this.IsNullable = merge.IsNullable;
            this.ReadOnly = merge.ReadOnly;
            this.ForceTrigger = merge.ForceTrigger;
            this.DeleteAction = merge.DeleteAction;
            this.IsLabel = merge.IsLabel;
            this.PrefetchLevel = merge.PrefetchLevel;
            this.FindMethod = merge.FindMethod;
            this.FindListMethod = merge.FindListMethod;
            if (merge.dbcolumn != null)
                this.DBColumnName = merge.dbcolumn;
        }

        internal void ResolveReferences(SchemaInfo schema)
        {
            if (References == null)
                return;
            ClassInfo ci = schema.FindClassByName(References);
            if (ci == null)
                throw new SoodaSchemaException("Class (or interface) " + References + " not found.");
            DataType = ci.GetFirstPrimaryKeyField().DataType;
            ReferencedClass = ci;
        }

        [XmlIgnore]
        public string TypeName
        {
            get
            {
                return References ?? DataType.ToString();
            }
            set
            {
#if DOTNET4
                if (Enum.TryParse(value, out DataType))
                {
                    References = null;
                    return;
                }
#else
                try
                {
                    DataType = (FieldDataType) Enum.Parse(typeof(FieldDataType), value);
                    References = null;
                    return;
                }
                catch (ArgumentException)
                {
                }
#endif
                References = value;
            }
        }

        [XmlIgnore]
        public Type Type
        {
            get
            {
                if (References != null)
                    return Type.GetType(ParentClass.Schema.Namespace + "." + References); // FIXME: included schema
                SoodaFieldHandler handler = GetFieldHandler();
                return IsNullable ? handler.GetNullableType() : handler.GetFieldType();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.IsSubclassOf(typeof(SoodaObject)))
                {
                    References = value.Name;
                }
                else
                {
                    DataType = FieldHandlerFactory.GetFieldDataType(value, out IsNullable);
                    References = null;
                }
            }
        }

        [XmlIgnore]
        public bool IsDynamic
        {
            get
            {
                return Table.IsDynamic;
            }
        }

    }
}
