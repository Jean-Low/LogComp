using System;
using System.Collections.Generic;

namespace rot1
{
    public class God
    {
        public static void VerifyType(string type,(string,object) ret){
            if(type != ret.Item1){
                throw new SystemException ($"Invalid variable type! expecting '{type}' but received '{ret.Item1}', Line {Parser.CurrentLine}.");
            }
        }

    }
    
}