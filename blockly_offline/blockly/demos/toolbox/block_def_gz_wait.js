Blockly.Blocks['waitforbuttonpressed'] = {
    init: function () {
        this.appendValueInput("buttonVariable")
            .setCheck("Button")
            .appendField("wait press on buttonvar:");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(195);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['waitforlight'] = {
    init: function () {
        this.appendValueInput("lightSensor")
            .setCheck("LightSensor")
            .appendField("wait for light sensorvar:");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(195);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['waitfordark'] = {
    init: function () {
        this.appendValueInput("lightSensor")
            .setCheck("LightSensor")
            .appendField("wait for dark sensorvar:");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(195);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};