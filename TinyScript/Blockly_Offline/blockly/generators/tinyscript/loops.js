Blockly.TinyScript['while'] = function (block) {
    var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
    var statements_statements = Blockly.TinyScript.statementToCode(block, 'statements');
    var code = 'while (' + value_condition + '){\n' + statements_statements + '}\n';
    return code;
};

Blockly.TinyScript['do_while'] = function (block) {
    var statements_statements = Blockly.TinyScript.statementToCode(block, 'statements');
    var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
    var code = 'do{\n' + statements_statements + '} while(' + value_condition + ');\n';
    return code;
};

Blockly.TinyScript['count'] = function (block) {
    var variable_variable = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable'), Blockly.Variables.NAME_TYPE);
    var number_initial = block.getFieldValue('initial');
    var number_until = block.getFieldValue('until');
    var dropdown_direction = block.getFieldValue('direction');
    var number_inc_value = block.getFieldValue('inc_value');
    var statements_core = Blockly.TinyScript.statementToCode(block, 'core');
    var inc;
    if (dropdown_direction == 'increment')
        inc = '+';
    else
        inc = '-';
    var code;
    if (number_inc_value != 1)
        code = 'count(from ' + variable_variable + '=' + number_initial + '; to ' + variable_variable + '=' + number_until + '; ' + variable_variable + inc + '=' + number_inc_value + '){\n' + statements_core + '}\n';
    else
        code = 'count(from ' + variable_variable + '=' + number_initial + '; to ' + variable_variable + '=' + number_until + '; ' + variable_variable + inc + inc + '){\n' + statements_core + '}\n';
    return code;
};

Blockly.TinyScript['for'] = function (block) {
    var variable_variable1 = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable1'), Blockly.Variables.NAME_TYPE);
    var value_init = Blockly.TinyScript.valueToCode(block, 'init', Blockly.TinyScript.ORDER_ATOMIC);
    var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
    var dropdown_direction = block.getFieldValue('direction');
    var variable_variable2 = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable2'), Blockly.Variables.NAME_TYPE);
    var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var statements_core = Blockly.TinyScript.statementToCode(block, 'core');
    var inc;
    if (dropdown_direction == 'increment')
        inc = '+';
    else
        inc = '-';
    if (value_name != 1)
        code = 'for(' + variable_variable1 + '=' + value_init + '; ' + value_condition + '; ' + variable_variable2 + inc + '=' + value_name + '){\n' + statements_core + '}\n';
    else
        var code = 'for(' + variable_variable1 + '=' + value_init + '; ' + value_condition + '; ' + variable_variable2 + inc + inc + '){\n' + statements_core + '}\n';
    return code;
};
