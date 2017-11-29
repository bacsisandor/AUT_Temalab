/*
Read input - reads from the input to a variable
variable_name - name of the variable
*/
Blockly.TinyScript['read_input'] = function (block) {
    var variable_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var code = 'read(' + variable_name + ');\n';
    return code;
};
