Blockly.Blocks['setled'] = {
    init: function () {
        this.appendValueInput("variable")
            .setCheck("LED")
            .appendField("set Led var:");
        this.appendDummyInput()
            .appendField("to")
            .appendField(new Blockly.FieldDropdown([["ON", "on()"], ["OFF", "off()"], ["BLINK 1 SEC", "blink()"]]), "whatToDo");
        this.setInputsInline(false);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(180);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['setpwmled'] = {
    init: function () {
        this.appendValueInput("variable")
            .setCheck("PWMLED")
            .appendField("set PWMLED var:");
        this.appendDummyInput()
            .appendField("to")
            .appendField(new Blockly.FieldNumber(0, 0, 1, 0.01), "pwmValue");
        this.setInputsInline(false);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(180);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['setrgbledcolor'] = {
    init: function () {
        this.appendValueInput("NAME")
            .setCheck("RGBLED")
            .appendField("set RGBLED on var.:");
        this.appendDummyInput()
            .appendField("red")
            .appendField(new Blockly.FieldDropdown([["0", "0"], ["1", "1"]]), "valueOfRed");
        this.appendDummyInput()
            .appendField("green")
            .appendField(new Blockly.FieldDropdown([["0", "0"], ["1", "1"]]), "valueOfGreen");
        this.appendDummyInput()
            .appendField("blue")
            .appendField(new Blockly.FieldDropdown([["0", "0"], ["1", "1"]]), "valueOfBlue");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(180);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['setseparateledofrgbledtocolorintensity'] = {
    init: function () {
        this.appendValueInput("variableRgbLed")
            .setCheck("RGBLED")
            .appendField("set RGBLED on var.:");
        this.appendDummyInput()
            .appendField("color")
            .appendField(new Blockly.FieldDropdown([["red", "red"], ["green", "green"], ["blue", "blue"]]), "colorChooser");
        this.appendDummyInput()
            .appendField("intensity to")
            .appendField(new Blockly.FieldNumber(0, 0, 1, 0.01), "intensity");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(180);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['operationofrobot'] = {
    init: function () {
        this.appendValueInput("robotVariable")
            .setCheck("Robot")
            .appendField("Robot:");
        this.appendDummyInput()
            .appendField(", operation:")
            .appendField(new Blockly.FieldDropdown([["forward", "forward()"], ["backward", "backward()"], ["left", "left()"], ["right", "right()"], ["stop", "stop()"]]), "operation");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(180);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['operationofmotor'] = {
    init: function () {
        this.appendValueInput("motorVariable")
            .setCheck("Motor")
            .appendField("Motor:");
        this.appendDummyInput()
            .appendField(", operation:")
            .appendField(new Blockly.FieldDropdown([["forward", "forward()"], ["backward", "backward()"], ["stop", "stop()"]]), "operation");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(180);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['setbasicoutputdevicestate'] = {
    init: function () {
        this.appendValueInput("outputDevice")
            .setCheck(null)
            .appendField("set output device on var");
        this.appendDummyInput()
            .appendField("to")
            .appendField(new Blockly.FieldDropdown([["on", "on"], ["off", "off"]]), "state");
        this.setInputsInline(false);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(180);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};