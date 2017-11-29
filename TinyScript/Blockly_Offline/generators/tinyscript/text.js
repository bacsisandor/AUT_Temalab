/*
Text -  creates text
value_text - text
*/
Blockly.TinyScript['text'] = function (block) {
    var value_text = Blockly.TinyScript.quote_(block.getFieldValue('TEXT'));
    return [value_text, Blockly.TinyScript.ORDER_ATOMIC];
};

/*
Text print - prints the given value
value_text - the value to print
*/
Blockly.TinyScript['text_print'] = function (block) {
    var value_text = Blockly.TinyScript.valueToCode(block, 'TEXT',
            Blockly.TinyScript.ORDER_NONE) || '\'\'';
    var code = 'print(' + value_text + ');\n';
    return code;
};

/*
Text join - joins the given values
elements - values to join
*/
Blockly.TinyScript['text_join'] = function (block) {
    var elements = new Array(block.itemCount_);
    for (var i = 0; i < block.itemCount_; i++) {
        elements[i] = Blockly.TinyScript.valueToCode(block, 'ADD' + i,
                Blockly.TinyScript.ORDER_COMMA) || '\'\'';
    }
    var code = elements.join('+');
    return [code, Blockly.TinyScript.ORDER_FUNCTION_CALL];
};
