

Function Soma(x as integer, y as integer) as integer
    dim a as integer
    a = x + y
    print a
    Soma = a
end function


sub Main ()
    dim a as integer
    dim b as integer
    a = input
    b = Soma (a, 4)
    print(b)
end sub
