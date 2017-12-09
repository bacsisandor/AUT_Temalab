/*
While - pre-test (while) loop
value_condition - condition
statements - statements to loop
*/
Blockly.TinyScript['while'] = function (block) {
    var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
    var statements = Blockly.TinyScript.statementToCode(block, 'statements');
    var code = 'while (' + value_condition + '){\n' + statements + '}\n';
    return code;
};

/*
Do while - post-test (do while) loop
value_condition - condition
statements - statements to loop
*/
Blockly.TinyScript['do_while'] = function (block) {
    var statements = Blockly.TinyScript.statementToCode(block, 'statements');
    var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
    var code = 'do{\n' + statements + '} while(' + value_condition + ');\n';
    return code;
};

/*
Count - count loop
variable_name - name of the loop counter
number_initial - loop counter's initial value
number_until - loop counter's condition value
dropdown_direction - counting direction
number_increment - increment (or decrement) value
statements - statements to loop
*/
Blockly.TinyScript['count'] = function (block) {
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable'), Blockly.Variables.NAME_TYPE);
    var number_initial = block.getFieldValue('initial');
    var number_until = block.getFieldValue('until');
    var dropdown_direction = block.getFieldValue('direction');
    var number_increment = block.getFieldValue('inc_value');
    var statements = Blockly.TinyScript.statementToCode(block, 'core');
    var inc;
    if (dropdown_direction == 'increment')
        inc = '+';
    else
        inc = '-';
    var code;
    if (number_increment != 1)
        code = 'count(from ' + variable_name + '=' + number_initial +
               ';to ' + variable_name + '=' + number_until +
               '; ' + variable_name + inc + '=' + number_increment + '){\n' + statements + '}\n';
    else
        code = 'count(from ' + variable_name + '=' + number_initial +
               '; to ' + variable_name + '=' + number_until +
               '; ' + variable_name + inc + inc + '){\n' + statements + '}\n';
    return code;
};

/*
For - For loop
variable_name1 - name of the loop counter (in initialization)
value_initial - loop counter's value in initialization
value_condition - loop counter's condition value
variable_name2 - name of the loop counter (in afterthought)
dropdown_direction - counting direction
value_increment - increment (or decrement) value
statements - statements to loop
*/
Blockly.TinyScript['for'] = function (block) {
    var variable_name1 = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable1'), Blockly.Variables.NAME_TYPE);
    var value_initial = Blockly.TinyScript.valueToCode(block, 'init', Blockly.TinyScript.ORDER_ATOMIC);
    var value_condition = Blockly.TinyScript.valueToCode(block, 'condition', Blockly.TinyScript.ORDER_ATOMIC);
    var dropdown_direction = block.getFieldValue('direction');
    var variable_name2 = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable2'), Blockly.Variables.NAME_TYPE);
    var value_increment = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var statements = Blockly.TinyScript.statementToCode(block, 'core');
    var inc;
    if (dropdown_direction == 'increment')
        inc = '+';
    else
        inc = '-';
    if (value_increment != 1)
        code = 'for(' + variable_name1 + '=' + value_initial +
               '; ' + value_condition +
               '; ' + variable_name2 + inc + '=' + value_increment + '){\n' + statements + '}\n';
    else
        var code = 'for(' + variable_name1 + '=' + value_initial +
                   '; ' + value_condition +
                   '; ' + variable_name2 + inc + inc + '){\n' + statements + '}\n';
    return code;
};
