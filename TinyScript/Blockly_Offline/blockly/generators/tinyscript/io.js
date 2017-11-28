Blockly.TinyScript['read_input'] = function (block) {
    var value_name = Blockly.TinyScript.valueToCode(block, 'NAME', Blockly.TinyScript.ORDER_ATOMIC);
    var code = 'read(' + value_name + ');\n';
    return code;
};
