# Note #
If you are looking for examples there is a directory full of them in Interpreter/Examples.

# PLang #
This is the repo for my fairly simple interpreted language.

It is dynamically typed. It is inspired by lua and a few other languages.

The implementation of it is written in C#. The aim of the project was not speed or efficiency, but to learn.

I have Trello board: https://trello.com/b/7mf0E5qw/plang

It is still rough around the edges and doesn't have much error handling, but it works for the most part.

# Current implemented features #
- Lexer (Code -> Tokens)
- Parser (Tokens -> AST)
- Runtime (Run AST)
- Variables and functions both global and local
- Scoping
- Function parameters
- Functions as first class objects
- Returning
- I/O printing and reading, console control
- Logical binary and unary operators
- Arithmetics
- Interop with C#
- While, For, If, Else
- Null
- Arrays
- A very simple barebones standard library, written in C#
- A require function, imports a different file's functions and variables