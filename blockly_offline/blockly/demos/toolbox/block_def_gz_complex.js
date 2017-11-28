Blockly.Blocks['vacuumcleanerhandler'] = {
    init: function () {
        this.appendValueInput("vacuumCleanerVariable")
            .setCheck(null)
            .appendField("vacuum cleaner handler method");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(30);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['robberdetectorhandler'] = {
    init: function () {
        this.appendValueInput("robberDetectorVariable")
            .setCheck(null)
            .appendField("Robboer detector var:");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(30);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['petfeederhandler'] = {
    init: function () {
        this.appendValueInput("petFeederVariable")
            .setCheck(null)
            .appendField("pet feeder handler method");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(30);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};