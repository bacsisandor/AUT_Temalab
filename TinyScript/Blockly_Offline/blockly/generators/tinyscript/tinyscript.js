Blockly.TinyScript['while'] = function(block) {
  var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
  var statements_statements = Blockly.TinyScript.statementToCode(block, 'statements');
  // TODO: Assemble TinyScript into code variable.
  var code = '...\n';
  return code;
};

Blockly.TinyScript['do_while'] = function(block) {
  var statements_statements = Blockly.TinyScript.statementToCode(block, 'statements');
  var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
  // TODO: Assemble TinyScript into code variable.
  var code = '...\n';
  return code;
};

Blockly.TinyScript['create_var'] = function(block) {
  var variable_var = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
  var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
  // TODO: Assemble TinyScript into code variable.
  var code = '...\n';
  return code;
};

Blockly.TinyScript['create_variabletype'] = function(block) {
  var dropdown_type = block.getFieldValue('type');
  var variable_var = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('var'), Blockly.Variables.NAME_TYPE);
  var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
  // TODO: Assemble TinyScript into code variable.
  var code = '...\n';
  return code;
};

Blockly.TinyScript['abs'] = function(block) {
  var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
  // TODO: Assemble TinyScript into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.TinyScript.ORDER_NONE];
};

Blockly.TinyScript['math_number'] = function(block) {
  // Numeric value.
  var code = parseFloat(block.getFieldValue('NUM'));
  return [code, Blockly.TinyScript.ORDER_ATOMIC];
};