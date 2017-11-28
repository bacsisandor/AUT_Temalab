Blockly.TinyScript['create_var'] = function (block) {
    var variable_var = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
    var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var code = 'var ' + variable_var + ' = ' + value_name + ';\n';
    return code;
};

Blockly.TinyScript['create_variabletype'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var variable_var = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
    var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var code;
    if (!value_name) {
        code = dropdown_type + ' ' + variable_var + ';\n';
    } else
        code = dropdown_type + ' ' + variable_var + ' = ' + value_name + ';\n';
    return code;
};

Blockly.TinyScript['variables_get'] = function(block) {
  var code = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('VAR'),
      Blockly.Variables.NAME_TYPE);
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['variables_set'] = function(block) {
  var argument0 = Blockly.TinyScript.valueToCode(block, 'VALUE',
      Blockly.TinyScript.ORDER_ASSIGNMENT) || '0';
  var varName = Blockly.TinyScript.variableDB_.getName(
      block.getFieldValue('VAR'), Blockly.Variables.NAME_TYPE);
  return varName + ' = ' + argument0 + ';\n';
};
