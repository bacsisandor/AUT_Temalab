Blockly.Blocks['create_var'] = {
    init: function () {
        this.appendDummyInput()
        .appendField("Create variable")
        .appendField(new Blockly.FieldVariable("myVariable"), "var");
        this.appendValueInput("NAME")
        .setCheck(["Boolean", "Number", "String"])
        .appendField("Init value");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(330);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['create_variabletype'] = {
    init: function () {
        this.appendDummyInput()
        .appendField("Create variable")
        .appendField(new Blockly.FieldDropdown([["Integer", "int"], ["Boolean", "bool"], ["String", "string"]]), "type")
        .appendField(new Blockly.FieldVariable("myVariable"), "var");
        this.appendValueInput("NAME")
        .setCheck(["Boolean", "Number", "String"])
        .appendField("Init value");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(330);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};
