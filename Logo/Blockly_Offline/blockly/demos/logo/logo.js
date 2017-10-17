Blockly.Blocks['pen_up'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Pen up");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(330);
 this.setTooltip("Lifts the pen up, so the turtle doesn't draw a line when moving.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['pen_down'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Pen down");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(330);
 this.setTooltip("Puts the pen down, so the turtle draws a line when moving.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['move_forward'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Forward");
    this.appendValueInput("forward_pixels")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(20);
 this.setTooltip("Move forward x pixels.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['move_backward'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Backward");
    this.appendValueInput("backward_pixels")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(20);
 this.setTooltip("Move backward x pixels.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['turn_left'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Turn left");
    this.appendValueInput("turn_left")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(20);
 this.setTooltip("Rotate the turtle x degrees left.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['turn_right'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Turn right");
    this.appendValueInput("turn_right")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(20);
 this.setTooltip("Rotate the turtle x degrees right.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['repeat'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Repeat");
    this.appendValueInput("repeat_number")
        .setCheck("Number");
    this.appendDummyInput()
        .appendField("times");
    this.appendStatementInput("repeat")
        .setCheck(null);
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(210);
 this.setTooltip("Repeats the listed instuctions n times.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['home'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Home");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("Moves the turtle to center, pointing upwards.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['clean'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Clean");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(345);
 this.setTooltip("Clean the drawing area.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['set_xy'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Set x");
    this.appendValueInput("x_position")
        .setCheck("Number");
    this.appendDummyInput()
        .appendField(", set y");
    this.appendValueInput("y_position")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("Move turtle to the specified location.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['set_x'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Set x");
    this.appendValueInput("x_position")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("Move turtle to the specified location.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['set_y'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Set y");
    this.appendValueInput("y_position")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("Move turtle to the specified location.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['set_heading'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Set heading");
    this.appendValueInput("head_position")
        .setCheck("Number");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(180);
 this.setTooltip("Rotate turtle to the specified heading.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['clear_screen'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Clear screen");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(345);
 this.setTooltip("Same as clear and home together.");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['operation'] = {
  init: function() {
    this.appendValueInput("left_value")
        .setCheck("Number");
    this.appendValueInput("right_value")
        .setCheck("Number")
        .appendField(new Blockly.FieldDropdown([["+","add"], ["-","substract"], ["*","multiply"], ["/","divide"]]), "op");
    this.setInputsInline(true);
    this.setOutput(true, null);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};