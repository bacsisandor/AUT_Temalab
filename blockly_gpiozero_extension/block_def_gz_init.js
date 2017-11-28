Blockly.Blocks['initrobotvacuumcleaner'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Robot Vacuum cleaner");
        this.appendValueInput("drive")
            .setCheck(null)
            .appendField("Drive Robot");
        this.appendValueInput("cleaner")
            .setCheck(null)
            .appendField("Cleaner Motor");
        this.appendValueInput("distSensorForward")
            .setCheck(null)
            .appendField("Distance Sensor forward");
        this.setOutput(true, null);
        this.setColour(30);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initrobberdetector'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("init Robboer motion detector");
        this.appendValueInput("portOfBuzzer")
            .setCheck(null)
            .appendField("Buzzer port:");
        this.appendValueInput("portOfMotionSensor")
            .setCheck(null)
            .appendField("Motion sensor:");
        this.setOutput(true, null);
        this.setColour(30);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initled'] = {
    init: function () {
        this.appendValueInput("gpioPortNumber")
            .setCheck("Number")
            .appendField("init Led");
        this.setInputsInline(true);
        this.setOutput(true, "LED");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initpwmled'] = {
    init: function () {
        this.appendValueInput("gpioPortNumber")
            .setCheck("Number")
            .appendField("init PWMLED");
        this.setInputsInline(true);
        this.setOutput(true, "PWMLED");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initbutton'] = {
    init: function () {
        this.appendValueInput("gpioPortNumber")
            .setCheck("Number")
            .appendField("init Button");
        this.setInputsInline(true);
        this.setOutput(true, "Button");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initmotionsensor'] = {
    init: function () {
        this.appendValueInput("gpioPortNumber")
            .setCheck("Number")
            .appendField("init MotionSensor");
        this.setInputsInline(true);
        this.setOutput(true, "MotionSensor");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initdistancesensorbasic'] = {
    init: function () {
        this.appendValueInput("echo")
            .setCheck("Number")
            .appendField("init DistanceSensor port1:");
        this.appendValueInput("trigger")
            .setCheck("Number")
            .appendField("port2:");
        this.setInputsInline(true);
        this.setOutput(true, "DistanceSensor");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initdistancesensor'] = {
    init: function () {
        this.appendValueInput("echo")
            .setCheck("Number")
            .appendField("init DistanceSensor port1:");
        this.appendValueInput("trigger")
            .setCheck("Number")
            .appendField("port2:");
        this.appendDummyInput()
            .appendField(",max dist.:")
            .appendField(new Blockly.FieldNumber(1), "max_distance");
        this.appendDummyInput()
            .appendField("threshold dist.:")
            .appendField(new Blockly.FieldNumber(0.2), "threshold_distance");
        this.setInputsInline(true);
        this.setOutput(true, "DistanceSensor");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initmotor'] = {
    init: function () {
        this.appendValueInput("motorForwardPort")
            .setCheck("Number")
            .appendField("init Motor fw port:");
        this.appendValueInput("motorBackwardPort")
            .setCheck("Number")
            .appendField("bw port:");
        this.setInputsInline(true);
        this.setOutput(true, "Motor");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initrobot'] = {
    init: function () {
        this.appendValueInput("robotLeftForwardPort")
            .setCheck("Number")
            .appendField("init Robot: left fw port:");
        this.appendValueInput("robotLeftBackwardPort")
            .setCheck("Number")
            .appendField("left bw port:");
        this.appendValueInput("robotRightForwardPort")
            .setCheck("Number")
            .appendField("right fw port:");
        this.appendValueInput("robotRightBackwardPort")
            .setCheck("Number")
            .appendField("right bw port:");
        this.setInputsInline(true);
        this.setOutput(true, "Robot");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initlightsensor'] = {
    init: function () {
        this.appendValueInput("NAME")
            .setCheck("Number")
            .appendField("init LightSensor");
        this.setInputsInline(true);
        this.setOutput(true, "LightSensor");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initbasicoutputdevice'] = {
    init: function () {
        this.appendValueInput("pinNumber")
            .setCheck(null)
            .appendField("init basic output device, pin");
        this.appendDummyInput()
            .appendField("Active high")
            .appendField(new Blockly.FieldDropdown([["yes", "yes"], ["no", "no"]]), "activeHighValue");
        this.setInputsInline(false);
        this.setOutput(true, null);
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initcputemperature'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("init cpu temp meter on var");
        this.setOutput(true, null);
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initledbargraph'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("init LEDBarGraph");
        this.appendValueInput("p1")
            .setCheck("Number")
            .appendField("p1");
        this.appendValueInput("p2")
            .setCheck("Number")
            .appendField("p2");
        this.appendValueInput("p3")
            .setCheck("Number")
            .appendField("p3");
        this.appendValueInput("p4")
            .setCheck("Number")
            .appendField("p4");
        this.appendValueInput("p5")
            .setCheck("Number")
            .appendField("p5");
        this.appendDummyInput()
            .appendField("pwm")
            .appendField(new Blockly.FieldDropdown([["yes", "True"], ["no ", "False"]]), "pwmType");
        this.setInputsInline(false);
        this.setOutput(true, null);
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initrgbled'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("init RGBLED :");
        this.appendValueInput("gpioPortNumberRed")
            .setCheck("Number")
            .appendField("red:");
        this.appendValueInput("gpioPortNumberGreen")
            .setCheck("Number")
            .appendField("green:");
        this.appendValueInput("gpioPortNumberBlue")
            .setCheck("Number")
            .appendField("blue:");
        this.setInputsInline(true);
        this.setOutput(true, "RGBLED");
        this.setColour(90);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['initpetfeeder'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("Init pet feeder");
        this.appendValueInput("servo")
            .setCheck(null)
            .appendField("servo:");
        this.appendValueInput("lightsensor")
            .setCheck(null)
            .appendField("lightsensor:");
        this.setOutput(true, null);
        this.setColour(30);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};