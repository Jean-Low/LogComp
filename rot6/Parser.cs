using System;
using System.Collections.Generic;


namespace rot1
{
    public static class Parser
    {
        static Tokenizer tokens; //token queue 
        static int CurrentLine;
        public static Node ParseStatements(){
            //Console.WriteLine("parsing expression");
            //Diagram rules
            //1st -> Begin\n
            //BEGIN\n -> Statement
            //BEGIN\n -> END
            //Statement -> Statement
            //Statement -> END
            
            Statements root = new Statements();

            /* V2.2 removed START , left here for reference on fiture Handouts
            //END check
            bool ended = false;

          
            //Statements always start with START
            if(tokens.actual.type != "START"){
                throw new SystemException ($"Statements need to start with a START symbol (position {tokens.position}) [Line: {CurrentLine}]");
            }
            tokens.SelectNext();
            if(tokens.actual.type != "LINEBREAK"){
                throw new SystemException ($"START needs a linebreak (position {tokens.position}) [Line: {CurrentLine}]");
            }   
            tokens.SelectNext();
            CurrentLine++;
            */

            while(tokens.actual.type != "EOF"){

                /*
                if(tokens.actual.type == "START"){
                    root.Add(ParseStatements());
                }
                 */
                //If line is empty, skip this line;
                if(tokens.actual.type == "LINEBREAK"){
                    tokens.SelectNext();
                    CurrentLine++;
                    continue; //double check to see if this does what I think it does!!!
                }

                //Return if ELSE line is found
                if(tokens.actual.type == "ELSE"){
                    break;
                }
                
                if(tokens.actual.type != "END" && tokens.actual.type != "WEND"){
                    //Console.WriteLine(tokens.actual.type);
                     root.Add(ParseStatement());
                } else {
                    //ended = true;
                    tokens.SelectNext();
                    if(tokens.actual.type != "LINEBREAK" && tokens.actual.type != "IF" && tokens.actual.type != "EOF"){
                        throw new SystemException ($"Invalid END construction. (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    if(tokens.actual.type == "LINEBREAK"){
                        tokens.SelectNext();
                        CurrentLine++;
                    }
                    break;
                }
            }
            /*
            if(ended){
                 return root;
            }
            throw new SystemException ($"END symbol not found, probably missing or misplacing a END line (position {tokens.position}) [Line: {CurrentLine}]");

            */
            return root;
           
        }
        public static Node ParseStatement(){
            //Console.WriteLine("parsing expression");
            //Diagram rules
            //1st - Identfier
            //identifier - =
            //= - expression
            //1st - print
            //print - expression
            //1st - statements
            
            Node root;

            //TODO- condense this code by replacing similar code with some functions (expect line end for example)

            switch(tokens.actual.type){
                case "END":
                    return new NoOp();
                case "IDENTIFIER":
                    // IDENTIFIER - = - EXPRESSION
                    // get identifier
                    Identifier ident = new Identifier((string)tokens.actual.value);
                    tokens.SelectNext();
                    if(tokens.actual.type != "EQUAL"){ //expecting a EQUAL
                        throw new SystemException ($"Identifier with no assignment ({tokens.actual.value}) (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    root = new BinOp(); //create the assign biop node
                    root.value = '=';
                    root.children[0] = ident; //left is the identfier
                    tokens.SelectNext();
                    root.children[1] = ParseExpression(); //right is a expression

                    if(tokens.actual.type != "LINEBREAK"){ //expect linebreak after the expression;
                        throw new SystemException ($"No LINEBREAK after assignment (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    CurrentLine++;
                    tokens.SelectNext(); //go to next token after linebreak
                    return root;
                case "PRINT":
                    //print - expression
                    root = new UnOp();
                    root.value = "print";
                    tokens.SelectNext();
                    root.children[0] = ParseExpression();
                    if(tokens.actual.type != "LINEBREAK"){ //expect linebreak after the expression;
                        throw new SystemException ($"No LINEBREAK after PRINT (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    CurrentLine++;
                    tokens.SelectNext(); //go to next token after linebreak
                    return root;
                case "WHILE":
                    root = new WhileNode();
                    tokens.SelectNext();
                    root.children[0] = ParseRelExpression();
                    if(tokens.actual.type != "LINEBREAK"){ //expect linebreak after the expression;
                        throw new SystemException ($"No LINEBREAK after Relative Expression (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    CurrentLine++;
                    tokens.SelectNext(); //go to next token after linebreak
                    root.children[1] = ParseStatements(); //already goes to next line, no select next required
                    return root;
                case "IF":
                    root = new IfNode();
                    tokens.SelectNext();
                    root.children[0] = ParseRelExpression();

                    if(tokens.actual.type != "THEN"){ //expect THEN after the rel expression;
                        throw new SystemException ($"THEN is required after an IF's Relative Expression (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    tokens.SelectNext();

                    if(tokens.actual.type != "LINEBREAK"){ //expect linebreak after THEN;
                        throw new SystemException ($"No LINEBREAK after Relative Expression (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    CurrentLine++;
                    tokens.SelectNext(); //go to next token after linebreak
                    
                    root.children[1] = ParseStatements(); //already goes to next line, no select next required
                    
                    if(tokens.actual.type == "ELSE"){
                        tokens.SelectNext();
                        if(tokens.actual.type != "LINEBREAK"){ //expect linebreak after ELSE;
                            throw new SystemException ($"No LINEBREAK after ELSE (position {tokens.position}) [Line: {CurrentLine}]");
                        }
                        CurrentLine++;
                        tokens.SelectNext(); //go to next token after linebreak
                        root.children[2] = ParseStatements(); //left the token after the END, so it is probably on IF (END IF\n)
                    }

                    if(tokens.actual.type != "IF"){ //expect IF after the statements END for END IF\n construction;
                        throw new SystemException ($"IF section end symbol not found, found END instead of END IF (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    tokens.SelectNext();

                    if(tokens.actual.type != "LINEBREAK"){ //expect linebreak after THEN;
                        throw new SystemException ($"No LINEBREAK after Relative Expression (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    CurrentLine++;
                    tokens.SelectNext(); //go to next token after linebreak

                    return root;
                
                default:
                    return new NoOp();
            }

        }
        public static Node ParseRelExpression(){
            //Console.WriteLine("parsing relative expression");
            //Diagram rules
            //1st - expression
            //expression -> equal | higher | lower
            //equal | higher| lower -> expression
            
            Node root = new BinOp();
            root.children[0] = ParseExpression();
            switch(tokens.actual.type){
                case "EQUAL":
                    root.value = "==";
                    break;
                case "HIGHER":
                    root.value = ">";
                    break;
                case "LOWER":
                    root.value = "<";
                    break;
            }
            tokens.SelectNext();
            root.children[1] = ParseExpression();

            return root;
        }

            public static Node ParseExpression(){
            //Console.WriteLine("parsing expression");
            //Diagram rules
            //1st - term
            //term -> minus | plus
            //term -> EOF
            //minus | plus -> term
            
            Node root = ParseTerm();

            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"PLUS","MINUS"};

            while(allowedTokens.Contains(tokens.actual.type)){

                //make a new node, current root is the new node left child
                Node temp = root;
                root = new BinOp();
                root.children[0] = temp;
                
                root.value = (tokens.actual.type == "MINUS") ? '-' : '+';
                tokens.SelectNext();
                root.children[1] = ParseTerm();

            }

            return root;
        }

        public static Node ParseTerm(){

            //Console.WriteLine("Parsing term");
            //Term rules
            //1st - factor
            //factor -> times | div
            //factor -> exit term
            //times | div -> factor

            //Get first factor
            Node root = ParseFactor();
            
            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"TIMES","DIV"};
            //check if token type is allowed
            while(allowedTokens.Contains(tokens.actual.type)){
                //make a new node, current root is the new node left child
                Node temp = root;
                root = new BinOp();
                root.children[0] = temp;
                
                root.value = (tokens.actual.type == "TIMES") ? '*' : '/';
                tokens.SelectNext();
                root.children[1] = ParseFactor();
            }
            return root;
        }

        public static Node ParseFactor(){
            
            //Console.WriteLine("Parsing Factor");

            //Factor rules
            //1st -> num
            //1st -> +
            //1st -> -
            //1st -> (
            //+ -> factor
            //- -> factor
            //( -> expression
            //expression -> )
            //factor acts as num

            Node root;

            switch(tokens.actual.type){
                    case "INT":
                        root = new IntVal();
                        root.value = tokens.actual.value;
                        tokens.SelectNext();
                        return(root);
                    case "IDENTIFIER":
                        root = new Identifier((string)tokens.actual.value);
                        root.value = tokens.actual.value;
                        tokens.SelectNext();
                        return(root);
                    case "PLUS":
                    case "MINUS":
                        root = new UnOp();
                        root.value = tokens.actual.type == "PLUS" ? '+': '-';
                        tokens.SelectNext();
                        root.children[0] = ParseFactor();
                        return root;
                    case "POPEN":
                        tokens.SelectNext();
                        root = ParseExpression();
                        if(tokens.actual.type != "PCLOSE"){
                            throw new SystemException ($"Missing closing parentesis (position {tokens.position}) [Line: {CurrentLine}]");
                        }
                        tokens.SelectNext();
                        return root;
                    case "INPUT":
                        root = new UnOp();
                        root.value = "input";
                        tokens.SelectNext();
                        return(root);
                }

            //End of term reached, but exiting was not allowed
            throw new SystemException ($"Invalid expression format. Expression end was unexpected(position {tokens.position}) [Line: {CurrentLine}]");
        }
        
        public static int Run(string code, bool debug){
            
            
            SymbolTable table = new SymbolTable();

            tokens = new Tokenizer();
            tokens.origin = code;
            tokens.SelectNext(); //initialize a token
            CurrentLine = 1;
            Console.WriteLine("Now parsing!");
            Node rootStatements = ParseStatements();
            Console.WriteLine("PARSED!\nNow evaluating!\n");
            return( (int) rootStatements.Evaluate(table)); //return signs : 1 - success
        }
    }
}