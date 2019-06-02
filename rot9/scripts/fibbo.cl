

sub fibbo(x as integer, y as integer,num as integer)
    dim a as integer
    a = y + x
    print(x)
    if(num > 0) then 
        call fibbo(a,x, num - 1)
    end if
end sub


sub Main ()
    call fibbo(1,0,input) 
end sub
