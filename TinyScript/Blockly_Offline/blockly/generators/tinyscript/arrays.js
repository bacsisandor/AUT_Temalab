Blockly.TinyScript['create_array'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('name'), Blockly.Variables.NAME_TYPE);
    var value_size = Blockly.TinyScript.valueToCode(block, 'size', Blockly.TinyScript.ORDER_ATOMIC);
    var code = dropdown_type + ' ' + variable_name + '[' + value_size + ']' + ';\n';
    return code;
};

Blockly.TinyScript['set_array'] = function (block) {
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('item'), Blockly.Variables.NAME_TYPE);
    var value_index = Blockly.TinyScript.valueToCode(block, 'index', Blockly.TinyScript.ORDER_ATOMIC);
    var value_value = Blockly.TinyScript.valueToCode(block, 'value', Blockly.TinyScript.ORDER_ATOMIC);
    var code = variable_name + '[' + value_index + ']' + ' = ' + value_value + ';\n';
    return code;
};

Blockly.TinyScript['get_array'] = function (block) {
    var variable_variable = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable'), Blockly.Variables.NAME_TYPE);
    var value_index = Blockly.TinyScript.valueToCode(block, 'index', Blockly.TinyScript.ORDER_ATOMIC);
    var code = variable_variable + '[' + value_index + ']';
    return [code, Blockly.TinyScript.ORDER_NONE];
};

Blockly.TinyScript['init_array'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('name'), Blockly.Variables.NAME_TYPE);
    var elements = new Array(block.itemCount_);
    for (var i = 0; i < block.itemCount_; i++) {
        elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
                Blockly.TinyScript.ORDER_COMMA) || 'null';
    }
    var code = dropdown_type + ' ' + variable_name + '[] = ' + '{' + elements.join(', ') + '};';
    return [code, Blockly.TinyScript.ORDER_ATOMIC];
};
