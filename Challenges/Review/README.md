# Develop Challenge

This challenge is to review the code in the event logger application.

# Requirements

Review the code in the Logger.cs class and suggest any ammendments you feel should be made to improve the code. Fill in your suggestions and reasoning below.

| Line(s) | Suggested Change | Reasoning |
|---------|------------------|-----------|
|  13-20  | Change if-else statement to a conditional statement for example: _et = et==0 ? new EventSourceA() : new EventSourceB(); | As the if statement is to set a value to a variable based on a parameter to the method; it is easier to use a conditional expression as it can easily return a value based on a condition; also seeing as though there are no nested if statements, this also aids in readability. The conditional expression also produces a smaller amount of code, meaning that the code is less bloated   |
|  29-36  |  Either remove try-catch statement or handle thrown exception | Currently exception is getting caught by the try-catch statement and then is getting thrown again, meaning that this code is unnecessary. if the exception is to be caught and handled further up the call stack, this piece of code is unnecessary; if the exception needs to be handled within this method, then the throw ex on line 35 needs to be changed for some code to either re-try the processing of the event or notify the user of the program of the error that has occurred  |
|  4-56   |   Code comments  | Code with good comments is much easier to debug and fix, as what a certain method does can be identified without neededing to execute it or spend large amounts of time looking at the code; this also helps other programmers fix or add additional sections to your code.   |
