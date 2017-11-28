Blockly.TinyScript['controls_if'] = function (block) {
    var n = 0;
    var code = '',
    branchCode,
    conditionCode;
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

Blockly.TinyScript['logic_compare'] = function (block) {
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

Blockly.TinyScript['logic_operation'] = function (block) {
    var operator = (block.getFieldValue('OP') == 'AND') ? '&&' : '||';
    var order = (operator == '&&') ? Blockly.TinyScript.ORDER_LOGICAL_AND :
    Blockly.TinyScript.ORDER_LOGICAL_OR;
    var argument0 = Blockly.TinyScript.valueToCode(block, 'A', order);
    var argument1 = Blockly.TinyScript.valueToCode(block, 'B', order);
    if (!argument0 && !argument1) {
        argument0 = 'false';
        argument1 = 'false';
    } else {
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

Blockly.TinyScript['logic_negate'] = function (block) {
    var order = Blockly.TinyScript.ORDER_LOGICAL_NOT;
    var argument0 = Blockly.TinyScript.valueToCode(block, 'BOOL', order) ||
        'true';
    var code = '!' + argument0;
    return [code, order];
};

Blockly.TinyScript['logic_boolean'] = function (block) {
    var code = (block.getFieldValue('BOOL') == 'TRUE') ? 'true' : 'false';
    return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['logic_null'] = function (block) {
    return ['null', Blockly.TinyScript.ORDER_ATOMIC];
};
