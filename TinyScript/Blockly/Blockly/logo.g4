grammar logo;

program: statementList EOF;

statementList: statement*;

statement: forward | back | left | right | setxy | setx | sety | setheading | home | clean | clearscreen | pendown | penup | repeat;

forward: FORWARD expr;
back: BACK expr;
left: LEFT expr;
right: RIGHT expr;
setxy: SETXY expr expr;
setx: SETX expr;
sety: SETY expr;
setheading: SETHEADING expr;
home: HOME;
clean: CLEAN;
clearscreen: CLEARSCREEN;
pendown: PENDOWN;
penup: PENUP;
repeat: REPEAT INT '[' statementList ']';

expr: product (ADDSUB product)*;

product: signedArgument (MULDIV signedArgument)*;

signedArgument: ADDSUB? argument;

argument: ('(' expr ')') | INT;

INT: [0-9]+;
ADDSUB: [+-];
MULDIV: [*/];
BRACKET1: '(';
BRACKET2: ')';
FORWARD: 'forward' | 'fd';
BACK: 'back' | 'bk';
LEFT: 'left' | 'lt';
RIGHT: 'right' | 'rt';
SETXY: 'setxy';
SETX: 'setx';
SETY: 'sety';
SETHEADING: 'setheading' | 'seth';
HOME: 'home';
CLEAN: 'clean';
CLEARSCREEN: 'clearscreen' | 'cs';
PENDOWN: 'pendown' | 'pd';
PENUP: 'penup' | 'pu';
REPEAT: 'repeat';
BRACKET3: '[';
BRACKET4: ']';
WS: (' '| '\t' | '\n' | '\r') -> skip;