/**
 * @license
 * Visual Blocks Language
 *
 * Copyright 2012 Google Inc.
 * https://developers.google.com/blockly/
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * @fileoverview Helper functions for generating Logo for blocks.
 * @author fraser@google.com (Neil Fraser)
 */
'use strict';

goog.provide('Blockly.Logo');

goog.require('Blockly.Generator');


/**
 * Logo code generator.
 * @type {!Blockly.Generator}
 */
Blockly.Logo = new Blockly.Generator('Logo');

/**
 * List of illegal variable names.
 * This is not intended to be a security feature.  Blockly is 100% client-side,
 * so bypassing this list is trivial.  This is intended to prevent users from
 * accidentally clobbering a built-in object or function.
 * @private
 */
Blockly.Logo.addReservedWords(
    'word,list,sentence,se,fput,lput,array,mdarray,listtoarray,arraytolist,'+
    'combine,reverse,gensym,first,last,firsts,butfirst,bf,butlast,bl,butfirsts,'+
    'bfs,item,mditem,pick,remove,remdup,quoted,split,setitem,mdsetitem,.setfirst,'+
    '.setbf,.setitem,push,pop,queue,dequeue,wordp,word?,listp,list?,arrayp,array?,'+
    'numberp,number?,emptyp,empty?,equalp,equal?,notequalp,notequal?,beforep,before?,'+
    '.eq,memberp,member?,substringp,substring?,count,ascii,char,member,uppercase,lowercase,'+
    'standout,parse,runparse,print,pr,type,show,readlist,readword,cleartext,ct,settextcolor,'+
    'textcolor,increasefont,decreasefont,settextsize,textsize,setfont,font,sum,difference,'+
    'product,quotient,power,remainder,modulo,minus,abs,round,sqrt,exp,log10,ln,'+
    'arctan,sin,cos,tan,radarctan,radsin,radcos,radtan,iseq,rseq,lessp,less?,greaterp,greater?,'+
    'lessequalp,lessequal?,greaterequalp,greaterequal?,random,rerandom,form,'+
    'bitand,bitor,bitxor,bitnot,ashift,lshift,true,false,and,or,xor,not,'+
    'forward,fd,back,bk,left,lt,right,rt,setpos,setxy,setx,sety,setheading,seth,home,arc,'+
    'pos,xcor,ycor,heading,towards,scrunch,showturtle,st,hideturtle,ht,clean,clearscreen,cs,'+
    'wrap,window,fence,fill,filled,label,setlabelheight,setlabelfont,setscrunch,shownp,shown?,'+
    'turtlemode,labelsize,labelfont,pendown,pd,penup,pu,penpaint,ppt,penerase,pe,penreverse,px,'+
    'setpencolor,setpalette,setpensize,setbackground,setscreencolor,setsc,pendownp,pendown?,'+
    'penmode,pencolor,pc,palette,pensize,background,bg,getscreencolor,getsc,mousepos,clickpos,'+
    'buttonp,button?,button,to,define,def,text,copydef,make,name,local,localmake,thing,global,'+
    'pprop,gprop,remprop,plist,procedurep,procedure?,primitivep,primitive?,definedp,defined?,'+
    'namep,name?,plistp,plist?,contents,buried,procedures,primitives,globals,names,plists,namelist,'+
    'pllist,arity,erase,erall,erps,erns,erpls,ern,epl,bury,buryall,buryname,unbury,unburyall,unburyname,'+
    'buriedp,buried?,run,runresult,repeat,forever,repcount,test,if,ifelse,iftrue,ift,iffalse,iff,stop,'+
    'output,op,catch,throw,error,wait,bye,.maybeoutput,ignore,for,do.while,while,do.until,until,case,cond,'+
    'apply,invoke,foreach,map,filter,find,reduce,crossmap'
);

/**
 * Order of operation ENUMs.
 * http://docs.python.org/reference/expressions.html#summary
 */
