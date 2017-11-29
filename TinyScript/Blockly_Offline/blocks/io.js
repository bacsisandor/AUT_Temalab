Blockly.Blocks['read_input'] = {
    init: function () {
        this.appendDummyInput()
        .appendField("Read to");
        this.appendValueInput("NAME")
        .setCheck(null);
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(195);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};
