using System;
using System.Collections.Generic;
using System.Text;

namespace CustomScaffold.Templates
{
    partial class Controller
    {
        public string EntityName;
        public string ContextGetterString;
        public string EntityIdName;
        public string ForeignKeyDropDownCreationString;
        public string ForeignKeyDropDownCreationWithSelectedIndexString;
        public string FormBindingParams;
        public string FixNullFieldsCode;
        public string ControllerAuthAttrib;
    }

    partial class Index
    {
        public Dictionary<string, string> DisplayOverrides;
        public List<string> ColumnNames;
        public string EntityIdName;
        public string EntityName;
    }

    partial class CreateEdit
    {
        public static readonly string IDENTIFIER_FK = "?";
        public static readonly string IDENTIFIER_HIDDEN = "~";
        public static readonly string IDENTIFIER_DATETIME = "^";

        public string EntityName;
        public string EntityIdName;
        public string Action;
        public List<string> ColumnNames;
    }

    partial class DetailsDelete
    {
        public string EntityName;
        public string EntityIdName;
        public string Action;
        public List<string> ColumnNames;
    }
}
