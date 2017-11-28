Blockly.Blocks['lightsensorvalue'] = {
    init: function () {
        this.appendValueInput("lightSensor")
            .appendField("LightSensor value var:");
        this.setInputsInline(true);
        this.setOutput(true, "Number");
        this.setColour(300);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['setvalueoffield'] = {
    init: function () {
        this.appendValueInput("variable")
            .setCheck(null)
            .appendField("set var value");
        this.appendValueInput("value")
            .setCheck(null)
            .appendField("to");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(300);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['getvalues'] = {
    init: function () {
        this.appendValueInput("variable")
            .setCheck(null)
            .appendField("get values of var");
        this.setOutput(true, null);
        this.setInputsInline(true);
        this.setColour(300);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['getcputempvalue'] = {
    init: function () {
        this.appendValueInput("cpuTempVar")
            .setCheck(null)
            .appendField("Cpu temp value on var");
        this.setInputsInline(true);
        this.setOutput(true, null);
        this.setColour(300);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['cputemperaturesinglevalue'] = {
    init: function () {
        this.appendValueInput("cputempvariable")
            .setCheck(null)
            .appendField("cpu temp value on var:");
        this.setOutput(true, null);
        this.setColour(300);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['distancesensorvalue'] = {
    init: function () {
        this.appendValueInput("distanceSensor")
            .setCheck("DistanceSensor")
            .appendField("DistanceSensor value var:");
        this.setInputsInline(true);
        this.setOutput(true, null);
        this.setColour(300);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};