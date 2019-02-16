using System;
using System.Collections.Generic;
using System.Text;

namespace System.CodeDom.Compiler
{
    public class CompilerError : Exception
    {
        public string ErrorText;
        public bool IsWarning;
    }

    public class CompilerErrorCollection : List<CompilerError>
    {

    }
}
