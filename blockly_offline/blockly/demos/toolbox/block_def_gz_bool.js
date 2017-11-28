Blockly.Blocks['isbuttonpressed'] = {
    init: function () {
        this.appendValueInput("button")
            .setCheck("Button")
            .appendField("is button pressed on var.");
        this.setInputsInline(true);
        this.setOutput(true, "Boolean");
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['istheremotion'] = {
    init: function () {
        this.appendValueInput("motionSensor")
            .setCheck("MotionSensor")
            .appendField("is motion on var.");
        this.setInputsInline(true);
        this.setOutput(true, "Boolean");
        this.setColour(120);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};