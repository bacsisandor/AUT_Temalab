Blockly.Blocks['gpioportnumber'] = {
  init: function() {
    this.appendDummyInput()
        .appendField(new Blockly.FieldDropdown([["GPIO2 (SDA I2C)","2"], ["GPIO3 (SCL I2C)","3"], ["GPIO4 ","4"], ["GPIO17","17"], ["GPIO27","27"], ["GPIO22","22"], ["GPIO10 (SPI MOSI)","10"], ["GPIO9 (SPI MISO)","9"], ["GPIO11 (SPI SCLK)","11"], ["ID SD (I2C)","OPTIONNAME"], ["ID SC (I2C ID)","OPTIONNAME"], ["GPIO5","5"], ["GPIO6","6"], ["GPIO13","13"], ["GPIO19","19"], ["GPIO26","26"], ["----","----"], ["GPIO14 (UART0 TXD)","14"], ["GPIO15 (UART0 RXD)","15"], ["GPIO18","18"], ["GPIO23","23"], ["GPIO24","24"], ["GPIO25","25"], ["GPIO8 (SPI CE0)","8"], ["GPIO7 (SPI CE1)","7"], ["GPIO12","12"], ["GPIO16","16"], ["GPIO20","20"], ["GPIO21","21"]]), "PORT");
    this.setOutput(true, "Number");
    this.setColour(0);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initled'] = {
  init: function() {
    this.appendValueInput("gpioPortNumber")
        .setCheck("Number")
        .appendField("init led on port");
    this.setInputsInline(true);
    this.setOutput(true, "LED");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initpwmled'] = {
  init: function() {
    this.appendValueInput("gpioPortNumber")
        .setCheck("Number")
        .appendField("init PWM led on port");
    this.setInputsInline(true);
    this.setOutput(true, "PWMLED");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initbutton'] = {
  init: function() {
    this.appendValueInput("gpioPortNumber")
        .setCheck("Number")
        .appendField("init button on port");
    this.setInputsInline(true);
    this.setOutput(true, "Button");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initmotionsensor'] = {
  init: function() {
    this.appendValueInput("gpioPortNumber")
        .setCheck("Number")
        .appendField("init motion sensor on port");
    this.setInputsInline(true);
    this.setOutput(true, "MotionSensor");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initdistancesensorbasic'] = {
  init: function() {
    this.appendValueInput("port1")
        .setCheck("Number")
        .appendField("init distance sensor: port 1 is");
    this.appendValueInput("port2")
        .setCheck("Number")
        .appendField("and port 2 is");
    this.setInputsInline(true);
    this.setOutput(true, "DistanceSensor");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initdistancesensor'] = {
  init: function() {
    this.appendValueInput("port1")
        .setCheck("Number")
        .appendField("init distance sensor: port 1 is");
    this.appendValueInput("port2")
        .setCheck("Number")
        .appendField("and port 2 is");
    this.appendDummyInput()
        .appendField(",max distance (default 1):")
        .appendField(new Blockly.FieldNumber(1), "max_distance");
    this.appendDummyInput()
        .appendField("threshold distance (default 0.2):")
        .appendField(new Blockly.FieldNumber(0.2), "threshold_distance");
    this.setInputsInline(true);
    this.setOutput(true, "DistanceSensor");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initmotor'] = {
  init: function() {
    this.appendValueInput("motorForwardPort")
        .setCheck("Number")
        .appendField("init motor: forward port");
    this.appendValueInput("motorBackwardPort")
        .setCheck("Number")
        .appendField(", backward port");
    this.setInputsInline(true);
    this.setOutput(true, "Motor");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initrobot'] = {
  init: function() {
    this.appendValueInput("robotLeftForwardPort")
        .setCheck("Number")
        .appendField("init robot: left forward port:");
    this.appendValueInput("robotLeftBackwardPort")
        .setCheck("Number")
        .appendField(", left backward port:");
    this.appendValueInput("robotRightForwardPort")
        .setCheck("Number")
        .appendField(", right forward port:");
    this.appendValueInput("robotRightBackwardPort")
        .setCheck("Number")
        .appendField(", right backward port:");
    this.setInputsInline(true);
    this.setOutput(true, "Robot");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initlightsensor'] = {
  init: function() {
    this.appendValueInput("NAME")
        .setCheck("Number")
        .appendField("init Light sensor on port");
    this.setInputsInline(true);
    this.setOutput(true, "LightSensor");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['initrgbled'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("init RGBLED on ports:");
    this.appendValueInput("gpioPortNumberRed")
        .setCheck("Number")
        .appendField("red");
    this.appendValueInput("gpioPortNumberGreen")
        .setCheck("Number")
        .appendField("green");
    this.appendValueInput("gpioPortNumberBlue")
        .setCheck("Number")
        .appendField("blue");
    this.setInputsInline(true);
    this.setOutput(true, "RGBLED");
    this.setColour(315);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['setled'] = {
  init: function() {
    this.appendValueInput("variable")
        .setCheck("LED")
        .appendField("set led variable");
    this.appendDummyInput()
        .appendField("to")
        .appendField(new Blockly.FieldDropdown([["ON","on()"], ["OFF","off()"], ["BLINK 1 SEC","blink()"]]), "whatToDo");
    this.setInputsInline(false);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['setpwmled'] = {
  init: function() {
    this.appendValueInput("variable")
        .setCheck("PWMLED")
        .appendField("set pwm led on variable");
    this.appendDummyInput()
        .appendField("to")
        .appendField(new Blockly.FieldNumber(0, 0, 1, 0.1), "pwmValue");
    this.setInputsInline(false);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['setrgbledcolor'] = {
  init: function() {
    this.appendValueInput("NAME")
        .setCheck("RGBLED")
        .appendField("set rgb led on variable:");
    this.appendDummyInput()
        .appendField("to red")
        .appendField(new Blockly.FieldDropdown([["0","0"], ["1","1"]]), "valueOfRed");
    this.appendDummyInput()
        .appendField("green")
        .appendField(new Blockly.FieldDropdown([["0","0"], ["1","1"]]), "valueOfGreen");
    this.appendDummyInput()
        .appendField("blue")
        .appendField(new Blockly.FieldDropdown([["0","0"], ["1","1"]]), "valueOfBlue");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['setseparateledofrgbledtocolorintensity'] = {
  init: function() {
    this.appendValueInput("variableRgbLed")
        .setCheck("RGBLED")
        .appendField("set rgb led variable");
    this.appendDummyInput()
        .appendField("'s color")
        .appendField(new Blockly.FieldDropdown([["red","red"], ["green","green"], ["blue","blue"]]), "colorChooser");
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

Blockly.Blocks['isbuttonpressed'] = {
  init: function() {
    this.appendValueInput("button")
        .setCheck("Button")
        .appendField("is button pressed on variable");
    this.setInputsInline(true);
    this.setOutput(true, "Boolean");
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['istheremotion'] = {
  init: function() {
    this.appendValueInput("motionSensor")
        .setCheck("MotionSensor")
        .appendField("is there motion on variable");
    this.setInputsInline(true);
    this.setOutput(true, "Boolean");
    this.setColour(45);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['waitforbuttonpressed'] = {
  init: function() {
    this.appendValueInput("buttonVariable")
        .setCheck("Button")
        .appendField("wait for press on button variable:");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['waitforlight'] = {
  init: function() {
    this.appendValueInput("lightSensor")
        .setCheck("LightSensor")
        .appendField("wait for light on light sensor variable:");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['waitfordark'] = {
  init: function() {
    this.appendValueInput("lightSensor")
        .setCheck("LightSensor")
        .appendField("wait for dark on light sensor variable");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['lightsensorvalue'] = {
  init: function() {
    this.appendValueInput("lightSensor")
        .setCheck("LightSensor")
        .appendField("light sensor value on light sensor variable:");
    this.setInputsInline(true);
    this.setOutput(true, "Number");
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['distancesensorvalue'] = {
  init: function() {
    this.appendValueInput("distanceSensor")
        .setCheck("DistanceSensor")
        .appendField("distance sensor measured distance on distance sensor variable:");
    this.setInputsInline(true);
    this.setOutput(true, null);
    this.setColour(180);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['operationofrobot'] = {
  init: function() {
    this.appendValueInput("robotVariable")
        .setCheck("Robot")
        .appendField("robot on variable");
    this.appendDummyInput()
        .appendField(", operation:")
        .appendField(new Blockly.FieldDropdown([["forward","forward()"], ["backward","backward()"], ["left","left()"], ["right","right()"], ["stop","stop()"]]), "operation");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['operationofmotor'] = {
  init: function() {
    this.appendValueInput("motorVariable")
        .setCheck("Motor")
        .appendField("motor on port:");
    this.appendDummyInput()
        .appendField(", operation:")
        .appendField(new Blockly.FieldDropdown([["forward","forward()"], ["backward","backward()"], ["stop","stop()"]]), "operation");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['sleep'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("sleep")
        .appendField(new Blockly.FieldNumber(0, 0, 10000), "sleepTime")
        .appendField("ms");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};