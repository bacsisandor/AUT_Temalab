Blockly.TinyScript['text'] = function (block) {
    var code = Blockly.TinyScript.quote_(block.getFieldValue('TEXT'));
    return [code, Blockly.TinyScript.ORDER_ATOMIC];
};

Blockly.TinyScript['text_print'] = function (block) {
    var msg = Blockly.TinyScript.valueToCode(block, 'TEXT',
            Blockly.TinyScript.ORDER_NONE) || '\'\'';
    var code = 'print(' + msg + ');\n';
    return code;
};

Blockly.TinyScript['text_join'] = function (block) {
    var elements = new Array(block.itemCount_);
    for (var i = 0; i < block.itemCount_; i++) {
        elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
                Blockly.TinyScript.ORDER_COMMA) || '\'\'';
    }
    var code = elements.join('+');
    return [code, Blockly.TinyScript.ORDER_FUNCTION_CALL];
};
