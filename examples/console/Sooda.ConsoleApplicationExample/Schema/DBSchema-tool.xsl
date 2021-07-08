<?xml version="1.0" encoding="windows-1250" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:sooda="http://www.sooda.org/schemas/SoodaSchema.xsd">

    <xsl:output method="html" doctype-public="-//W3C//DTD HTML 4.01//EN" />
    <xsl:template match="/">
        <html>
            <head>
                <style>
                    body {
                        font-family: Verdana;
                        font-size: 10px;
                    }

		    h3 { 
			font-family: Verdana;
			font-size: 13px;
			font-weight: bolder;
			margin: 15px 2px 2px 3px;
		   }

		    h3.relation { 
			font-family: Verdana;
			font-size: 13px;
			font-weight: bolder;
			margin: 15px 2px 2px 3px;
			font-style: italic;
		   }

		   a
		   {
			color: #3300FF;
			text-decoration: none;
		   }

		   a:hover
		   {
			color: #FF9900;
			text-decoration: underline;
		   }

		   a:visited
		   {
			color: #3300FF;
			text-decoration: none;
		   }

                    td {
                        font-family: Verdana;
                        font-size: 10px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                        padding-left: 5px;
                        padding-right: 15px;  
                        margin: 0px 0px 0px 0px; 
                        border: solid 1px #c0c0c0;
                    }

		    td.fieldmodificator { 
			font-family: Verdana;
			font-size: 11px;
			padding: 3px 2px 3px 2px; 
			margin: 0px 0px 0px 0px;
			border: solid 1px #c0c0c0;
			text-align: center;
		    }

		    td.inheritedclass
		    {
			font-family: Verdana;
			font-size: 11px;
			font-weight: bold;
			text-align: center;
			vertical-align: center;
			height: 3px;
		    }

		    .headerfieldmodificator {
                        background-color: #408040;
                        font-weight: bold;
                        color: white;
                        font-family: Verdana;
                        font-size: 11px;
			padding: 3px 2px 5px 3px; 
			margin: 0px 0px 0px 0px;
			text-align: center;
                      }


                    .dane {
                        border: solid 1px rgb(13,43,136);
                        border-collapse: collapse;
                        page-break-after: always;
                    }

		    div.version
		    {
			font-size: 9px; 
			font-weight: normal;
			font-family: Tahoma;
			left: 0px;
			top: 0px;
			position: absolute;
			border-width: 0px 1px 1px 0px;
			border-style: none solid solid none;
			border-color: #C0C0C0;
			padding: 1px 2px 1px 2px;
		    }

		    #clipboard
		    {
			font-size: 5px; 
			font-weight: normal;
			font-family: Tahoma;
			right: 5px;
			top: 5px;
            background-color: white;

			position: absolute;
			border: 3px double #000;
			padding: 1px 2px 1px 2px;
		    }

		    .error { font-weight: bold; color: #f00; }
		    .warning { font-weight: bold; color: #f90; }
                        
                    .header {
                        background-color: #408040;
                        font-weight: bold;
                        color: white;
                        font-family: Verdana;
                        font-size: 11px;
                        padding-top: 5px;
                        padding-bottom: 5px;
                        padding-left: 5px;
                        padding-right: 15px;
                        margin: 0px, 0px, 0px, 0px;
                            }
                    
                    .const { background-color: #cfc; color: #000; }

                        span.missing
                        {
                        color: red;
                        }

                        #output
                        {
                        width: 960px;
                        }
                       
                       .description { margin-top: 0px; margin-bottom: 10px; }
                       .title { color: #808080; font-size: 13px; font-family: Verdana; font-weight: bold; padding-top: 1px; padding-bottom: 1px;  padding-left: 2px; padding-right: 2px; border: none; vertical-align: top; }
                       .name { color: #000000; font-size: 13px; font-family: Verdana; font-weight: bold;  padding-top: 1px; padding-bottom: 1px; padding-left: 2px; padding-right: 2px; border: none; }
                       .value { color: #000000; font-size: 13px; font-family: Verdana; font-weight: normal;  padding-top: 1px; padding-bottom: 1px;  padding-left: 2px; padding-right: 2px; border: none; }

                       .inherit { font-weight: normal; }

                       .std { color:  #999999; }

			#nav-menu { width: 220px;  bottom: 10px; overflow-y: auto; overflow-x: none; scrollbar-base-color:white; padding: 0px 0px 0px 0px;}
			#nav-menu ul { list-style: none; padding: 0; margin: 0; width: 165px; }
			#nav-menu li		{ background-color: #fff; color: #30f; text-decoration: none; width: 190px; padding: 1px 5px 1px 5px; border: 1px solid #000; }
			#nav-menu a		{text-decoration: none; width: 165px;}
			#nav-menu li:hover	{ background-color: #eee; color: #f90; text-decoration: none; width: 190px; padding: 1px 5px 1px 5px; border: 1px solid #000; border-top-style: none;}
			#nav-menu a:hover	{ text-decoration: none; width: 165px; }



			/**************** menu coding *****************/
			#menubox { position: fixed; left: 0px; top: 0px; width: 960px; }
			#menu { float: right; }
			#menu ul { list-style: none; margin: 0; padding: 0; }
			#menu a { display: block; background-color: #fff; color: #30f; text-decoration: none; padding: 2px 5px 3px 5px; border: 1px solid #000; cursor: pointer, hand;  }

			#menu a:hover {	background-color: #eee; color: #f90; text-decoration: none; }
			#menu li {	position: relative; float: left; cursor: pointer, hand; }

			#menu li.item { position: relative; float: left; width: 190px; }

			#menu ul ul { position: absolute;  top: 18px; left: 0px; width: 100%; height: 100%; z-index: 1; }

			div#menu ul ul,
			div#menu ul li:hover ul ul {display: none;}

			div#menu ul li:hover ul,
			div#menu ul ul li:hover ul {display: block;}

		</style>
                </head>
        <script type="text/javascript">
var classes = {};
<xsl:apply-templates select="//sooda:class|//sooda:relation" mode="json" />


function initializeClasses()
{
    for (var clsIt in classes)
    {
        var cls = classes[clsIt];
        cls.primaryKeys = [];
        cls.foreignKeys = [];
        cls.nullableColumns = [];
        cls.requiredColumns = [];
        for (var columnIt in cls.columns)
        {
            var column = cls.columns[columnIt];
            column.primaryKey = (column.primaryKey === "true"); // strict equal
            column.nullable = !column.primaryKey &#38;&#38; (column.nullable == "true");
            if (column.dbcolumn == undefined)
                column.dbcolumn = column.name;

            if (column.primaryKey)
                cls.primaryKeys.push(column.name);
            if (column.references != undefined)
                cls.foreignKeys.push(column.name);

            if (column.nullable)
                cls.nullableColumns.push(column.name);
            else
                cls.requiredColumns.push(column.name);
        }
    }
}

initializeClasses();

//<![CDATA[

// formatowanie tekstu w postaci: ala ma {0} kota {1}
function format(str, arg1 /*, ... */)
{
    return __formatV(str, arguments, 1);
}

function __formatV(str, args, offset)
{
    if (str == undefined || str == null || str == '')
        return '';

    var r = /{\d+}/g;

    return str.replace(r, function(found) {
          var argId = parseInt(found.substr(1, found.length - 1));
          return args[argId+offset];        
    });
}

function formatLine(str, arg1 /*, ... */)
{
    return __formatV(str, arguments, 1) + '\r\n';
}


var USER = "atmouser";


function TSQL(globalSettings)
{
    var settings = globalSettings ||
    {
        grantUser: null,    // nazwa uzytkownika dla ktorego sa generowane uprawnienia, null - nie sa generowane
        nullReferences: true,   // gdy true, referencje są zawsze generowane jako nullable
        maxVarCharSize: 4000, // gdy wieksze od 4000 generuje nvarchar(max)
    };
    
    // Tworzenie tabeli
    // @returns - changelog do utworzenia całej tabeli
    function _Table(className)
    {
        var cls = classes[className];
        
        if (!!cls.inheritFrom && classes[cls.inheritFrom].table.name == cls.table.name)
        {
            return this.InheritTable(className);
        }

        var ret = formatLine("--- {0}", cls.name);
        ret += formatLine("create table {0}", cls.table.name);
        ret += formatLine("(");

        // --- tabela ---
        var lastColumn = cls.columnNames.length - 1;
        for (var i = 0; i <= lastColumn; ++i)
        {
            var semi = (i == lastColumn) ? "" : ",";
            var columnName = cls.columnNames[i];
            var column = cls.columns[columnName];

            ret += formatLine("    {0}{1}", this.Column(className, columnName), semi);
        }
        ret += formatLine(");");
        ret += this.ApplyDDL();
        ret += formatLine();

        // --- uprawnienia do tabeli ---
        var grant = this.Grant(className);
        if (grant != '')
        {
            ret += formatLine("{0}", grant);
            ret += formatLine();
        }


        // --- primary keys ---
        if (cls.primaryKeys.length > 0)
        {
            ret += formatLine("{0}", this.PrimaryKey(className));
            ret += formatLine();
        }

        // --- foreign keys ---
        for (var i = 0; i < cls.foreignKeys.length; ++i)
        {
            ret += formatLine("{0}", this.ForeignKey(className, cls.foreignKeys[i]));
        }
        if (cls.foreignKeys.length > 0)
            ret += formatLine();

        // --- indeksy ---
        for (var i = 0; i < cls.foreignKeys.length; ++i)
        {
            ret += formatLine("{0}", this.Index(className, cls.foreignKeys[i]));
        }
        if (cls.foreignKeys.length > 0)
            ret += formatLine();

        ret += this.ApplyDDL();
        ret += formatLine();

        // --- consts ---
        if (cls.constNames.length > 0)
        {
            ret += formatLine("{0}", this.AllConsts(className));
            ret += formatLine();
        }


        ret += formatLine();
        ret += formatLine();

        return ret;
    }
    
    function _InheritTable(className)
    {
        var cls = classes[className];
        var from = classes[cls.inheritFrom];
        
        var ret = formatLine("--- {0} : {1} ---", cls.name, from.name);
        ret += formatLine();
         
        // --- tabela odziedziczona z innej - dodajemy nowe kolumny ---
        var lastColumn = cls.columnNames.length - 1;
        for (var i = 0; i <= lastColumn; ++i)
        {
            var semi = (i == lastColumn) ? "" : ",";
            var columnName = cls.columnNames[i];
            var column = cls.columns[columnName];
            
            if (from.columns[columnName] != null)
                continue;
            
            ret += formatLine("alter table {0} add {1};", cls.table.name, this.Column(className, columnName));
        }
        ret += this.ApplyDDL();
        ret += formatLine();

        // --- foreign keys ---
        for (var i = 0; i < cls.foreignKeys.length; ++i)
        {
            ret += formatLine("{0}", this.ForeignKey(className, cls.foreignKeys[i]));
        }
        if (cls.foreignKeys.length > 0)
            ret += formatLine();

        // --- indeksy ---
        for (var i = 0; i < cls.foreignKeys.length; ++i)
        {
            ret += formatLine("{0}", this.Index(className, cls.foreignKeys[i]));
        }
        if (cls.foreignKeys.length > 0)
            ret += formatLine();

        ret += this.ApplyDDL();
        ret += formatLine();

        // --- consts ---
        if (cls.constNames.length > 0)
        {
            ret += formatLine("{0}", this.AllConsts(className));
            ret += formatLine();
        }

        return ret;
    }

    function _ApplyDDL()
    {
        return formatLine("go");
    }

    function _ApplyDML()
    {
        return "";
    }

    function _Grant(className)
    {
        var cls = classes[className];
        return !settings.grantUser ? '' : format("grant delete, insert, select, update on {0} to {1};", cls.table.name, settings.grantUser);
    }

    // column: { name, soodatype, nullable }
    function _Column(className, columnName)
    {
        var cls = classes[className];
        var column = cls.columns[columnName];

        var name = column.dbcolumn;

        // type(lowercase) -> function
        var typeMapping = 
        {
            "integer": function(c) { return "int"; },
            "string" : function(c) { return format("nvarchar({0})", c.size > settings.maxVarCharSize ? 'max' : c.size); },
            "ansistring" : function(c) { return format("varchar({0})", c.size > settings.maxVarCharSize ? 'max' : c.size); },
            "booleanasinteger": function(c) { return "int"; },
            "timespan": function(c) { return "int"; },
            "datetime": function(c) { return "DateTime"; },
            "decimal": function(c) { return c.size ? format(c.precision ? "decimal({0}, {1})" : "decimal({0})", c.size, c.precision) : "decimal"; },
            "guid": function(c) { return "uniqueidentifier"; }
        };

        var type = typeMapping[column.type.toLowerCase()](column);

        var isnull = (!column.nullable && !(settings.nullReferences && !!column.references) && !cls.inheritFrom) ? "not null" : "null";

        return format("{0} {1} {2}", name, type, isnull);
    }

    function _AllConsts(className)
    {
        var cls = classes[className];

        var ret = "";
        for (var i = 0; i < cls.constNames.length; ++i)
        {
            ret += formatLine("{0}", this.Const(className, cls.constNames[i], false));
        }
        ret += this.ApplyDML();
        return ret;
    }

    // dla const'ów generowane są wszystkie kolumny,
    // nullable na końcu, (z wartością null, wymagane - ze znakami zapytania do uzupełnienia
    function _Const(className, constName, apply)
    {
        var cls = classes[className];
        var cons = cls.consts[constName];


        var keyColumn = cls.columns[cls.primaryKeys[0]];
        var labelCls = cls; 
        var labelColumn = cls.columns[cls.label];
        while (labelColumn == undefined && labelCls.inheritFrom != undefined)
        {
            labelCls = classes[labelCls.inheritFrom]; 
            labelColumn = labelCls.columns[cls.label];
        }

        // key, label, req, nullable
        var columns = format("{0}, {1}", keyColumn.dbcolumn, labelColumn.dbcolumn);
        var values = format("{0}, '{1}'", cons.key, cons.name);


        for (var it = 0; it < cls.columnNames.length; ++it)
        {
            var column = cls.columns[cls.columnNames[it]];
            if (column == keyColumn || column == labelColumn)
                continue;

            columns += format(", {0}", column.dbcolumn);
            values += format(", {0}", column.nullable ? "null" : column.name);
        }

        var ret = format("insert into {0}({1}) values({2});", cls.table.name, columns, values);
        if (apply === true)
        {
            ret = formatLine("{0}", ret);
            ret += this.ApplyDML();
        }
        return ret;
    }

    function _PrimaryKey(className)
    {
        var cls = classes[className];

        var pks = "";
        for (var i = 0; i < cls.primaryKeys.length; ++i)
        {
            if (i != 0)
                pks += ", ";
            var pk = cls.columns[cls.primaryKeys[i]];
            pks += pk.dbcolumn;
        }

        var pk_name = this.ShortName("PK_{0}", cls.table.name);
        return format("alter table {0} add constraint {1} primary key ({2});", cls.table.name, pk_name, pks);
    }

    function _ForeignKey(className, columnName)
    {
        var cls = classes[className];
        var column = cls.columns[columnName];

        var refCls = classes[column.references];
        var refColumn = refCls.columns[refCls.primaryKeys[0]];

        // alter table RCPEntryType add constraint FK_RCPEntryType_object_class foreign key (object_class) references ObjectClass(id);
        var fk_name = this.ShortName("FK_{0}_{1}", cls.table.name, column.dbcolumn);
        return format("alter table {0} add constraint {1} foreign key ({2}) references {3}({4});",
            cls.table.name, fk_name, column.dbcolumn, refCls.table.name, refColumn.dbcolumn);
    }

    function _Index(className, columnName)
    {
        var cls = classes[className];
        var column = cls.columns[columnName];

        var idx_name = this.ShortName("IDX_{0}_{1}", cls.table.name, column.dbcolumn);
        return format("create index {0} on {1}({2});", idx_name, cls.table.name, column.dbcolumn);
    }

    function _AddColumn(className, columnName)
    {
        var cls = classes[className];
        var column = cls.columns[columnName];

        var isNullable = column.nullable;
        column.nullable = true;        // by this.Column wygenerowało typ "null"
        
        var ret = formatLine("alter table {0} add {1};", cls.table.name, this.Column(className, columnName));
        if (isNullable == false)       // kolumna nie jest nullowalna: uzupełniamy jej zawartość i zmieniamy jej typ na nienullowalną
        {
            ret += this.ApplyDML();
            ret += formatLine("update {0} set {1} = xx;", cls.table.name, column.dbcolumn);
            ret += this.ApplyDDL();
            column.nullable = isNullable;
            ret += formatLine("alter table {0} alter column {1};", cls.table.name, this.Column(className, columnName));
        }

        if (column.references != undefined)
        {
            ret += formatLine("{0}", this.ForeignKey(className, columnName));
            ret += formatLine("{0}", this.Index(className, columnName));
        }

        ret += this.ApplyDML();
        ret += formatLine();

        return ret;
    }

    function _AlterColumn(className, columnName)
    {
        var cls = classes[className];
        var column = cls.columns[columnName];

        var ret = formatLine("alter table {0} alter column {1};", cls.table.name, this.Column(className, columnName));
        ret += this.ApplyDDL();
        ret += formatLine();
        return ret;
    }

    // zwraca skróconą nazwę w danym formacie
    function _ShortName(_format, args/*, ... */)
    {
        return __formatV(_format, arguments, 1);
    }


    this.Table = _Table;
    this.InheritTable = _InheritTable;
    this.Grant = _Grant;
    this.ApplyDDL = _ApplyDDL;
    this.ApplyDML = _ApplyDML;
    this.Column = _Column;
    this.PrimaryKey = _PrimaryKey;
    this.Index = _Index;
    this.ForeignKey = _ForeignKey;
    this.AddColumn = _AddColumn;
    this.AlterColumn = _AlterColumn;
    this.Const = _Const;
    this.AllConsts = _AllConsts;
    this.ShortName = _ShortName;
}

Oracle.prototype = new TSQL();
function Oracle()
{
    // column: { name, soodatype, nullable }
    function _Column(className, columnName)
    {
        var cls = classes[className];
        var column = cls.columns[columnName];

        var name = column.dbcolumn;

        // type(lowercase) -> function
        var typeMapping = 
        {
            "integer": function(c) { return "int"; },
            "string" : function(c) { return format("nvarchar2({0})", c.size); },
            "booleanasinteger": function(c) { return "int"; },
            "datetime": function(c) { return "date"; },
            "decimal": function(c) { return c.size ? format(c.precision ? "decimal({0}, {1})" : "decimal({0})", c.size, c.precision) : "decimal"; }
        };

        var type = typeMapping[column.type.toLowerCase()](column);

        var isnull = (!column.nullable) ? "not null" : "null";

        return format("{0} {1} {2}", name, type, isnull);
    }

    function _Grant(className)
    {
        return '';
    }

    function _ApplyDDL()
    {
        return "";
    }

    function _ApplyDML()
    {
        return formatLine("commit;");
    }

    function _ShortName(_format, args/*, ... */)
    {
        var maxLength = 30;
        var empty = __formatV(_format+'', ['','','','','','','','',''], 0);
        var left = maxLength - empty.length; // na tyle znakow jest miejsce do uzupelnienia

        var total = 0;                       // tyle znakow powinno byc uzupelnionych (bez skrotow)
        for (var it = 1; it < arguments.length; ++it)
        {
            total += arguments[it].length;
        }

        if (left >= total)
        {
            return __formatV(_format, arguments, 1);
        }

        var argv = [];
        for (var it = 1; it < arguments.length; ++it)
        {
            var a = new String(arguments[it]);
            var shl = Math.floor(a.length * left / total);
            //argv.push(a + ":" + shl + "|" + a.length + "/" + left + "/" + total); //.substring(0, shl));
            if (shl == 0)
            {
                shl = 1;
            }
            argv.push(a.substring(0, shl));
            left -= shl;
            total -= a.length;
        }

        return __formatV(_format, argv, 0);
    }


    this.Column = _Column;
    this.Grant = _Grant;
    this.ApplyDDL = _ApplyDDL;
    this.ApplyDML = _ApplyDML;
    this.ShortName = _ShortName;

}

var Provider = TSQL; Oracle;
var DB = new Provider();        
            

function Output(txt)
{
    window.clipboardData.setData('Text', txt);
    alert(txt);
}

//]]></script>

                <body>
		   <div class="version">dbschema-tool.xsl v1.2.09</div>

		   <div id="output">
			<div id="menubox">
			<div id="menu" >
				<ul><xsl:call-template name="list-classes" /></ul>
			</div>
			</div>


                        <h2>Tabele w bazie danych:</h2>
                        <xsl:apply-templates select="//sooda:class[@name]|//sooda:relation[@name]">
                            <xsl:sort select="@name"/>
                        </xsl:apply-templates>
                    </div>
            </body>
        </html>
    </xsl:template>

    <!--xsl:template match="sooda:class">
        <xsl:if test="sooda:collectionOneToMany or sooda:collectionManyToMany">
            <p>Kolekcje:</p>
            <ul>
                <xsl:for-each select="sooda:collectionOneToMany">
                    <li><xsl:value-of select="@name" /> - 1-N - element kolekcji - <b><xsl:value-of select="@class" /></b></li>
                </xsl:for-each>
                <xsl:for-each select="sooda:collectionManyToMany">
                    <li><xsl:value-of select="@name" /> - N-N - relacja - <b><xsl:value-of select="@relation" /></b></li>
                </xsl:for-each>
            </ul>
        </xsl:if>
    </xsl:template-->

    <xsl:template name="list-classes">
        <xsl:param name="letters">abcdefghijklmnopqrstuvwxyz</xsl:param>
        
        <xsl:variable name="letter" select="substring($letters, 1, 1)" />
        <xsl:variable name="uletter" select="translate($letter, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />

        <xsl:variable name="classes" select="//sooda:class[starts-with(@name, $letter) or starts-with(@name, $uletter)]|//sooda:relation[starts-with(@name, $letter) or starts-with(@name, $uletter)]" />

        <xsl:if test="count($classes) &gt; 0">
            <li><a><xsl:value-of select="$uletter" />...</a>
                <ul><xsl:for-each select="$classes">
                    <xsl:sort select="@name"/>
                    <li class="item"><xsl:if test="position() != 1"><xsl:attribute name="style">border-top-color: red;</xsl:attribute></xsl:if>
                    <xsl:call-template name="reference"><xsl:with-param name="classname" select="@name" /></xsl:call-template></li>
                </xsl:for-each></ul>
            </li>
        </xsl:if>
        <xsl:if test="string-length($letters) &gt; 1">
            <xsl:call-template name="list-classes"><xsl:with-param name="letters" select="substring($letters, 2)" /></xsl:call-template>
        </xsl:if>
    </xsl:template> <!-- template: list-classes -->


    <xsl:template match="sooda:class">
        <h3><a><xsl:attribute name="name"><xsl:value-of select="generate-id(.)" /></xsl:attribute></a><xsl:value-of select="@name" />
        <xsl:if test="@inheritFrom"><span class="inherit"><xsl:call-template name="inherit"><xsl:with-param name="classname" select="@inheritFrom" /></xsl:call-template></span></xsl:if>
            </h3>
        <xsl:variable name="class" select="@name" />
        <xsl:variable name="references" select="//sooda:class[./sooda:table/sooda:field[@references=$class]]|//sooda:relation[./sooda:table/sooda:field[@references=$class]]" /> <!-- lista odwolan innych klas do naszej klasy -->
            <table border="0" width="100%" class="description">
            
        <xsl:variable name="tables" select=".//sooda:table" />
        <xsl:if  test="count($tables) != 1 or $tables[1]/@name != $class"> <!-- gdy wiecej niz jedna tabela w klasie, lub niezgodnosc nazw -->
            <tr><td class="title" width="35px">
                <xsl:choose>
                    <xsl:when test="count($tables) = 1">Tabela:</xsl:when>
                    <xsl:otherwise>Tabele:</xsl:otherwise>
                </xsl:choose>
            </td><td class="name" width="*">
                <xsl:for-each select="$tables">
                    <xsl:value-of select="@name" />
                    <xsl:if test="position() != last()">, </xsl:if>
                </xsl:for-each>
            </td></tr>
        </xsl:if>

            <xsl:if test="sooda:description"><tr><td class="title" width="35px">Opis:  </td><td class="value" width="*"><xsl:value-of select="sooda:description" /></td></tr></xsl:if>
        <xsl:if test="count($references) &gt; 0"><tr><td class="title" width="35px">Użyta&#160;w:</td><td class="value" width="*">
            <xsl:for-each select="$references"><xsl:sort select="./@name" />
                <xsl:variable name="refclassname" select="./@name" />
                <xsl:call-template name="reference"><xsl:with-param name="classname" select="$refclassname" /></xsl:call-template>

                <xsl:choose>
                    <xsl:when test="count(./sooda:table/sooda:field[@references=$class and @name != $class]) &gt; 0"> <!-- sa pola o nazwie roznej od nazwy klasy -->
                    <span class="std" style="font-size: 9px; vertical-align: center;">&#32;(<xsl:for-each select="./sooda:table/sooda:field[@references=$class]">
                        <xsl:sort select="./@name" />
                        <xsl:if test="./@nullable!='true'">*</xsl:if>
                        <xsl:value-of select="./@name" />
                        <xsl:if test="position() != last()">, </xsl:if>
                    </xsl:for-each>)</span>
                    </xsl:when>
                    <xsl:otherwise> <!-- jest tylko pole o takiej samej nazwie jak nazwa klasy -> jesli wymagane, to gwiazdka -->
                        <xsl:if test="./sooda:table/sooda:field[@references=$class]/@nullable != 'true'"><span class="std" style="font-size: 9px; vertical-align: center;">&#32;(*)</span></xsl:if>
                    </xsl:otherwise>
                </xsl:choose>
                <xsl:if test="position() != last()">, </xsl:if>
            </xsl:for-each>
        </td></tr></xsl:if>
        <xsl:variable name="inheritedin" select="//sooda:class[@inheritFrom = $class]" />
        <xsl:if test="count($inheritedin) &gt; 0"><tr><td class="title" width="35">Rodzic:</td><td class="value" width="*">
            <xsl:for-each select="$inheritedin">
                <xsl:call-template name="reference"><xsl:with-param name="classname" select="./@name" /></xsl:call-template>
                <xsl:if test="position() != last()">, </xsl:if>
            </xsl:for-each>	
        </td></tr>	</xsl:if>
        
            </table>

        <xsl:apply-templates select=".//sooda:table" />

        <!-- obsługa stałych -->
        <xsl:if test="count(.//sooda:const) &gt; 0">
            <table width="50%" class="dane" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="header const" width="180px">Id</td>
                    <td class="header const" width="400px">Nazwa</td>
                    <td class="headerfieldmodificator const" width="20"> <!-- generowanie wszystkich const'ow -->
                        <a style="cursor: hand;"><xsl:attribute name="onclick">Output(DB.AllConsts('<xsl:value-of select="@name" />'));</xsl:attribute>+</a>
                    </td>
                </tr>
                <xsl:for-each select=".//sooda:const">
                    <tr>
                        <td><xsl:value-of select="@key" /></td>
                        <td><xsl:value-of select="@name" /></td>
                        <td class="fieldmodificator"><a style="cursor: hand;"><xsl:attribute name="onclick">Output(DB.Const('<xsl:value-of select="../@name" />', '<xsl:value-of select="@name" />', true));</xsl:attribute>+</a></td>
                    </tr>
                </xsl:for-each>
            </table>
        </xsl:if>
        <!-- end of: obsługa stałych -->
    </xsl:template>  <!-- template: sooda:class -->

    <xsl:template match="sooda:relation">
        <h3><a><xsl:attribute name="name"><xsl:value-of select="generate-id(.)" /></xsl:attribute></a><xsl:value-of select="@name" />
        <xsl:if test="../@inheritFrom"><span class="inherit"><xsl:call-template name="inherit"><xsl:with-param name="classname" select="../@inheritFrom" /></xsl:call-template></span></xsl:if>
            <span class="std">~</span></h3>
        <xsl:apply-templates select=".//sooda:table" />
    </xsl:template> <!-- template: sooda:relation -->


    <xsl:template match="sooda:table">

        
        <table width="100%" class="dane" cellpadding="0" cellspacing="0">
            <tr>
		<td class="header" width="180px">Pole</td>
		<td class="headerfieldmodificator" width="10px">&#160;</td>
                <td class="header" width="180px">Pole w tabeli</td>
                <td class="header" width="160px">Typ / Referencja <i>(typ)</i></td>
                <td class="header" width="*">Opis</td>
                <td class="headerfieldmodificator" width="20"> <!-- generowanie calej tabeli -->
                <a style="cursor: hand;"><xsl:attribute name="onclick">Output(DB.Table('<xsl:value-of select="../@name" />'));</xsl:attribute>+</a>
		</td>
            </tr>

	    <xsl:call-template name="inherit-fields">
		<xsl:with-param name="classname" select="../@name" />
		<xsl:with-param name="thisclass" select="../@name" />
	    </xsl:call-template>
        </table>
	<div>
		
	</div>

        <br/><br/>
    </xsl:template> <!-- template: sooda:table -->

    <xsl:template name="inherit-fields"> <!-- wypisujemy pola w danej klasie i klasach z których ona dziedziczy -->
        <xsl:param name="classname" select="name" /> <!-- nazwa klasy aktualnie przetwarzanej -->
	<xsl:param name="classes"></xsl:param>	         <!-- ciag nazw klas ktore przetwarzamy -->
	<xsl:param name="thisclass" />			 <!-- nazwa klasy od ktorej rozpoczelismy przetwarzanie -->

	<!-- komponujemy string |klasa||klasa nadrzedna||druga klasa nadrzedna| -->
	<xsl:variable name="class" select="(//sooda:class[@name=$classname]|//sooda:relation[@name=$classname])[1]" />
        
	<xsl:if test="$class/@inheritFrom">
	    <xsl:call-template name="inherit-fields">
		<xsl:with-param name="classname" select="$class/@inheritFrom" />
		<xsl:with-param name="classes">|<xsl:value-of select="$classname" />|<xsl:value-of select="$classes" /></xsl:with-param>
		<xsl:with-param name="thisclass" select="$thisclass" />
	    </xsl:call-template>
	</xsl:if>

	<xsl:if test="not($class/@inheritFrom)"> <!-- jeśli już dotarlismy do klasy 'na szczycie - to znaczy ze nie przebieglismy sie po wszystkich klasach -->
		<xsl:variable name="classliststr">|<xsl:value-of select="$classname" />|<xsl:value-of select="$classes" /></xsl:variable>
		<xsl:variable name="classlist" select="(//sooda:class[contains($classliststr, concat('|', @name, '|'))]|//sooda:relation[@name=$classname])" /> <!-- mamy liste klas od danej klasy do klasy bazowej -->

		<xsl:for-each select="$classlist[not(@inheritFrom)]//sooda:field[@primaryKey]">
			<xsl:apply-templates select=".">
				<xsl:with-param name="thisclass" select="$thisclass" />
			</xsl:apply-templates>
		</xsl:for-each>

		<xsl:for-each select="$classlist//sooda:field[not(@primaryKey)]"> <!-- $classlist/sooda:collectionOneToMany -->
			<xsl:sort select="@primaryKey" order="descending" />
			<xsl:sort select="@nullable" />
			<xsl:sort select="@name" />
			<xsl:apply-templates select=".">
				<xsl:with-param name="thisclass" select="$thisclass" />
			</xsl:apply-templates>
		</xsl:for-each>

		<xsl:for-each select="$classlist/sooda:collectionOneToMany|$classlist/sooda:collectionManyToMany">
			<xsl:sort select="@name" />
			<xsl:apply-templates select=".">
				<xsl:with-param name="thisclass" select="$thisclass" />
			</xsl:apply-templates>
		</xsl:for-each>
	</xsl:if>
	   
    </xsl:template>
    
    <xsl:template name="inherit"> <!-- wypisujemy klasę po której dziedziczymy -->
        <xsl:param name="classname" select="name" /> <!-- nazwa klasy po której dziedziczymy -->
        <xsl:variable name="class" select="//sooda:class[@name=$classname][1]" />
        : <xsl:call-template name="reference"><xsl:with-param name="classname" select="$class/@name" /></xsl:call-template>
        <xsl:if test="$class/@inheritFrom">
            <xsl:call-template name="inherit">
                <xsl:with-param name="classname" select="$class/@inheritFrom" />
            </xsl:call-template>
        </xsl:if>
    </xsl:template>

    <xsl:template name="reference"> <!-- wypisujemy nazwę klasy oraz referencję (odnośnik) do niej -->
	<xsl:param name="classname" />
	<xsl:variable name="refclass" select="(//sooda:class[@name=$classname]|//sooda:relation[@name=$classname])" />		    
	<a><xsl:attribute name="href">#<xsl:value-of select="generate-id($refclass)" /></xsl:attribute><xsl:value-of select="$classname" /><xsl:if test="local-name($refclass) = 'relation'"><span class="std">~</span></xsl:if></a>
	
    </xsl:template>

    <xsl:template match="sooda:collectionOneToMany">
	<xsl:param name="thisclass">unknown</xsl:param>
	<xsl:param name="prefix"></xsl:param> <!-- by był zgodny z field -->
	<tr style="background: #FFF5FF">
		<xsl:variable name="field" select="@name" />
		<xsl:variable name="class" select="../@name" />
		<td width="180px"><xsl:if test="$thisclass != $class"><span class="std"><xsl:value-of select="$class" />.</span></xsl:if><xsl:value-of select="$field" /></td>
		<td width="10px" class="fieldmodificator" />
		<td width="180px"><span class="std"><xsl:value-of select="@foreignField" /></span></td>
		<td width="160px"><span class="std">1-</span>N: <xsl:call-template name="reference"><xsl:with-param name="classname" select="@class" /></xsl:call-template></td>
		<td />
		<td />
	</tr>
    </xsl:template>
    <xsl:template match="sooda:collectionManyToMany">
	<xsl:param name="thisclass">unknown</xsl:param>
	<xsl:param name="prefix"></xsl:param> <!-- by był zgodny z field -->
	<tr style="background: #FFF5FF">
		<xsl:variable name="field" select="@name" />
		<xsl:variable name="class" select="../@name" />
		<td width="180px"><xsl:if test="$thisclass != $class"><span class="std"><xsl:value-of select="$class" />.</span></xsl:if><xsl:value-of select="$field" /></td>
		<td width="10px" class="fieldmodificator" />
		<td width="180px"><span class="std"> <xsl:call-template name="reference"><xsl:with-param name="classname" select="@relation" /></xsl:call-template></span></td>
		<td width="160px">M-N: 
			<xsl:variable name="rel" select="@relation" />
			<xsl:variable name="master" select="number(@masterField) + 1" />
			<xsl:call-template name="reference"><xsl:with-param name="classname" select="//sooda:relation[@name=$rel]//sooda:field[$master]/@references" /></xsl:call-template>
		</td>
		<td />
		<td />
	</tr>
    </xsl:template>

    <xsl:template name="db-field-type"> <!-- generuje identyfikator pola (razem z jego typem) w SQL'u -->
        <xsl:param name="field" /><xsl:value-of select="$field/@dbcolumn"/><xsl:text> </xsl:text><xsl:choose> <!-- typ -->
	    <xsl:when test="$field/@type='BooleanAsInteger'">int</xsl:when>
	    <xsl:when test="$field/@type='Integer'">int</xsl:when>
	    <xsl:when test="$field/@type='DateTime'">DateTime</xsl:when>
	    <xsl:when test="$field/@type='Decimal'">decimal<xsl:if test="@size">(<xsl:value-of select="@size" /><xsl:if test="@precision">,<xsl:value-of select="@precision" /></xsl:if>)</xsl:if></xsl:when>
	    <xsl:when test="$field/@type='String'">nvarchar(<xsl:value-of select="@size"/>)</xsl:when>
        </xsl:choose><xsl:text> </xsl:text><xsl:choose> <!-- nullable -->
		<xsl:when test="@nullable = 'true'">null</xsl:when>
		<xsl:otherwise>not null</xsl:otherwise>
	</xsl:choose></xsl:template>

    <xsl:template match="sooda:field">
        <xsl:param name="thisclass">unknown</xsl:param>
	<xsl:param name="prefix"></xsl:param>
        <tr>
            <xsl:variable name="field" select="@name" />
	    <xsl:variable name="class" select="../../@name" />
	    <xsl:variable name="tablename" select="../@name" />
            <xsl:if test="@primaryKey"><xsl:attribute name="style">background: #FFFFCC</xsl:attribute></xsl:if>
            <xsl:if test="@nullable = 'true'"><xsl:attribute name="style">background: #EEFFFF</xsl:attribute></xsl:if>
	    <td width="180px"><xsl:if test="$thisclass != $class"><span class="std"><xsl:value-of select="$class" />.</span></xsl:if><xsl:value-of select="$field" /></td>
	    <td width="10px" class="fieldmodificator">
		<xsl:choose>
			<xsl:when test="@primaryKey"><b>#</b></xsl:when>
			<xsl:when test="@nullable = 'true'"><span class="std">o</span></xsl:when>
			<xsl:otherwise><b>*</b></xsl:otherwise>
		</xsl:choose>
	    </td>
            <td width="180px"><xsl:value-of select="@dbcolumn"/></td>
            <td width="160px">
                <xsl:variable name="type">
                    <xsl:choose>
                        <xsl:when test="@type='BooleanAsInteger'">
                            Boolean
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:value-of select="@type"/>  
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:variable>
                
                <xsl:choose>
                    <xsl:when test="@references">
			<xsl:call-template name="reference"><xsl:with-param name="classname" select="@references" /></xsl:call-template>&#xA0;<i><span class="std">(<xsl:value-of select="$type" />)</span></i></xsl:when>
                    <xsl:otherwise><xsl:value-of select="$type" />
                            <xsl:if test="@type='BooleanAsInteger'"><span class="std">as Integer</span></xsl:if><!-- obejście :-(  - by as Integer moglo byc innego koloru niz null -->
                    </xsl:otherwise> 
                </xsl:choose>
		<xsl:choose>
                    <xsl:when test="@type='BooleanAsInteger'"/> <!-- 4 -->
                    <xsl:when test="@type='Integer'" /> <!-- 4 -->
                    <xsl:when test="@type='DateTime'" /> <!-- 8 -->
                    <xsl:when test="@type='Decimal'">
                        <xsl:choose>
                            <xsl:when test="@size">(<xsl:value-of select="@size" /><xsl:if test="@precision">,<xsl:value-of select="@precision" /></xsl:if>)</xsl:when>
                            <xsl:otherwise><span class="std" alt="rozmiar standardowy">:9</span></xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>(<xsl:value-of select="@size"/>)</xsl:otherwise>
                </xsl:choose>
            </td>
            <td>
	
		<!-- sprawdzamy  czy nie ma bledow, jesli sa to je wypisujemy -->
		<xsl:variable name="dbcolumn" select="@dbcolumn" />
		<xsl:variable name="primarykey" select="@primaryKey = 'true'" />

		<xsl:choose>
		    <xsl:when test="not($field)">
			<div class="error">Brak nazwy pola (field)</div>
		    </xsl:when>
		    <xsl:when test="$field = ''">
			<div class="error">Pusta nazwa pola (field)</div>
		    </xsl:when>
		    <xsl:when test="count(../sooda:field[@name=$field]) != 1">
			<div class="error">Zduplikowana nazwa pola (field)</div>
		    </xsl:when>
		    <xsl:when test="not(@primaryKey) and count(//sooda:table[@name=$tablename]/sooda:field[@name=$field]) != 1">
			<div class="warning">Zduplikowana nazwa pola (field) w tabeli <xsl:value-of select="$tablename" /></div>
		    </xsl:when>
		    <xsl:otherwise />
		 </xsl:choose>

		<xsl:choose>
		    <xsl:when test="not($dbcolumn)">
			<div class="warning">Brak nazwy pola (dbcolumn)</div>
		    </xsl:when>
		    <xsl:when test="$dbcolumn = ''">
			<div class="warning">Pusta nazwa pola (dbcolumn)</div>
		    </xsl:when>
		    <xsl:when test="count(../sooda:field[@dbcolumn=$dbcolumn]) != 1">
			<div class="error">Zduplikowana nazwa pola (dbcolumn) <xsl:value-of select="count(../sooda:field[@dbcolumn=$dbcolumn])" /></div>
		    </xsl:when>
		    <xsl:when test="not(@primaryKey) and count(//sooda:table[@name=$tablename]/sooda:field[@dbcolumn=$dbcolumn]) != 1">
			 <div class="warning">Zduplikowana nazwa pola (dbcolumn) w tabeli <xsl:value-of select="$tablename" /></div>
		    </xsl:when>
		    <xsl:otherwise />
		 </xsl:choose>

		 <xsl:variable name="references" select="@references" />

		 <xsl:if test="string-length($references) &gt; 0 and count(//sooda:class[@name=$references]) = 0">
		    <div class="error">Nie znaleziono klasy referencyjnej <xsl:value-of select="$references" /></div>
		 </xsl:if>

		 <xsl:if test="contains($field, ' ')">
		    <div class="error">Nazwa pola (field) zawiera spację</div>
		 </xsl:if>
		 <xsl:if test="contains($dbcolumn, ' ')">
		    <div class="error">Nazwa kolumny (dbcolumn) zawiera spację</div>
		 </xsl:if>
		 <xsl:if test="(@type = 'String' or @type='AnsiString') and (not(@size) or @size = 0)">
		    <div class="error">Brak rozmiaru pola tekstowego</div>
		 </xsl:if>

		 <xsl:if test="@type != 'String' and @type != 'Decimal' and @type != 'AnsiString' and @size">
		    <div class="error">Podano rozmiar dla pola o typie innym niż String i Decimal</div>
		 </xsl:if>
		 <!-- END OF sprawdzamy  czy nie ma bledow, jesli sa to je wypisujemy -->

		<xsl:if test=". != ''">       <!-- jeśli jest nasz komentarz (w db-schema) - to wstawiamy komentarz z db-schema -->
			<xsl:value-of select="." />
		</xsl:if>
            </td>
	    <td class="fieldmodificator">
            <a style="cursor: hand;"><xsl:attribute name="onclick">Output(DB.AddColumn('<xsl:value-of select="../../@name" />', '<xsl:value-of select="@name" />'));</xsl:attribute>+</a>
		&#32;
            <a style="cursor: hand;"><xsl:attribute name="onclick">Output(DB.AlterColumn('<xsl:value-of select="../../@name" />', '<xsl:value-of select="@name" />'));</xsl:attribute>*</a>
	    </td>
        </tr>
    </xsl:template>

    <xsl:template match="@* | node()" mode="documentation" >
        <xsl:copy>
            <xsl:apply-templates select="@* | node()" mode="documentation" />
        </xsl:copy>
    </xsl:template>

    <xsl:template match="summary">
        <xsl:apply-templates mode="documentation" />
    </xsl:template>


    <xsl:template match="sooda:field|sooda:const" mode="json">{<xsl:apply-templates select="." mode="json_properties" />}</xsl:template>
    <xsl:template match="sooda:class|sooda:relation" mode="json">
        classes['<xsl:value-of select="@name" />'] = {
            <xsl:apply-templates select="." mode="json_properties" />,
            table: { <xsl:apply-templates select="sooda:table" mode="json_properties" /> },
            columns: {<xsl:for-each select="sooda:table/sooda:field">
                "<xsl:value-of select="@name" />":  {<xsl:apply-templates select="." mode="json_properties" />}<xsl:if test="position() != last()">,</xsl:if> 
            </xsl:for-each>
            },
            columnNames: [ <xsl:for-each select="sooda:table/sooda:field">"<xsl:value-of select="@name" />"<xsl:if test="position() != last()">,</xsl:if></xsl:for-each> ],
            consts: {<xsl:for-each select="sooda:const">
                "<xsl:value-of select="@name" />": {<xsl:apply-templates select="." mode="json_properties" />}<xsl:if test="position() != last()">,</xsl:if>
                </xsl:for-each>
            },
            constNames: [ <xsl:for-each select="sooda:const">"<xsl:value-of select="@name" />"<xsl:if test="position() != last()">,</xsl:if></xsl:for-each> ]
        };
    </xsl:template>

    <!-- zrzuca wpis do tablicy obiektów json'owskich -->
    <xsl:template match="*" mode="json_properties" name="json_properties">objectType: "<xsl:value-of select="name()" />"<xsl:apply-templates select="@*" mode="json_properties" /></xsl:template>

    <!-- zrzuca tag do obiektu json'owego -->
    <xsl:template match="@*" mode="json_properties">, "<xsl:value-of select="name()" />": "<xsl:value-of select="." />"</xsl:template>
    

</xsl:stylesheet>

