Blockly.TinyScript['while'] = function(block) {
  var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
  var statements_statements = Blockly.TinyScript.statementToCode(block, 'statements');
  var code = 'while ('+value_condition+'){\n'+statements_statements+'}\n';
  return code;
};

Blockly.TinyScript['do_while'] = function(block) {
  var statements_statements = Blockly.TinyScript.statementToCode(block, 'statements');
  var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
  var code = 'do{\n'+statements_statements+'} while('+value_condition+');\n';
  return code;
};

Blockly.TinyScript['create_var'] = function(block) {
  var variable_var = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
  var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
  var code = 'var '+variable_var+' = '+value_name+';\n';
  return code;
};

Blockly.TinyScript['create_variabletype'] = function(block) {
  var dropdown_type = block.getFieldValue('type');
  var variable_var = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
  var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
  var code;
  //"var name;" or "var name = asd;"
  if(!value_name){code=dropdown_type+' '+variable_var+';\n';}
  else code=dropdown_type+' '+variable_var+' = '+value_name+';\n';
  return code;
};

Blockly.TinyScript['abs'] = function(block) {
  var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
  var code = 'abs('+value_name+')';
  return [code, Blockly.TinyScript.ORDER_NONE];
};

Blockly.TinyScript['math_number'] = function(block) {
  var code = parseFloat(block.getFieldValue('NUM'));
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['controls_if'] = function(block) {
  // If/elseif/else condition.
  var n = 0;
  var code = '', branchCode, conditionCode;
  do {
    conditionCode = Blockly.TinyScript.valueToCode(block, 'IF' + n,
      Blockly.TinyScript.ORDER_NONE) || 'false';
    branchCode = Blockly.TinyScript.statementToCode(block, 'DO' + n);
    code += (n > 0 ? ' else ' : '') +
        'if (' + conditionCode + ') {\n' + branchCode + '}';

    ++n;
  } while (block.getInput('IF' + n));

  if (block.getInput('ELSE')) {
    branchCode = Blockly.TinyScript.statementToCode(block, 'ELSE');
    code += ' else {\n' + branchCode + '}';
  }
  return code + '\n';
};

Blockly.TinyScript['controls_ifelse'] = Blockly.TinyScript['controls_if'];

Blockly.TinyScript['logic_compare'] = function(block) {
  // Comparison operator.
  var OPERATORS = {
    'EQ': '==',
    'NEQ': '!=',
    'LT': '<',
    'LTE': '<=',
    'GT': '>',
    'GTE': '>='
  };
  var operator = OPERATORS[block.getFieldValue('OP')];
  var order = (operator == '==' || operator == '!=') ?
      Blockly.TinyScript.ORDER_EQUALITY : Blockly.TinyScript.ORDER_RELATIONAL;
  var argument0 = Blockly.TinyScript.valueToCode(block, 'A', order) || '0';
  var argument1 = Blockly.TinyScript.valueToCode(block, 'B', order) || '0';
  var code = argument0 + ' ' + operator + ' ' + argument1;
  return [code, order];
};

Blockly.TinyScript['logic_operation'] = function(block) {
  // Operations 'and', 'or'.
  var operator = (block.getFieldValue('OP') == 'AND') ? '&&' : '||';
  var order = (operator == '&&') ? Blockly.TinyScript.ORDER_LOGICAL_AND :
      Blockly.TinyScript.ORDER_LOGICAL_OR;
  var argument0 = Blockly.TinyScript.valueToCode(block, 'A', order);
  var argument1 = Blockly.TinyScript.valueToCode(block, 'B', order);
  if (!argument0 && !argument1) {
    // If there are no arguments, then the return value is false.
    argument0 = 'false';
    argument1 = 'false';
  } else {
    // Single missing arguments have no effect on the return value.
    var defaultArgument = (operator == '&&') ? 'true' : 'false';
    if (!argument0) {
      argument0 = defaultArgument;
    }
    if (!argument1) {
      argument1 = defaultArgument;
    }
  }
  var code = argument0 + ' ' + operator + ' ' + argument1;
  return [code, order];
};

Blockly.TinyScript['logic_negate'] = function(block) {
  // Negation.
  var order = Blockly.TinyScript.ORDER_LOGICAL_NOT;
  var argument0 = Blockly.TinyScript.valueToCode(block, 'BOOL', order) ||
      'true';
  var code = '!' + argument0;
  return [code, order];
};

Blockly.TinyScript['logic_boolean'] = function(block) {
  // Boolean values true and false.
  var code = (block.getFieldValue('BOOL') == 'TRUE') ? 'true' : 'false';
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['logic_null'] = function(block) {
  // Null data type.
  return ['null', Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['controls_for'] = function(block) {
  // For loop.
  var variable0 = Blockly.TinyScript.variableDB_.getName(
      block.getFieldValue('VAR'), Blockly.Variables.NAME_TYPE);
  var argument0 = Blockly.TinyScript.valueToCode(block, 'FROM',
      Blockly.TinyScript.ORDER_ASSIGNMENT) || '0';
  var argument1 = Blockly.TinyScript.valueToCode(block, 'TO',
      Blockly.TinyScript.ORDER_ASSIGNMENT) || '0';
  var increment = Blockly.TinyScript.valueToCode(block, 'BY',
      Blockly.TinyScript.ORDER_ASSIGNMENT) || '1';
  var branch = Blockly.TinyScript.statementToCode(block, 'DO');
  branch = Blockly.TinyScript.addLoopTrap(branch, block.id);
  var code;
  if (Blockly.isNumber(argument0) && Blockly.isNumber(argument1) &&
      Blockly.isNumber(increment)) {
    // All arguments are simple numbers.
    var up = parseFloat(argument0) <= parseFloat(argument1);
    code = 'for (' + variable0 + ' = ' + argument0 + '; ' +
        variable0 + (up ? ' <= ' : ' >= ') + argument1 + '; ' +
        variable0;
    var step = Math.abs(parseFloat(increment));
    if (step == 1) {
      code += up ? '++' : '--';
    } else {
      code += (up ? ' += ' : ' -= ') + step;
    }
    code += ') {\n' + branch + '}\n';
  } else {
    code = '';
    // Cache non-trivial values to variables to prevent repeated look-ups.
    var startVar = argument0;
    if (!argument0.match(/^\w+$/) && !Blockly.isNumber(argument0)) {
      startVar = Blockly.TinyScript.variableDB_.getDistinctName(
          variable0 + '_start', Blockly.Variables.NAME_TYPE);
      code += 'var ' + startVar + ' = ' + argument0 + ';\n';
    }
    var endVar = argument1;
    if (!argument1.match(/^\w+$/) && !Blockly.isNumber(argument1)) {
      var endVar = Blockly.TinyScript.variableDB_.getDistinctName(
          variable0 + '_end', Blockly.Variables.NAME_TYPE);
      code += 'var ' + endVar + ' = ' + argument1 + ';\n';
    }
    // Determine loop direction at start, in case one of the bounds
    // changes during loop execution.
    var incVar = Blockly.TinyScript.variableDB_.getDistinctName(
        variable0 + '_inc', Blockly.Variables.NAME_TYPE);
    code += 'var ' + incVar + ' = ';
    if (Blockly.isNumber(increment)) {
      code += Math.abs(increment) + ';\n';
    } else {
      code += 'Math.abs(' + increment + ');\n';
    }
    code += 'if (' + startVar + ' > ' + endVar + ') {\n';
    code += Blockly.TinyScript.INDENT + incVar + ' = -' + incVar + ';\n';
    code += '}\n';
    code += 'for (' + variable0 + ' = ' + startVar + '; ' +
        incVar + ' >= 0 ? ' +
        variable0 + ' <= ' + endVar + ' : ' +
        variable0 + ' >= ' + endVar + '; ' +
        variable0 + ' += ' + incVar + ') {\n' +
        branch + '}\n';
  }
  return code;
};

//remove thet pow thing
Blockly.TinyScript['math_arithmetic'] = function(block) {
  // Basic arithmetic operators, and power.
  var OPERATORS = {
    'ADD': [' + ', Blockly.TinyScript.ORDER_ADDITION],
    'MINUS': [' - ', Blockly.TinyScript.ORDER_SUBTRACTION],
    'MULTIPLY': [' * ', Blockly.TinyScript.ORDER_MULTIPLICATION],
    'DIVIDE': [' / ', Blockly.TinyScript.ORDER_DIVISION],
    'POWER': [null, Blockly.TinyScript.ORDER_COMMA]  // Handle power separately.
  };
  var tuple = OPERATORS[block.getFieldValue('OP')];
  var operator = tuple[0];
  var order = tuple[1];
  var argument0 = Blockly.TinyScript.valueToCode(block, 'A', order) || '0';
  var argument1 = Blockly.TinyScript.valueToCode(block, 'B', order) || '0';
  argument1 = '('+argument1+')'; //see logo: - (-10) example why ive done this
  var code;
  // Power in JavaScript requires a special case since it has no operator.
  if (!operator) {
    code = 'Math.pow(' + argument0 + ', ' + argument1 + ')';
    return [code, Blockly.TinyScript.ORDER_FUNCTION_CALL];
  }
  code = argument0 + operator + argument1;
  return [code, order];
};

//default?
Blockly.TinyScript['variables_get'] = function(block) {
  // Variable getter.
  var code = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('VAR'),
      Blockly.Variables.NAME_TYPE);
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['variables_set'] = function(block) {
  // Variable setter.
  var argument0 = Blockly.TinyScript.valueToCode(block, 'VALUE',
      Blockly.TinyScript.ORDER_ASSIGNMENT) || '0';
  var varName = Blockly.TinyScript.variableDB_.getName(
      block.getFieldValue('VAR'), Blockly.Variables.NAME_TYPE);
  return varName + ' = ' + argument0 + ';\n';
};

//default?
Blockly.TinyScript['text'] = function(block) {
  // Text value.
  var code = Blockly.TinyScript.quote_(block.getFieldValue('TEXT'));
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['text_print'] = function(block) {
  var msg = Blockly.TinyScript.valueToCode(block, 'TEXT',
      Blockly.TinyScript.ORDER_NONE) || '\'\'';
  var code = 'print('+msg+');\n';
  return code;
};

Blockly.TinyScript['text_join'] = function(block) {
  // Create a string made up of any number of elements of any type.
      var elements = new Array(block.itemCount_);
      for (var i = 0; i < block.itemCount_; i++) {
        elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
            Blockly.TinyScript.ORDER_COMMA) || '\'\'';
      }
      var code = elements.join('+');
      return [code, Blockly.TinyScript.ORDER_FUNCTION_CALL];
};

Blockly.TinyScript['math_change'] = function(block) {
  // Add to a variable in place.
  var argument0 = Blockly.TinyScript.valueToCode(block, 'DELTA',
      Blockly.TinyScript.ORDER_ADDITION) || '0';
  var varName = Blockly.TinyScript.variableDB_.getName(
      block.getFieldValue('VAR'), Blockly.Variables.NAME_TYPE);
  switch(argument0){
	  case '1' : return varName + '++;';
	  case '-1' : return varName + '--;';
  }
  if(argument0>1){
	return varName + '+='+ argument0 +';';
  }
  else if(argument0<-1){
	return varName + '-='+ Math.abs(argument0) +';';
  }
};

Blockly.TinyScript['create_array'] = function(block) {
  var dropdown_type = block.getFieldValue('type');
  var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('name'), Blockly.Variables.NAME_TYPE);
  var value_size = Blockly.TinyScript.valueToCode(block, 'size', Blockly.TinyScript.ORDER_ATOMIC);
  var code = dropdown_type + ' ' + variable_name + '[' + value_size + ']' + ';\n';
  return code;
};

Blockly.TinyScript['set_array'] = function(block) {
  var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('item'), Blockly.Variables.NAME_TYPE);
  var value_index = Blockly.TinyScript.valueToCode(block, 'index', Blockly.TinyScript.ORDER_ATOMIC);
  var value_value = Blockly.TinyScript.valueToCode(block, 'value', Blockly.TinyScript.ORDER_ATOMIC);
  var code = variable_name + '[' + value_index + ']' + ' = ' + value_value +';\n';
  return code;
};

Blockly.TinyScript['get_array'] = function(block) {
  var variable_variable = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable'), Blockly.Variables.NAME_TYPE);
  var value_index = Blockly.TinyScript.valueToCode(block, 'index', Blockly.TinyScript.ORDER_ATOMIC);
  var code = variable_variable + '[' + value_index + ']';
  return [code, Blockly.TinyScript.ORDER_NONE];
};


Blockly.TinyScript['init_array'] = function(block) {
  var dropdown_type = block.getFieldValue('type');
  var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('name'), Blockly.Variables.NAME_TYPE);
  var elements = new Array(block.itemCount_);
  for (var i = 0; i < block.itemCount_; i++) {
    elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
        Blockly.TinyScript.ORDER_COMMA) || 'null';
  }
  var code = dropdown_type + ' ' + variable_name+ '[] = ' + '{' + elements.join(', ') + '}';
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
}; 


Blockly.TinyScript['maximum_select'] = function(block) {
  var selected = block.getFieldValue('select');
  var elements = new Array(block.itemCount_);
  for (var i = 0; i < block.itemCount_; i++) {
    elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
        Blockly.TinyScript.ORDER_COMMA) || 'null';
  }
  var code = selected + '(' + elements.join(', ') + ')';
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
}; 