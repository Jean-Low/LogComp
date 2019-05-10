using System;
using System.Collections.Generic;

namespace rot1
{
    public class IntVal : Node{
        override public object Evaluate(SymbolTable symbolTable){
            return (int) value;
            
        }
    }
}