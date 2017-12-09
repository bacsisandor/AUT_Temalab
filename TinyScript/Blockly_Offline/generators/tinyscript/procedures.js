Blockly.TinyScript['procedures_defreturn'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var funcName = Blockly.TinyScript.variableDB_.getName(
            block.getFieldValue('NAME'), Blockly.Procedures.NAME_TYPE);
    var branch = Blockly.TinyScript.statementToCode(block, 'STACK');
    if (Blockly.TinyScript.STATEMENT_PREFIX) {
        var id = block.id.replace(/\$/g, '$$$$'); // Issue 251.
        branch = Blockly.TinyScript.prefixLines(
                Blockly.TinyScript.STATEMENT_PREFIX.replace(/%1/g,
                    '\'' + id + '\''), Blockly.TinyScript.INDENT) + branch;
    }
    if (Blockly.TinyScript.INFINITE_LOOP_TRAP) {
        branch = Blockly.TinyScript.INFINITE_LOOP_TRAP.replace(/%1/g,
                '\'' + block.id + '\'') + branch;
    }
    var returnValue = Blockly.TinyScript.valueToCode(block, 'RETURN',
            Blockly.TinyScript.ORDER_NONE) || '';
    if (returnValue) {
        returnValue = '  return ' + returnValue + ';\n';
    }
    var args = [];
    for (var i = 0; i < block.arguments_.length; i++) {
        args[i] = Blockly.TinyScript.variableDB_.getName(block.arguments_[i],
                Blockly.Variables.NAME_TYPE);
    }
    if (dropdown_type == null) {
        dropdown_type = 'void';
    }
    var elements = new Array(block.arguments_.length);
    for (var i = 0; i < block.arguments_.length; i++) {
        elements[i] = block.getFieldValue('type' + i);
    }

    var code = dropdown_type + ' ' + funcName + '(';
    if (block.arguments_.length != 0) {
        for (var i = 0; i < block.arguments_.length - 1; i++) {
            code = code + elements[i] + ' ' + args[i] + ',';
        }
        for (var i = block.arguments_.length - 1; i < block.arguments_.length; i++) {
            code = code + elements[i] + ' ' + args[i];
        }
    }

    code = code + ') {\n' + branch + returnValue + '}';
    code = Blockly.TinyScript.scrub_(block, code);
    Blockly.TinyScript.definitions_['%' + funcName] = code;
    return null;
};

Blockly.TinyScript['procedures_defnoreturn'] =
    Blockly.TinyScript['procedures_defreturn'];

Blockly.TinyScript['procedures_callreturn'] = function (block) {
    var funcName = Blockly.TinyScript.variableDB_.getName(
            block.getFieldValue('NAME'), Blockly.Procedures.NAME_TYPE);
    var args = [];
    for (var i = 0; i < block.arguments_.length; i++) {
        args[i] = Blockly.TinyScript.valueToCode(block, 'ARG' + i,
                Blockly.TinyScript.ORDER_COMMA) || 'null';
    }
    var code = funcName + '(' + args.join(', ') + ')';
    return [code, Blockly.TinyScript.ORDER_FUNCTION_CALL];
};

Blockly.TinyScript['procedures_callnoreturn'] = function (block) {
    var funcName = Blockly.TinyScript.variableDB_.getName(
            block.getFieldValue('NAME'), Blockly.Procedures.NAME_TYPE);
    var args = [];
    for (var i = 0; i < block.arguments_.length; i++) {
        args[i] = Blockly.TinyScript.valueToCode(block, 'ARG' + i,
                Blockly.TinyScript.ORDER_COMMA) || 'null';
    }
    var code = funcName + '(' + args.join(', ') + ');\n';
    return code;
};

Blockly.TinyScript['procedures_ifreturn'] = function (block) {
    var condition = Blockly.TinyScript.valueToCode(block, 'CONDITION',
            Blockly.TinyScript.ORDER_NONE) || 'false';
    var code = 'if (' + condition + ') {\n';
    if (block.hasReturnValue_) {
        var value = Blockly.TinyScript.valueToCode(block, 'VALUE',
                Blockly.TinyScript.ORDER_NONE) || 'null';
        code += '  return ' + value + ';\n';
    } else {
        code += '  return;\n';
    }
    code += '}\n';
    return code;
};
