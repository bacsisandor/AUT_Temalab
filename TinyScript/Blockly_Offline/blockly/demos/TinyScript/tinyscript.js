Blockly.Blocks['while'] = {
  init: function() {
    this.appendValueInput("condition")
        .setCheck("Boolean")
        .appendField("Repeat While");
    this.appendStatementInput("statements")
        .setCheck(null);
    this.setInputsInline(false);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(120);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['do_while'] = {
  init: function() {
    this.appendStatementInput("statements")
        .setCheck(null)
        .appendField("Do");
    this.appendValueInput("condition")
        .setCheck("Boolean")
        .appendField("While");
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(120);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['create_var'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Create variable")
        .appendField(new Blockly.FieldVariable("myVariable"), "var");
    this.appendValueInput("NAME")
        .setCheck(["Boolean", "Number", "String"])
        .appendField("Init value");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(330);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['create_variabletype'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Create variable")
        .appendField(new Blockly.FieldDropdown([["Integer","int"], ["Boolean","bool"], ["String","string"]]), "type")
        .appendField(new Blockly.FieldVariable("myVariable"), "var");
    this.appendValueInput("NAME")
        .setCheck(["Boolean", "Number", "String"])
        .appendField("Init value");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(330);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['abs'] = {
  init: function() {
    this.appendValueInput("NAME")
        .setCheck("Number")
        .appendField("Absolute value of");
    this.setOutput(true, null);
    this.setColour(230);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['create_array'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Create array");
    this.appendDummyInput()
        .appendField("type")
        .appendField(new Blockly.FieldDropdown([["integer","int"], ["boolean","bool"], ["string","string"]]), "type")
        .appendField(new Blockly.FieldVariable("myArray"), "name");
    this.appendValueInput("size")
        .setCheck("Number")
        .appendField("Size");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(65);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};

Blockly.Blocks['set_array'] = {
  init: function() {
    this.appendDummyInput()
        .appendField("Set")
        .appendField(new Blockly.FieldVariable("item"), "item");
    this.appendValueInput("index")
        .setCheck("Number")
        .appendField("index");
    this.appendValueInput("value")
        .setCheck(["Boolean", "Number", "String"])
        .appendField("to");
    this.setInputsInline(true);
    this.setPreviousStatement(true, null);
    this.setNextStatement(true, null);
    this.setColour(65);
 this.setTooltip("");
 this.setHelpUrl("");
  }
};