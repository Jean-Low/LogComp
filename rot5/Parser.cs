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

            //Statements always start with START
            if(tokens.actual.type != "START"){
                throw new SystemException ($"Statements need to start with a START symbol (position {tokens.position}) [Line: {CurrentLine}]");
            }

            while(tokens.actual.type != "EOF"){
                //break lines?
                //
                if(tokens.actual.type == "START"){
                    tokens.SelectNext();
                    if(tokens.actual.type != "LINEBREAK"){
                        throw new SystemException ($"START needs a linebreak (position {tokens.position}) [Line: {CurrentLine}]");
                    }   
                    tokens.SelectNext();
                    //Console.WriteLine(tokens.actual.type);
                    

                    CurrentLine++;
                }

                if(tokens.actual.type != "END"){
                    //Console.WriteLine(tokens.actual.type);
                     root.Add(ParseStatement());
                } else {
                    tokens.SelectNext();
                    if(tokens.actual.type != "LINEBREAK" && tokens.actual.type != "EOF"){
                        throw new SystemException ($"END doesnt end the file or break the line (position {tokens.position}) [Line: {CurrentLine}]");
                    }
                    if(tokens.actual.type != "EOF"){
                        tokens.SelectNext();
                        CurrentLine++;
                    }
                }
            }

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
                    root = new BinOp(); //creat the assign biop node
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
                case "START":
                    return ParseStatements();
                
                default:
                    return new NoOp();
            }

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