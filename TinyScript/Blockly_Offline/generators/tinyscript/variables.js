/*
Create var - creates a variable
variable_name - name of the variable
value_value - initial value of the variable
*/
Blockly.TinyScript['create_var'] = function (block) {
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
    var value_value = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var code = 'var ' + variable_name + ' = ' + value_value + ';\n';
    return code;
};

/*
Create variable type - creates a variable with the given type
dropdown_type - type of the variable
varaible_name - name of the variable
value_value - initial value of the variable
*/
Blockly.TinyScript['create_variabletype'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
    var value_value = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var code;
    if (!value_value) {
        code = dropdown_type + ' ' + variable_name + ';\n';
    } else
        code = dropdown_type + ' ' + variable_name + ' = ' + value_value + ';\n';
    return code;
};

/*
Variables get - gets the value of the varaible
variable_name - name of the variable
*/
Blockly.TinyScript['variables_get'] = function(block) {
  var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('VAR'),
      Blockly.Variables.NAME_TYPE);
  return [variable_name, Blockly.TinyScript.ORDER_ATOMIC];
};

/*
Variables set - sets the variable's value to the given value
varaible_name - name of the variable
value_value - value to set the variable
*/
Blockly.TinyScript['variables_set'] = function(block) {
  var value_value = Blockly.TinyScript.valueToCode(block, 'VALUE',
      Blockly.TinyScript.ORDER_ASSIGNMENT) || '0';
  var variable_name = Blockly.TinyScript.variableDB_.getName(
      block.getFieldValue('VAR'), Blockly.Variables.NAME_TYPE);
  return variable_name + ' = ' + value_value + ';\n';
};