Blockly.Logo.ORDER_ATOMIC = 0;            // 0 "" ...
Blockly.Logo.ORDER_COLLECTION = 1;        // tuples, lists, dictionaries
Blockly.Logo.ORDER_STRING_CONVERSION = 1; // `expression...`
Blockly.Logo.ORDER_MEMBER = 2.1;          // . []
Blockly.Logo.ORDER_FUNCTION_CALL = 2.2;   // ()
Blockly.Logo.ORDER_EXPONENTIATION = 3;    // **
Blockly.Logo.ORDER_UNARY_SIGN = 4;        // + -
Blockly.Logo.ORDER_BITWISE_NOT = 4;       // ~
Blockly.Logo.ORDER_MULTIPLICATIVE = 5;    // * / // %
Blockly.Logo.ORDER_ADDITIVE = 6;          // + -
Blockly.Logo.ORDER_BITWISE_SHIFT = 7;     // << >>
Blockly.Logo.ORDER_BITWISE_AND = 8;       // &
Blockly.Logo.ORDER_BITWISE_XOR = 9;       // ^
Blockly.Logo.ORDER_BITWISE_OR = 10;       // |
Blockly.Logo.ORDER_RELATIONAL = 11;       // in, not in, is, is not,
                                            //     <, <=, >, >=, <>, !=, ==
Blockly.Logo.ORDER_LOGICAL_NOT = 12;      // not
Blockly.Logo.ORDER_LOGICAL_AND = 13;      // and
Blockly.Logo.ORDER_LOGICAL_OR = 14;       // or
Blockly.Logo.ORDER_CONDITIONAL = 15;      // if else
Blockly.Logo.ORDER_LAMBDA = 16;           // lambda
Blockly.Logo.ORDER_NONE = 99;             // (...)

/**
 * List of outer-inner pairings that do NOT require parentheses.
 * @type {!Array.<!Array.<number>>}
 */
Blockly.Logo.ORDER_OVERRIDES = [
  // (foo()).bar -> foo().bar
  // (foo())[0] -> foo()[0]
  [Blockly.Logo.ORDER_FUNCTION_CALL, Blockly.Logo.ORDER_MEMBER],
  // (foo())() -> foo()()
  [Blockly.Logo.ORDER_FUNCTION_CALL, Blockly.Logo.ORDER_FUNCTION_CALL],
  // (foo.bar).baz -> foo.bar.baz
  // (foo.bar)[0] -> foo.bar[0]
  // (foo[0]).bar -> foo[0].bar
  // (foo[0])[1] -> foo[0][1]
  [Blockly.Logo.ORDER_MEMBER, Blockly.Logo.ORDER_MEMBER],
  // (foo.bar)() -> foo.bar()
  // (foo[0])() -> foo[0]()
  [Blockly.Logo.ORDER_MEMBER, Blockly.Logo.ORDER_FUNCTION_CALL],

  // not (not foo) -> not not foo
  [Blockly.Logo.ORDER_LOGICAL_NOT, Blockly.Logo.ORDER_LOGICAL_NOT],
  // a and (b and c) -> a and b and c
  [Blockly.Logo.ORDER_LOGICAL_AND, Blockly.Logo.ORDER_LOGICAL_AND],
  // a or (b or c) -> a or b or c
  [Blockly.Logo.ORDER_LOGICAL_OR, Blockly.Logo.ORDER_LOGICAL_OR]
];

/**
 * Initialise the database of variable names.
 * @param {!Blockly.Workspace} workspace Workspace to generate code from.
 */
Blockly.Logo.init = function(workspace) {
  /**
   * Empty loops or conditionals are not allowed in Python.
   */
  Blockly.Logo.PASS = this.INDENT + 'pass\n';
  // Create a dictionary of definitions to be printed before the code.
  Blockly.Logo.definitions_ = Object.create(null);
  // Create a dictionary mapping desired function names in definitions_
  // to actual function names (to avoid collisions with user functions).
  Blockly.Logo.functionNames_ = Object.create(null);

  if (!Blockly.Logo.variableDB_) {
    Blockly.Logo.variableDB_ =
        new Blockly.Names(Blockly.Logo.RESERVED_WORDS_);
  } else {
    Blockly.Logo.variableDB_.reset();
  }

  var defvars = [];
  var variables = workspace.getAllVariables();
  for (var i = 0; i < variables.length; i++) {
    defvars[i] = Blockly.Logo.variableDB_.getName(variables[i].name,
        Blockly.Variables.NAME_TYPE) + ' = None';
  }
  Blockly.Logo.definitions_['variables'] = defvars.join('\n');
};

/**
 * Prepend the generated code with the variable definitions.
 * @param {string} code Generated code.
 * @return {string} Completed code.
 */
