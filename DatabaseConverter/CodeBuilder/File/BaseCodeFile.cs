using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.CodeBuilder.File
{
    public abstract class BaseCodeFile
    {
        private StringBuilder _builder;
        protected StringBuilder CodeWriter { get { return _builder; } }

        protected string StartBracket = "{";

        protected string EndBracket = "}";
        public BaseCodeFile() { _builder = new(); }

        /// <summary>
        /// Store data into StringBuilder <see cref="_builder"/>
        /// </summary>
        public abstract StringBuilder Save(bool lastBracket = false);


    }
}
