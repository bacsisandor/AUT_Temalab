Blockly.Blocks['setrgbledcolortovar'] = {
    init: function () {
        this.appendValueInput("rgbled")
            .setCheck(null)
            .appendField("RGBLED on var");
        this.appendDummyInput()
            .appendField("color")
            .appendField(new Blockly.FieldDropdown([["red", "red"], ["green", "green"], ["blue", "blue"]]), "color");
        this.appendValueInput("var")
            .setCheck("Number")
            .appendField("brightness");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(270);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['setpwmledtovar'] = {
    init: function () {
        this.appendValueInput("pwmled")
            .setCheck(null)
            .appendField("PWMLED on var");
        this.appendValueInput("var")
            .setCheck("Number")
            .appendField("to brightness");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(270);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};