Blockly.Logo['pen_up'] = function(block) {
  var code = 'penup\n';
  return code;
};

Blockly.Logo['pen_down'] = function(block) {
  var code = 'pendown\n';
  return code;
};

Blockly.Logo['move_forward'] = function(block) {
  var value_forward_pixels = Blockly.Logo.valueToCode(block, 'forward_pixels', Blockly.Logo.ORDER_ATOMIC);
  var code = 'forward ' + value_forward_pixels + '\n';
  return code;
};	

Blockly.Logo['move_backward'] = function(block) {
  var value_backward_pixels = Blockly.Logo.valueToCode(block, 'backward_pixels', Blockly.Logo.ORDER_ATOMIC);
  var code = 'back '+ value_backward_pixels + '\n';
  return code;
};

Blockly.Logo['turn_left'] = function(block) {
  var value_turn_left = Blockly.Logo.valueToCode(block, 'turn_left', Blockly.Logo.ORDER_ATOMIC);
  var code = 'left ' + value_turn_left + '\n';
  return code;
};

Blockly.Logo['turn_right'] = function(block) {
  var value_turn_right = Blockly.Logo.valueToCode(block, 'turn_right', Blockly.Logo.ORDER_ATOMIC);
	var code = 'right ' + value_turn_right + '\n';
  return code;
};

Blockly.Logo['repeat'] = function(block) {
  var number_repeat_number = block.getFieldValue('repeat_number');
  var statements_repeat = Blockly.Logo.statementToCode(block, 'repeat');
  var code = 'repeat ' + number_repeat_number + ' [\n' + statements_repeat + ']\n';
  return code;
};

Blockly.Logo['home'] = function(block) {
  var code = 'home ' + '\n';
  return code;
};

Blockly.Logo['clean'] = function(block) {
  var code = 'clean ' + '\n';
  return code;
};

Blockly.Logo['set_xy'] = function(block) {
  var value_x_position = Blockly.Logo.valueToCode(block, 'x_position', Blockly.Logo.ORDER_ATOMIC);
  var value_y_position = Blockly.Logo.valueToCode(block, 'y_position', Blockly.Logo.ORDER_ATOMIC);
  var code = 'setxy ' + value_x_position + ' ' + value_y_position + '\n';
  return code;
};

Blockly.Logo['set_x'] = function(block) {
  var value_x_position = Blockly.Logo.valueToCode(block, 'x_position', Blockly.Logo.ORDER_ATOMIC);
  var code = 'setx ' + value_x_position + '\n';
  return code;
};

Blockly.Logo['set_y'] = function(block) {
  var value_y_position = Blockly.Logo.valueToCode(block, 'y_position', Blockly.Logo.ORDER_ATOMIC);
  var code = 'sety ' + value_y_position + '\n';
  return code;
};

Blockly.Logo['set_heading'] = function(block) {
  var value_head_position = Blockly.Logo.valueToCode(block, 'head_position', Blockly.Logo.ORDER_ATOMIC);
  var code = 'setheading ' + value_head_position + '\n';
  return code;
};

Blockly.Logo['clear_screen'] = function(block) {
  var code = 'clearscreen '+ '\n';
  return code;
};

Blockly.Logo['operation'] = function(block) {
  var value_left_value = Blockly.Logo.valueToCode(block, 'left_value', Blockly.Logo.ORDER_ATOMIC);
  var dropdown_op = block.getFieldValue('op');
  var value_right_value = Blockly.Logo.valueToCode(block, 'right_value', Blockly.Logo.ORDER_ATOMIC);
  
  var code = value_left_value;
  switch(dropdown_op){
	  case 'add' : code = code + '+';break;
	  case 'substract' : code = code + '-'; break;
	  case 'multiply' : code = code + '*'; break;
	  case 'divide' : code = code + '/'; break;
  }
  code = code + value_right_value;
  return [code, Blockly.Logo.ORDER_NONE];
};

Blockly.Logo['math_number'] = function(block) {
  var code = parseInt(block.getFieldValue('NUM'));
  return [code, Blockly.Logo.ORDER_ATOMIC];
};