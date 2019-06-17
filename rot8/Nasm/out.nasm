
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
                
                
PUSH DWORD 0
PUSH DWORD 0
PUSH DWORD 0
PUSH DWORD 0
PUSH DWORD 0
PUSH DWORD 0
PUSH DWORD 0
MOV [EBP-16],EBX
MOV EBX, 0
MOV [EBP-4],EBX
MOV EBX, 1111
MOV [EBP-8],EBX
MOV EBX, 1111
MOV [EBP-12],EBX
MOV EBX, True
MOV [EBP-28],EBX
LOOP_1:
MOV EBX, [EBP-16]
PUSH EBX
MOV EBX, 0
POP EAX
CMP EAX, EBX
CALL binop_jg
CMP EBX, False
JE EXIT_1
MOV EBX, [EBP-16]
PUSH EBX
MOV EBX, [EBP-16]
PUSH EBX
MOV EBX, 3
POP EAX
IDIV EBX
MOV EBX, EAX
PUSH EBX
MOV EBX, 3
POP EAX
IMUL EBX
MOV EBX, EAX
POP EAX
SUB EAX, EBX
MOV EBX, EAX
MOV [EBP-20],EBX
MOV EBX, [EBP-16]
PUSH EBX
MOV EBX, [EBP-16]
PUSH EBX
MOV EBX, 5
POP EAX
IDIV EBX
MOV EBX, EAX
PUSH EBX
MOV EBX, 5
POP EAX
IMUL EBX
MOV EBX, EAX
POP EAX
SUB EAX, EBX
MOV EBX, EAX
MOV [EBP-24],EBX
MOV EBX, [EBP-16]
PUSH EBX
CALL print
POP EBX
LOOP_2:
MOV EBX, [EBP-20]
PUSH EBX
MOV EBX, 0
POP EAX
CMP EAX, EBX
CALL binop_je
PUSH EBX
MOV EBX, [EBP-24]
PUSH EBX
MOV EBX, 0
POP EAX
CMP EAX, EBX
CALL binop_je
POP EAX
AND EAX, EBX
CMP EBX, False
JE ELSE_2
MOV EBX, [EBP-12]
PUSH EBX
CALL print
POP EBX
MOV EBX, False
MOV [EBP-28],EBX
JMP ENDIF_2
ELSE_2:
ENDIF_2:
LOOP_3:
MOV EBX, [EBP-20]
PUSH EBX
MOV EBX, 0
POP EAX
CMP EAX, EBX
CALL binop_je
PUSH EBX
MOV EBX, [EBP-28]
PUSH EBX
MOV EBX, True
POP EAX
CMP EAX, EBX
CALL binop_je
POP EAX
AND EAX, EBX
CMP EBX, False
JE ELSE_3
MOV EBX, [EBP-4]
PUSH EBX
CALL print
POP EBX
MOV EBX, False
MOV [EBP-28],EBX
JMP ENDIF_3
ELSE_3:
ENDIF_3:
LOOP_4:
MOV EBX, [EBP-24]
PUSH EBX
MOV EBX, 0
POP EAX
CMP EAX, EBX
CALL binop_je
PUSH EBX
MOV EBX, [EBP-28]
PUSH EBX
MOV EBX, True
POP EAX
CMP EAX, EBX
CALL binop_je
POP EAX
AND EAX, EBX
CMP EBX, False
JE ELSE_4
MOV EBX, [EBP-8]
PUSH EBX
CALL print
POP EBX
MOV EBX, False
MOV [EBP-28],EBX
JMP ENDIF_4
ELSE_4:
ENDIF_4:
MOV EBX, True
MOV [EBP-28],EBX
MOV EBX, [EBP-16]
PUSH EBX
MOV EBX, 1
POP EAX
SUB EAX, EBX
MOV EBX, EAX
MOV [EBP-16],EBX
JMP LOOP_1
EXIT_1:
            ;interrupcione
            POP EBP
            MOV EAX, 1
            INT 0x80
            
            