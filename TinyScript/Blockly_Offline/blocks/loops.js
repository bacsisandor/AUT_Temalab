Blockly.Blocks['while'] = {
    init: function () {
        this.appendValueInput("condition")
        .setCheck("Boolean")
        .appendField("Repeat While");
        this.appendStatementInput("statements")
        .setCheck(null);
        this.setInputsInline(false);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['do_while'] = {
    init: function () {
        this.appendStatementInput("statements")
        .setCheck(null)
        .appendField("Do");
        this.appendValueInput("condition")
        .setCheck("Boolean")
        .appendField("While");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['count'] = {
    init: function () {
        this.appendDummyInput()
        .appendField("Count from")
        .appendField(new Blockly.FieldVariable("variable"), "variable")
        .appendField("=")
        .appendField(new Blockly.FieldNumber(0), "initial")
        .appendField("to")
        .appendField(new Blockly.FieldNumber(0), "until")
        .appendField("")
        .appendField(new Blockly.FieldDropdown([["increment", "increment"], ["decrement", "decrement"]]), "direction")
        .appendField("variable by")
        .appendField(new Blockly.FieldNumber(0), "inc_value");
        this.appendStatementInput("core")
        .setCheck(null);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(135);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['for'] = {
    init: function () {
        this.appendDummyInput()
        .appendField("Loop from")
        .appendField(new Blockly.FieldVariable("i"), "variable1")
        .appendField("=");
        this.appendValueInput("init")
        .setCheck("Number");
        this.appendDummyInput()
        .appendField("until");
        this.appendValueInput("condition")
        .setCheck("Boolean");
        this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["increment", "increment"], ["decrement", "decrement"]]), "direction")
        .appendField(new Blockly.FieldVariable("i"), "variable2")
        .appendField("by");
        this.appendValueInput("inc_number")
        .setCheck("Number");
        this.appendStatementInput("core")
        .setCheck(null);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(135);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};
