/*
Create array - creates an array with the given type and size
dropdown_type - type of the array elements
variable_name - name of the array
value_size - size of the array
*/
Blockly.TinyScript['create_array'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('name'), Blockly.Variables.NAME_TYPE);
    var value_size = Blockly.TinyScript.valueToCode(block, 'size', Blockly.TinyScript.ORDER_ATOMIC);
    var code = dropdown_type + ' ' + variable_name + '[' + value_size + ']' + ';\n';
    return code;
};

/*
Set array - sets a specific element of the array to the given value
variable_name - name of the array
value_index - index of an element
value_value - change the element to this value
*/
Blockly.TinyScript['set_array'] = function (block) {
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('item'), Blockly.Variables.NAME_TYPE);
    var value_index = Blockly.TinyScript.valueToCode(block, 'index', Blockly.TinyScript.ORDER_ATOMIC);
    var value_value = Blockly.TinyScript.valueToCode(block, 'value', Blockly.TinyScript.ORDER_ATOMIC);
    var code = variable_name + '[' + value_index + ']' + ' = ' + value_value + ';\n';
    return code;
};

/*
Get array - gets a specific element of the array
variable_name - name of the array
value_index - index of an element
*/
Blockly.TinyScript['get_array'] = function (block) {
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('variable'), Blockly.Variables.NAME_TYPE);
    var value_index = Blockly.TinyScript.valueToCode(block, 'index', Blockly.TinyScript.ORDER_ATOMIC);
    var code = variable_name + '[' + value_index + ']';
    return [code, Blockly.TinyScript.ORDER_NONE];
};

/*
Init array - creates an array from given values
dropdown_type - type of the array elements
variable_name - name of the array
elements - values of the array elements
*/
Blockly.TinyScript['init_array'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var variable_name = Blockly.TinyScript.variableDB_.getName(block.getFieldValue('name'), Blockly.Variables.NAME_TYPE);
    var elements = new Array(block.itemCount_);
    for (var i = 0; i < block.itemCount_; i++) {
        elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
                Blockly.TinyScript.ORDER_COMMA) || 'null';
    }
    var code = dropdown_type + ' ' + variable_name + '[] = ' + '{' + elements.join(', ') + '};\n';
    return [code, Blockly.TinyScript.ORDER_ATOMIC];
};
