Blockly.Python['gpioportnumber'] = function(block) {
  var dropdown_port = block.getFieldValue('PORT');
  // TODO: Assemble Python into code variable.
  var code = dropdown_port;
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initled'] = function(block) {
  var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = 'LED' + value_gpioportnumber;
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initpwmled'] = function(block) {
  var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = 'PWMLED' + value_gpioportnumber;
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initbutton'] = function(block) {
  var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = 'Button' + value_gpioportnumber;
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initmotionsensor'] = function(block) {
  var value_gpioportnumber = Blockly.Python.valueToCode(block, 'gpioPortNumber', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = 'MotionSensor' + value_gpioportnumber;
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initdistancesensorbasic'] = function(block) {
  var value_port1 = Blockly.Python.valueToCode(block, 'port1', Blockly.Python.ORDER_ATOMIC);
  var value_port2 = Blockly.Python.valueToCode(block, 'port2', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = 'DistanceSensor' + value_port1 + value_port2;
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initdistancesensor'] = function(block) {
  var value_port1 = Blockly.Python.valueToCode(block, 'port1', Blockly.Python.ORDER_ATOMIC);
  var value_port2 = Blockly.Python.valueToCode(block, 'port2', Blockly.Python.ORDER_ATOMIC);
  var number_max_distance = block.getFieldValue('max_distance');
  var number_threshold_distance = block.getFieldValue('threshold_distance');
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initmotor'] = function(block) {
  var value_motorforwardport = Blockly.Python.valueToCode(block, 'motorForwardPort', Blockly.Python.ORDER_ATOMIC);
  var value_motorbackwardport = Blockly.Python.valueToCode(block, 'motorBackwardPort', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initrobot'] = function(block) {
  var value_robotleftforwardport = Blockly.Python.valueToCode(block, 'robotLeftForwardPort', Blockly.Python.ORDER_ATOMIC);
  var value_robotleftbackwardport = Blockly.Python.valueToCode(block, 'robotLeftBackwardPort', Blockly.Python.ORDER_ATOMIC);
  var value_robotrightforwardport = Blockly.Python.valueToCode(block, 'robotRightForwardPort', Blockly.Python.ORDER_ATOMIC);
  var value_robotrightbackwardport = Blockly.Python.valueToCode(block, 'robotRightBackwardPort', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initlightsensor'] = function(block) {
  var value_name = Blockly.Python.valueToCode(block, 'NAME', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = 'LightSensor' + value_name;
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['initrgbled'] = function(block) {
  var value_gpioportnumberred = Blockly.Python.valueToCode(block, 'gpioPortNumberRed', Blockly.Python.ORDER_ATOMIC);
  var value_gpioportnumbergreen = Blockly.Python.valueToCode(block, 'gpioPortNumberGreen', Blockly.Python.ORDER_ATOMIC);
  var value_gpioportnumberblue = Blockly.Python.valueToCode(block, 'gpioPortNumberBlue', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['setled'] = function(block) {
  var value_variable = Blockly.Python.valueToCode(block, 'variable', Blockly.Python.ORDER_ATOMIC);
  var dropdown_whattodo = block.getFieldValue('whatToDo');
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['setpwmled'] = function(block) {
  var value_variable = Blockly.Python.valueToCode(block, 'variable', Blockly.Python.ORDER_ATOMIC);
  var number_pwmvalue = block.getFieldValue('pwmValue');
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['setrgbledcolor'] = function(block) {
  var value_name = Blockly.Python.valueToCode(block, 'NAME', Blockly.Python.ORDER_ATOMIC);
  var dropdown_valueofred = block.getFieldValue('valueOfRed');
  var dropdown_valueofgreen = block.getFieldValue('valueOfGreen');
  var dropdown_valueofblue = block.getFieldValue('valueOfBlue');
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['setseparateledofrgbledtocolorintensity'] = function(block) {
  var value_variablergbled = Blockly.Python.valueToCode(block, 'variableRgbLed', Blockly.Python.ORDER_ATOMIC);
  var dropdown_colorchooser = block.getFieldValue('colorChooser');
  var number_intensity = block.getFieldValue('intensity');
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['isbuttonpressed'] = function(block) {
  var value_button = Blockly.Python.valueToCode(block, 'button', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['istheremotion'] = function(block) {
  var value_motionsensor = Blockly.Python.valueToCode(block, 'motionSensor', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['istherelight'] = function(block) {
  var value_lightsensor = Blockly.Python.valueToCode(block, 'lightSensor', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['waitforbuttonpressed'] = function(block) {
  var value_buttonvariable = Blockly.Python.valueToCode(block, 'buttonVariable', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['waitforlight'] = function(block) {
  var value_lightsensor = Blockly.Python.valueToCode(block, 'lightSensor', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['waitfordark'] = function(block) {
  var value_lightsensor = Blockly.Python.valueToCode(block, 'lightSensor', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['lightsensorvalue'] = function(block) {
  var value_lightsensor = Blockly.Python.valueToCode(block, 'lightSensor', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['distancesensorvalue'] = function(block) {
  var value_distancesensor = Blockly.Python.valueToCode(block, 'distanceSensor', Blockly.Python.ORDER_ATOMIC);
  // TODO: Assemble Python into code variable.
  var code = '...';
  // TODO: Change ORDER_NONE to the correct strength.
  return [code, Blockly.Python.ORDER_NONE];
};

Blockly.Python['operationofrobot'] = function(block) {
  var value_robotvariable = Blockly.Python.valueToCode(block, 'robotVariable', Blockly.Python.ORDER_ATOMIC);
  var dropdown_operation = block.getFieldValue('operation');
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['operationofmotor'] = function(block) {
  var value_motorvariable = Blockly.Python.valueToCode(block, 'motorVariable', Blockly.Python.ORDER_ATOMIC);
  var dropdown_operation = block.getFieldValue('operation');
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};

Blockly.Python['sleep'] = function(block) {
  var number_sleeptime = block.getFieldValue('sleepTime');
  // TODO: Assemble Python into code variable.
  var code = '...\n';
  return code;
};