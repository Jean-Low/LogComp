using System;
using System.Collections.Generic;

namespace rot1
{
    public class SymbolTable
    {
        private Dictionary<string,(string,object)> table;
        public SymbolTable parent = null;
        public SymbolTable(){
            table = new Dictionary<string,(string,object)>();
        }
        // possible types: int, bool, none, function
        public (string,object) Get(string key){ //TODO return a sign to throw the error in parser, so i can throw it with the line index
            (string,object) value;
            if(table.ContainsKey(key)){
                value = table[key];
                return value;
            } else {
                if(parent != null){
                    value = parent.Get(key);
                    return value;
                }
                throw new SystemException ($"Undefined Variable! ({key}). Line {Parser.CurrentLine}");
            }
        }

        public int Set(string key, object value, string type){
            if(table.ContainsKey(key)){
                if(table[key].Item1 != type){
                    throw new SystemException ($"Invalid assing of type {type} to variable of type {table[key].Item1}. ({key})");
                }
                table[key] = (type,value);
                return 1; //assigned
            }
            if(value == null){
                table.Add(key,(type,value));
                return 2; //instantiated
            }
            throw new SystemException ($"Variable {key} was not initialized. {table[key].Item1}. ({key})");
        }
        
        
    }
}