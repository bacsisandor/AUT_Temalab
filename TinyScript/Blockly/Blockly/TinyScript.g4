grammar TinyScript;

program: variableDeclarationList statementList EOF;
variableDeclarationList: variableDeclaration*;
statementList: statement*;
statement: ifStatement | whileStatement | doWhileStatement | forStatement | assignmentStatement | printStatement ;
variableDeclaration: variableDeclaration1 | variableDeclaration2 ;

variableDeclaration1: typeName varName ('=' expression)? ';' ;
variableDeclaration2: 'var' varName '=' expression ';' ;

whileStatement: 'while' '(' expression ')' block ;
doWhileStatement: 'do' block 'while' '(' expression ')' ';' ;
forStatement: 'for' '(' varName '=' expression ';' varName '<=' expression ';' incrementStatement ')' block ;
incrementStatement: (varName '++') | (varName '+=' expression) ;
ifStatement: 'if' '(' expression ')' block elseIfStatement* elseStatement? ;
elseIfStatement: 'else' 'if' '(' expression ')' block ;
elseStatement: 'else' block ;
block: '{' statementList '}' ;
assignmentStatement: varName '=' expression ';' ;
printStatement: 'print' '(' expression ')' ';' ;

expression: sum (compareOp sum)?;
compareOp: EQ | NEQ | LT | LTE | GT | GTE;
sum: product (PLUSMINUS product)*;
product: signedArgument (MULDIV signedArgument)*;
signedArgument: PLUSMINUS? argument;
argument: varName | value | ('(' expression ')');

value: INT | STRING | BOOLEAN | NULL;
typeName: INTTYPE | BOOLEANTYPE | STRINGTYPE;
varName: ID;

PLUSMINUS: [+-];
MULDIV: [*/];
EQ: '==';
NEQ: '!=';
LT: '<';
LTE: '<=';
GT: '>';
GTE: '>=';
INC1: '++';
INC2: '+=';

INTTYPE: 'integer' | 'int';
BOOLEANTYPE: 'boolean' | 'bool';
STRINGTYPE: 'string';

SEMI: ';';
EQUALS: '=';
IF: 'if';
ELSE: 'else';
WHILE: 'while';
DO: 'do';
FOR: 'for';
PRINT: 'print';
VAR: 'var';
CURLY1: '{';
CURLY2: '}';
BRACKET1: '(';
BRACKET2: ')';
NULL: 'null';
BOOLEAN: 'true' | 'false';
STRING: '"' (~[\r\n])* '"';
INT: [0-9]+ ;
ID: [a-zA-Z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;
