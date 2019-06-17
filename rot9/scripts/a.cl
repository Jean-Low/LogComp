Function fibonacci(n as integer) as integer
    ' codigo do raphael costa
    dim flag as boolean
    
    flag = false
    if n = 0 then
        fibonacci = 1
        flag = true
    end if

    if n = 1 then 
        fibonacci = 1
        flag = true
    end if

    if flag = false then
        fibonacci = fibonacci(n-2) + fibonacci(n-1)
    end if

End Function
Sub main()
    dim counter as integer
    counter = 0
    dim max as integer
    max = input
    while (counter < max)
        print fibonacci(counter)
        counter = counter + 1
    wend
End Sub
