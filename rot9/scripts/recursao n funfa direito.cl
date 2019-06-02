
function count (x as integer) as integer
    print(x)
    count = count(x-1)
end function


sub Main ()
    print(count(10)) 
end sub

;impossivel de rodar pelo visto, so subs podem ser recursivas
;isso por conta do jeito como funcoes retornam
;se funcoes retornassem por algo q n tivesse o mesmo nome, daria
