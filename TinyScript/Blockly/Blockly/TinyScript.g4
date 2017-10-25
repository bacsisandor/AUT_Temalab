grammar TinyScript;

program: statementList;
statementList: statement*;
statement: variableDeclaration | ifStatement | whileStatement | doWhileStatement | forStatement | assignmentStatement | printStatement ;

variableDeclaration: (typeName | 'var') varName ('=' expression)? ';' ;
typeName: INTTYPE | BOOLEANTYPE | STRINGTYPE;
varName: ID;
condition: expression (COND | '<=') expression;
whileStatement: 'while' '(' condition ')' block ;
doWhileStatement: 'do' block 'while' '(' condition ')' ';' ;
forStatement: 'for' '(' varName '=' expression ';' varName '<=' expression ';' incrementStatement ')' block ;
incrementStatement: (varName '++') | (varName '+=' expression) ;
ifStatement: 'if' '(' condition ')' block elseStatement? ;
elseStatement: 'else' block ;
block: '{' statementList '}' ;
assignmentStatement: varName '=' expression ';' ;
printStatement: 'print' '(' STRING ')' ';' ;

expression: product (PLUSMINUS product)*;
product: signedArgument (MULDIV signedArgument)*;
signedArgument: PLUSMINUS? argument;
argument: varName | value | ('(' expression ')');
value: INT | STRING | BOOLEAN | NULL;

PLUSMINUS: [+-];
MULDIV: [*/];
COND: '<' | '>' | '>=' | '==' | '!=';
LTE: '<=';
INC1: '++';
INC2: '+=';

INTTYPE: 'integer';
BOOLEANTYPE: 'boolean';
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
