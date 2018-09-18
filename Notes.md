These are some notes I took while following the guide:

> A scanner (or “lexer”) takes in the linear stream of characters and chunks them together into a series of something more akin to “words”. In programming languages, each of these words is called a token. Some tokens are single characters, like ( and ,. Others may be several characters long, like numbers (123), string literals ("hi!"), and identifiers (min).

> The next step is parsing. This is where our syntax gets a grammar—the ability to compose larger expressions and statements out of smaller parts...  
> ...A parser takes the flat sequence of tokens and builds a tree structure that mirrors the nested nature of the grammar.