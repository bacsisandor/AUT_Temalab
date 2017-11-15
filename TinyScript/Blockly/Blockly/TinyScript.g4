grammar TinyScript;

program: variableDeclarationList statementList EOF;
variableDeclarationList: variableDeclaration*;
statementList: statement*;
statement: ifStatement | whileStatement | doWhileStatement | forStatement | assignmentStatement | arrayAssignmentStatement | functionCallStatement | incrementStatement | decrementStatement | readStatement;
variableDeclaration: variableDeclaration1 | variableDeclaration2 | arrayDeclaration | arrayInitialization;

variableDeclaration1: typeName varName ('=' expression)? ';' ;
variableDeclaration2: 'var' varName '=' expression ';' ;
arrayDeclaration: typeName varName '[' expression ']' ';' ;
arrayInitialization: typeName varName '[' ']' '=' '{' (expression (',' expression)*)? '}' ';' ;

whileStatement: 'while' '(' expression ')' block ;
doWhileStatement: 'do' block 'while' '(' expression ')' ';' ;
forStatement: 'for' '(' varName '=' expression ';' expression ';' (incrementation | decrementation) ')' block ;
ifStatement: 'if' '(' expression ')' block elseIfStatement* elseStatement? ;
elseIfStatement: 'else' 'if' '(' expression ')' block ;
elseStatement: 'else' block ;
block: '{' statementList '}' ;
assignmentStatement: varName '=' expression ';' ;
arrayAssignmentStatement: varName '[' expression ']' '=' expression ';' ;
functionCallStatement: functionCall ';' ;
incrementStatement: incrementation ';' ;
decrementStatement: decrementation ';' ;
incrementation: (varName '++') | (varName '+=' expression) ;
decrementation: (varName '--') | (varName '-=' expression) ;
readStatement: 'read' '(' varName ')' ';' ;

expression: sum (compareOp sum)?;
compareOp: EQ | NEQ | LT | LTE | GT | GTE;
sum: product (PLUSMINUS product)*;
product: signedArgument (MULDIV signedArgument)*;
signedArgument: PLUSMINUS? argument;
argument:  varName | value | indexedArray | functionCall | ('(' expression ')');
indexedArray: varName '[' expression ']' ;
functionCall: functionName '(' (expression (',' expression)*)? ')' ;

value: INT | STRING | BOOLEAN | NULL;
typeName: INTTYPE | BOOLEANTYPE | STRINGTYPE;
functionName: ID;
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
DEC1: '--';
DEC2: '-=';

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
VAR: 'var';
READ: 'read';
CURLY1: '{';
CURLY2: '}';
BRACKET1: '(';
BRACKET2: ')';
BRACKET3: '[';
BRACKET4: ']';
COMMA: ',';
NULL: 'null';
BOOLEAN: 'true' | 'false';
STRING: '"' (~[\r\n])* '"';
INT: [0-9]+ ;
ID: [a-zA-Z][a-zA-Z0-9_]*;
WS: (' '| '\t' | '\n' | '\r') -> skip;
