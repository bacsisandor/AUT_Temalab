Blockly.Python['gpioportnumber'] = function (block) {
    var dropdown_port = block.getFieldValue('PORT');
    var code = dropdown_port;
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initled'] = function (block) {
    var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
    var code = 'LED' + value_gpioportnumber;
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initpwmled'] = function (block) {
    var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
    var code = 'PWMLED' + value_gpioportnumber;
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initbutton'] = function (block) {
    var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
    var code = 'Button' + value_gpioportnumber;
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initmotionsensor'] = function (block) {
    var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
    var code = 'MotionSensor' + value_gpioportnumber;
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initdistancesensorbasic'] = function (block) {
    var value_port1 = Blockly.Python.valueToCode(block, 'port1', Blockly.Python.ORDER_ATOMIC);
    var value_port2 = Blockly.Python.valueToCode(block, 'port2', Blockly.Python.ORDER_ATOMIC);
    value_port1 = value_port1.substring(1, value_port1.length - 1);
    value_port2 = value_port2.substring(1, value_port2.length - 1);
    var code = 'DistanceSensor(' + value_port1 + ',' + value_port2 + ')';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initdistancesensor'] = function (block) {
    var value_port1 = Blockly.Python.valueToCode(block, 'port1', Blockly.Python.ORDER_ATOMIC);
    var value_port2 = Blockly.Python.valueToCode(block, 'port2', Blockly.Python.ORDER_ATOMIC);
    var number_max_distance = block.getFieldValue('max_distance');
    var number_threshold_distance = block.getFieldValue('threshold_distance');
    value_port1 = value_port1.substring(1, value_port1.length - 1);
    value_port2 = value_port2.substring(1, value_port2.length - 1);
    var code = 'DistanceSensor(' + value_port1 + ', ' + value_port2
        + ', max_distance=' + number_max_distance
        + ', threshold_distance=' + number_threshold_distance + ')';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initmotor'] = function (block) {
    var value_motorforwardport = Blockly.Python.valueToCode(block, 'motorForwardPort', Blockly.Python.ORDER_ATOMIC);
    var value_motorbackwardport = Blockly.Python.valueToCode(block, 'motorBackwardPort', Blockly.Python.ORDER_ATOMIC);
    value_motorforwardport = value_motorforwardport.substring(1, value_motorforwardport.length - 1);
    value_motorbackwardport = value_motorbackwardport.substring(1, value_motorbackwardport.length - 1);
    var code = 'Motor(forward=' + value_motorforwardport + ', backward=' + value_motorbackwardport + ')';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initrobot'] = function (block) {
    var value_robotleftforwardport = Blockly.Python.valueToCode(block, 'robotLeftForwardPort', Blockly.Python.ORDER_ATOMIC);
    var value_robotleftbackwardport = Blockly.Python.valueToCode(block, 'robotLeftBackwardPort', Blockly.Python.ORDER_ATOMIC);
    var value_robotrightforwardport = Blockly.Python.valueToCode(block, 'robotRightForwardPort', Blockly.Python.ORDER_ATOMIC);
    var value_robotrightbackwardport = Blockly.Python.valueToCode(block, 'robotRightBackwardPort', Blockly.Python.ORDER_ATOMIC);
    value_robotleftforwardport = value_robotleftforwardport.substring(1, value_robotleftforwardport.length - 1);
    value_robotleftbackwardport = value_robotleftbackwardport.substring(1, value_robotleftbackwardport.length - 1);
    value_robotrightforwardport = value_robotrightforwardport.substring(1, value_robotrightforwardport.length - 1);
    value_robotrightbackwardport = value_robotrightbackwardport.substring(1, value_robotrightbackwardport.length - 1);
    var code = 'Robot(left=(' + value_robotleftforwardport + ', ' + value_robotleftbackwardport
        + '), right=(' + value_robotrightforwardport + ', ' + value_robotrightbackwardport + '))';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initlightsensor'] = function (block) {
    var value_name = Blockly.Python.valueToCode(block, 'NAME', Blockly.Python.ORDER_ATOMIC);
    var code = 'LightSensor' + value_name;
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initrgbled'] = function (block) {
    var value_gpioportnumberred = Blockly.Python.valueToCode(block, 'gpioPortNumberRed', Blockly.Python.ORDER_ATOMIC);
    var value_gpioportnumbergreen = Blockly.Python.valueToCode(block, 'gpioPortNumberGreen', Blockly.Python.ORDER_ATOMIC);
    var value_gpioportnumberblue = Blockly.Python.valueToCode(block, 'gpioPortNumberBlue', Blockly.Python.ORDER_ATOMIC);
    value_gpioportnumberred = value_gpioportnumberred.substring(1, value_gpioportnumberred.length - 1);
    value_gpioportnumbergreen = value_gpioportnumbergreen.substring(1, value_gpioportnumbergreen.length - 1);
    value_gpioportnumberblue = value_gpioportnumberblue.substring(1, value_gpioportnumberblue.length - 1);

    var code = 'RGBLED(red=' + value_gpioportnumberred
        + ', green=' + value_gpioportnumbergreen
        + ', blue=' + value_gpioportnumberblue + ')';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['setled'] = function (block) {
    var value_variable = Blockly.Python.valueToCode(block, 'variable', Blockly.Python.ORDER_ATOMIC);
    var dropdown_whattodo = block.getFieldValue('whatToDo');
    dropdown_whattodo = '.' + dropdown_whattodo;
    return value_variable + dropdown_whattodo + '\n';
};

Blockly.Python['setpwmled'] = function (block) {
    var value_variable = Blockly.Python.valueToCode(block, 'variable', Blockly.Python.ORDER_ATOMIC);
    var number_pwmvalue = '.value = ' + block.getFieldValue('pwmValue');
    return value_variable + number_pwmvalue + '\n';
};

Blockly.Python['setrgbledcolor'] = function (block) {
    var value_name = Blockly.Python.valueToCode(block, 'NAME', Blockly.Python.ORDER_ATOMIC) + '.color = (';
    var dropdown_valueofred = block.getFieldValue('valueOfRed');
    var dropdown_valueofgreen = block.getFieldValue('valueOfGreen');
    var dropdown_valueofblue = block.getFieldValue('valueOfBlue');

    return value_name
        + dropdown_valueofred
        + ', ' + dropdown_valueofgreen
        + ', ' + dropdown_valueofblue + ')\n';
};

Blockly.Python['setseparateledofrgbledtocolorintensity'] = function (block) {
    var value_variablergbled = Blockly.Python.valueToCode(block, 'variableRgbLed', Blockly.Python.ORDER_ATOMIC);
    var dropdown_colorchooser = block.getFieldValue('colorChooser');
    var number_intensity = block.getFieldValue('intensity');
    return value_variablergbled + '.' + dropdown_colorchooser + ' = ' + number_intensity+ '\n';
};

Blockly.Python['isbuttonpressed'] = function (block) {
    var value_button = Blockly.Python.valueToCode(block, 'button', Blockly.Python.ORDER_ATOMIC);
    var code = value_button + '.is_pressed';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['istheremotion'] = function (block) {
    var value_motionsensor = Blockly.Python.valueToCode(block, 'motionSensor', Blockly.Python.ORDER_ATOMIC);
    var code = value_motionsensor + '.when_motion';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['waitforbuttonpressed'] = function (block) {
    var value_buttonvariable = Blockly.Python.valueToCode(block, 'buttonVariable', Blockly.Python.ORDER_ATOMIC);
    return value_buttonvariable + '.wait_for_press()\n';
};

Blockly.Python['waitforlight'] = function (block) {
    var value_lightsensor = Blockly.Python.valueToCode(block, 'lightSensor', Blockly.Python.ORDER_ATOMIC);
    return value_lightsensor + '.wait_for_light()\n';
};

Blockly.Python['waitfordark'] = function (block) {
    var value_lightsensor = Blockly.Python.valueToCode(block, 'lightSensor', Blockly.Python.ORDER_ATOMIC);
    return value_lightsensor + '.wait_for_dark()\n';
};

Blockly.Python['lightsensorvalue'] = function (block) {
    var value_lightsensor = Blockly.Python.valueToCode(block, 'lightSensor', Blockly.Python.ORDER_ATOMIC);
    var code = value_lightsensor + '.values';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['distancesensorvalue'] = function (block) {
    var value_distancesensor = Blockly.Python.valueToCode(block, 'distanceSensor', Blockly.Python.ORDER_ATOMIC);
    var code = value_distancesensor + '.distance';
    return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['operationofrobot'] = function (block) {
    var value_robotvariable = Blockly.Python.valueToCode(block, 'robotVariable', Blockly.Python.ORDER_ATOMIC);
    var dropdown_operation = block.getFieldValue('operation');
    return value_robotvariable + '.' + dropdown_operation + '\n';
};

Blockly.Python['operationofmotor'] = function (block) {
    var value_motorvariable = Blockly.Python.valueToCode(block, 'motorVariable', Blockly.Python.ORDER_ATOMIC);
    var dropdown_operation = block.getFieldValue('operation');
    return value_motorvariable + '.' + dropdown_operation + '\n';
};

Blockly.Python['sleep'] = function (block) {
    var number_sleeptime = block.getFieldValue('sleepTime');
    return 'sleep(' + number_sleeptime + ')\n';
};

Blockly.Python['sleepvariable'] = function(block) {
  var value_timetosleep = Blockly.Python.valueToCode(block, 'timeToSleep', Blockly.Python.ORDER_ATOMIC);
  var code = 'sleep(' + value_timetosleep + ')\n';
  return code;
};