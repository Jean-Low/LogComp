using System;
using System.Collections.Generic;

namespace rot1
{
    public class SymbolTable
    {
        private Dictionary<string,object> table;

        public SymbolTable(){
            table = new Dictionary<string,object>();
        }

        public object Get(string key){ //TODO return a sign to throw the error in parser, so i can throw it with the line index
            object value;
            try{
                value = table[key];
            } catch(Exception dummy_error) {
                throw new SystemException ($"Undefined Variable! ({key})");
            }
            return table[key];
        }

        public int Set(string key, object value){
            if(table.ContainsKey(key)){
                table[key] = value;
                return 2;
            }
            table.Add(key,value);
            return 1; //1 - created key | 2 - replaced key;
        }
        
    }
}