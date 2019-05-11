using System;
using System.Collections.Generic;

namespace rot1
{
    public class Identifier : Node{
        public Identifier(string _key){
            this.value = _key;
        }
        override public object Evaluate(SymbolTable symbolTable){
            return (symbolTable.Get((string)this.value));
        }
    }
}