Blockly.Logo.finish = function(code) {
  // Convert the definitions dictionary into a list.
  var imports = [];
  var definitions = [];
  for (var name in Blockly.Logo.definitions_) {
    var def = Blockly.Logo.definitions_[name];
    if (def.match(/^(from\s+\S+\s+)?import\s+\S+/)) {
      imports.push(def);
    } else {
      definitions.push(def);
    }
  }
  // Clean up temporary data.
  delete Blockly.Logo.definitions_;
  delete Blockly.Logo.functionNames_;
  Blockly.Logo.variableDB_.reset();
  var allDefs = imports.join('\n') + '\n\n' + definitions.join('\n\n');
  return allDefs.replace(/\n\n+/g, '\n\n').replace(/\n*$/, '\n\n\n') + code;
};

/**
 * Naked values are top-level blocks with outputs that aren't plugged into
 * anything.
 * @param {string} line Line of generated code.
 * @return {string} Legal line of code.
 */
Blockly.Logo.scrubNakedValue = function(line) {
  return line + '\n';
};

/**
 * Encode a string as a properly escaped Python string, complete with quotes.
 * @param {string} string Text to encode.
 * @return {string} Python string.
 * @private
 */
Blockly.Logo.quote_ = function(string) {
  // Can't use goog.string.quote since % must also be escaped.
  string = string.replace(/\\/g, '\\\\')
                 .replace(/\n/g, '\\\n')
                 .replace(/\%/g, '\\%');

  // Follow the CPython behaviour of repr() for a non-byte string.
  var quote = '\'';
  if (string.indexOf('\'') !== -1) {
    if (string.indexOf('"') === -1) {
      quote = '"';
    } else {
      string = string.replace(/'/g, '\\\'');
    }
  };
  return quote + string + quote;
};

/**
 * Common tasks for generating Python from blocks.
 * Handles comments for the specified block and any connected value blocks.
 * Calls any statements following this block.
 * @param {!Blockly.Block} block The current block.
 * @param {string} code The Python code created for this block.
 * @return {string} Python code with comments and subsequent blocks added.
 * @private
 */
Blockly.Logo.scrub_ = function(block, code) {
  var commentCode = '';
  // Only collect comments for blocks that aren't inline.
  if (!block.outputConnection || !block.outputConnection.targetConnection) {
    // Collect comment for this block.
    var comment = block.getCommentText();
    comment = Blockly.utils.wrap(comment, Blockly.Logo.COMMENT_WRAP - 3);
    if (comment) {
      if (block.getProcedureDef) {
        // Use a comment block for function comments.
        commentCode += '"""' + comment + '\n"""\n';
      } else {
        commentCode += Blockly.Logo.prefixLines(comment + '\n', '# ');
      }
    }
    // Collect comments for all value arguments.
    // Don't collect comments for nested statements.
    for (var i = 0; i < block.inputList.length; i++) {
      if (block.inputList[i].type == Blockly.INPUT_VALUE) {
        var childBlock = block.inputList[i].connection.targetBlock();
        if (childBlock) {
          var comment = Blockly.Logo.allNestedComments(childBlock);
          if (comment) {
            commentCode += Blockly.Logo.prefixLines(comment, '# ');
          }
        }
      }
    }
  }
  var nextBlock = block.nextConnection && block.nextConnection.targetBlock();
  var nextCode = Blockly.Logo.blockToCode(nextBlock);
  return commentCode + code + nextCode;
};

/**
 * Gets a property and adjusts the value, taking into account indexing, and
 * casts to an integer.
 * @param {!Blockly.Block} block The block.
 * @param {string} atId The property ID of the element to get.
 * @param {number=} opt_delta Value to add.
 * @param {boolean=} opt_negate Whether to negate the value.
 * @return {string|number}
 */
Blockly.Logo.getAdjustedInt = function(block, atId, opt_delta, opt_negate) {
  var delta = opt_delta || 0;
  if (block.workspace.options.oneBasedIndex) {
    delta--;
  }
  var defaultAtIndex = block.workspace.options.oneBasedIndex ? '1' : '0';
  var atOrder = delta ? Blockly.Logo.ORDER_ADDITIVE :
      Blockly.Logo.ORDER_NONE;
  var at = Blockly.Logo.valueToCode(block, atId, atOrder) || defaultAtIndex;

  if (Blockly.isNumber(at)) {
    // If the index is a naked number, adjust it right now.
    at = parseInt(at, 10) + delta;
    if (opt_negate) {
      at = -at;
    }
  } else {
    // If the index is dynamic, adjust it in code.
    if (delta > 0) {
      at = 'int(' + at + ' + ' + delta + ')';
    } else if (delta < 0) {
      at = 'int(' + at + ' - ' + -delta + ')';
    } else {
      at = 'int(' + at + ')';
    }
    if (opt_negate) {
      at = '-' + at;
    }
  }
  return at;
};
