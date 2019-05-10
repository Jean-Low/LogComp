using System;
using System.Collections.Generic;

namespace rot1
{
    public class NoOp : Node{
        override public object Evaluate(SymbolTable symbolTable){
            return null;
        }
    }
}