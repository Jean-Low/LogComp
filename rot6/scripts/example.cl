PRINT 111111
ab = 2*3 + INPUT * 10
PRINT ab
IF ab< 100 THEN
    PRINT 1
ELSE
    PRINT 2
END IF
PRINT 222222
ab = INPUT
count = 0
WHILE count < ab
    PRINT count
    count = count + 1
WEND
PRINT 333333
target = 168
guess = 0
right = 0
WHILE right = 0
    guess = INPUT
    IF guess > target THEN
        PRINT 1
    ELSE
        IF guess < target THEN
            PRINT 0
        ELSE
            IF guess = target THEN
                PRINT 10
                right = 1
            END IF
        END IF
   END IF
