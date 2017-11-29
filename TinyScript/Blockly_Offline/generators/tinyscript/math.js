/*
Abs - absolute value
value_name - name of the value
*/
Blockly.TinyScript['abs'] = function (block) {
    var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var code = 'abs(' + value_name + ')';
    return [code, Blockly.TinyScript.ORDER_NONE];
};

/*
Math number - number
*/
Blockly.TinyScript['math_number'] = function (block) {
    var code = parseFloat(block.getFieldValue('NUM'));
    return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

/*
Math arithmetic - math arithmetic with 2 operands;
                  supported operators: + - * /
argument0 - first operand
argument2 - second operand
*/
Blockly.TinyScript['math_arithmetic'] = function (block) {
    var OPERATORS = {
        'ADD': [' + ', Blockly.TinyScript.ORDER_ADDITION],
        'MINUS': [' - ', Blockly.TinyScript.ORDER_SUBTRACTION],
        'MULTIPLY': [' * ', Blockly.TinyScript.ORDER_MULTIPLICATION],
        'DIVIDE': [' / ', Blockly.TinyScript.ORDER_DIVISION],
        'POWER': [null, Blockly.TinyScript.ORDER_COMMA]
    };
    var tuple = OPERATORS[block.getFieldValue('OP')];
    var operator = tuple[0];
    var order = tuple[1];
    var argument0 = Blockly.TinyScript.valueToCode(block, 'A', order) || '0';
    var argument1 = Blockly.TinyScript.valueToCode(block, 'B', order) || '0';
    if (argument1 < 0)
        argument1 = '(' + argument1 + ')'; //see logo: - (-10) example why ive done this
    var code;
    // Power in TinyScript is not supported
    if (!operator) {
        code = 'Math.pow(' + argument0 + ', ' + argument1 + ')';
        return [code, Blockly.TinyScript.ORDER_FUNCTION_CALL];
    }
    code = argument0 + operator + argument1;
    return [code, order];
};

/*
Math change - changes the value of a variable
variable_name - name of the varaible
argument0 - change value
*/
Blockly.TinyScript['math_change'] = function (block) {
    var argument0 = Blockly.TinyScript.valueToCode(block, 'DELTA',
            Blockly.TinyScript.ORDER_ADDITION) || '0';
    var variable_name = Blockly.TinyScript.variableDB_.getName(
            block.getFieldValue('VAR'), Blockly.Variables.NAME_TYPE);
    switch (argument0) {
    case '1':
        return variable_name + '++;';
    case '-1':
        return variable_name + '--;';
    }
    if (argument0 > 1) {
        return variable_name + '+=' + argument0 + ';';
    } else if (argument0 < -1) {
        return variable + '-=' + Math.abs(argument0) + ';';
    }
};

/*
Maximum select - selects maximum (or minimum) value of the elements
selected - select maximum or minimum function
elements - elements
*/
Blockly.TinyScript['maximum_select'] = function (block) {
    var selected = block.getFieldValue('select');
    var elements = new Array(block.itemCount_);
    for (var i = 0; i < block.itemCount_; i++) {
        elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
                Blockly.TinyScript.ORDER_COMMA) || 'null';
    }
    var code = selected + '(' + elements.join(', ') + ')';
    return [code, Blockly.TinyScript.ORDER_ATOMIC];
};
