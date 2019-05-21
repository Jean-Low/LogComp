using System;
using System.IO;
using System.Collections.Generic;

namespace rot1
{
    public class Writer
    {
        static string outFile = "";

        public static Dictionary<string,int> varTable = new Dictionary<string,int>();
        public static int varIndex = 1;
        public static int labelIndex = 1;

        public static void write(string line){
            outFile += Environment.NewLine + line;
        }

        public static void declare(string name){
            varTable.Add(name,varIndex * 4);
            varIndex++;
            outFile += Environment.NewLine + "PUSH DWORD 0";
        }

        public static void assign(string var){
            int index = varTable[var];
            outFile += Environment.NewLine + $"MOV [EBP-{index}],EBX";
        }

        public static void intval(int val){
            outFile += Environment.NewLine + $"MOV EBX, {val}";
        }

        public static void WriteFooter(){
            outFile += 
            @"
            ;interrupcione
            POP EBP
            MOV EAX, 1
            INT 0x80
            
            ";
        }

        public static void WriteFile(){
            WriteFooter();
            File.WriteAllText("./Nasm/out.nasm",outFile);
        }

        public static void WriteHeader(){
            outFile = 
                @"
                ; constantes
                SYS_EXIT equ 1
                SYS_READ equ 3
                SYS_WRITE equ 4
                STDIN equ 0
                STDOUT equ 1
                True equ 1
                False equ 0

                segment .data

                segment .bss ; variaveis
                    res RESB 1

                section .text
                    global _start
                
                print:  ; subrotina print

                PUSH EBP ; guarda o base pointer
                MOV EBP, ESP ; estabelece um novo base pointer

                MOV EAX, [EBP+8] ; 1 argumento antes do RET e EBP
                XOR ESI, ESI

                print_dec: ; empilha todos os digitos
                MOV EDX, 0
                MOV EBX, 0x000A
                DIV EBX
                ADD EDX, '0'
                PUSH EDX
                INC ESI ; contador de digitos
                CMP EAX, 0
                JZ print_next ; quando acabar pula
                JMP print_dec

                print_next:
                CMP ESI, 0
                JZ print_exit ; quando acabar de imprimir
                DEC ESI

                MOV EAX, SYS_WRITE
                MOV EBX, STDOUT

                POP ECX
                MOV [res], ECX
                MOV ECX, res

                MOV EDX, 1
                INT 0x80
                JMP print_next

                print_exit:
                POP EBP
                RET

                ; subrotinas if/while
                binop_je:
                JE binop_true
                JMP binop_false

                binop_jg:
                JG binop_true
                JMP binop_false

                binop_jl:
                JL binop_true
                JMP binop_false

                binop_false:
                MOV EBX, False  
                JMP binop_exit
                binop_true:
                MOV EBX, True
                binop_exit:
                RET

                _start:
                    PUSH EBP ; guarda o base pointer
                
                MOV EBP, ESP ; estabelece um novo pointer

                ;codigo gerado abaixo:
                
                ";
        }



    }
    
}