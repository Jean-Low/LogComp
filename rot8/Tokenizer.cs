using System;
using System.Collections.Generic;
using System.Linq;

namespace rot1
{
    public class Tokenizer
    {
        public string origin; //source string
        public int position = 0; //actual token separation pos
        public Token actual; //last parsed token

        public bool comment = false; //check if inside a comment


        private List<String> reservedWords = new List<String>(){"END","WEND","PRINT","INPUT","IF","ELSE","THEN","WHILE","SUB",
                                                                "MAIN","INTEGER","BOOLEAN","DIM","AS","TRUE","FALSE","AND","OR","NOT"};
        public Token SelectNext(){
            //Console.WriteLine("next token");
            Token ret =  new Token();
            string tokenizable = "";
            while(true){
                //If origin got to the end or comment starter (') detected
                if(position >= origin.Length){
                    if(tokenizable.Length > 0){
                        int value;
                        if(int.TryParse(tokenizable, out value)){
                            ret.type = "INT";
                            ret.value = value;
                            actual = ret;
                            return actual;
                        } else {
                            throw new SystemException($"Unexpected EOF could not convert to Integer ( {tokenizable} )");
                        }
                    }
                    ret.type = "EOF";
                    actual = ret;
                    return ret;
                }


                //comment
                if(comment) {
                    if(origin[position] == '\n'){
                        comment = false;
                    }
                    position++;
                    continue;
                }

                else if (origin[position] == '\''){
                    comment = true;
                    position++;
                    continue;
                }

                //Handle sign case
                if("+-*/()=><\n".Contains(origin[position])){
                    if(tokenizable.Length == 0){ //token is a sign or simbol
                        switch(origin[position]){
                            case '+':
                                ret.type = "PLUS";
                                break;
                            case '-':
                                ret.type = "MINUS";
                                break;
                            case '*':
                                ret.type = "TIMES";
                                break;
                            case '/':
                                ret.type = "DIV";
                                break;
                            case '(':
                                ret.type = "POPEN";
                                break;
                            case ')':
                                ret.type = "PCLOSE";
                                break;
                            case '=':
                                ret.type = "EQUAL";
                                break;
                            case '\n':
                                ret.type = "LINEBREAK";
                                actual = ret;
                                break;
                            case '>':
                                ret.type = "HIGHER";
                                break;
                            case '<':
                                ret.type = "LOWER";
                                break;
                        }
                        position++;
                        actual = ret;
                        return ret;
                    }
                    //token was a number or ident and ended in a sign (so the sign is not included and the token ended)
                    if(ret.type == "IDENTIFIER"){
                        //same as identifier ending in space
                        string upToken = tokenizable.ToUpper();
                        if(reservedWords.Contains(upToken)){
                            ret.type = upToken;
                            actual = ret;
                            return ret;
                        }
                        ret.type = "IDENTIFIER";
                        ret.value = tokenizable;
                        actual = ret;
                        return ret;
                    } else {
                        int value;
                        if(int.TryParse(tokenizable, out value)){
                            ret.type = "INT";
                            ret.value = value;
                            actual = ret;
                            return actual;
                        } else {
                            throw new SystemException($"Unexpected not parseble integer found ( {tokenizable} )");
                        }
                    }
                }

                //char is part of a number
                if(int.TryParse(origin[position].ToString(), out int dummy)) { //not +|- and is a number;
                    tokenizable += origin[position];
                    position++;
                }

                //char is a letter (can be identifier, space or a invalid char token)
                else { 
                    if(origin[position] == ' ' || origin[position] == '\t'){ //is a space?
                        if(tokenizable.Length == 0){ //ignore space
                            position++;
                        } else { //handle end of ident or number token
                            //v2.1: Bug where a number is considered a identifier if there is a space after it. doublechecking it here
                            if(int.TryParse(tokenizable, out int value)){
                                ret.type = "INT";
                                ret.value = value;
                                actual = ret;
                                return actual;
                            }
                            string upToken = tokenizable.ToUpper();
                            if(reservedWords.Contains(upToken)){
                                ret.type = upToken;
                                actual = ret;
                                return ret;
                                
                            }
                            //check if word is reserved
                            ret.type = "IDENTIFIER";
                            ret.value = tokenizable;
                            actual = ret;
                            return ret;
                            //set ret
                            //save actual
                            //return ret
                        }
                        
                    } else if(tokenizable.Length == 0){ //start with letter or '_'
                        if(Char.IsLetter(origin[position]) || origin[position] == '_'){
                            ret.type = "IDENTIFIER";
                            tokenizable+= origin[position];
                            position++;
                        } else {
                            throw new SystemException($"Unexpected token found, identifiers should start with '_' or a letter. ( {origin[position]} )");
                        }
                    } else{ //token exist, found found chars after letter or '_'
                        if(ret.type == "IDENTIFIER"){
                            tokenizable+= origin[position];
                            position++;
                        } else {
                            throw new SystemException($"Unexpected token found, unknow char at position {position} ( {origin[position]} )");
                        }
                    }
                    
                }
                /* (V1.1.1) (Changed to last two if statements. Now throws an error if invalid char to showcase comment feature better)
                else { //if this branch runs, this char is not in my alphabet, so it will be ignored;
                    position++;
                }
                */

            }
        }

    }
}
