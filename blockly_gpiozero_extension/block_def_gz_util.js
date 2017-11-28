Blockly.Blocks['gpioportnumber'] = {
    init: function () {
        this.appendDummyInput()
            .appendField(new Blockly.FieldDropdown([["GPIO2 (SDA I2C)", "2"], ["GPIO3 (SCL I2C)", "3"], ["GPIO4 ", "4"], ["GPIO17", "17"], ["GPIO27", "27"], ["GPIO22", "22"], ["GPIO10 (SPI MOSI)", "10"], ["GPIO9 (SPI MISO)", "9"], ["GPIO11 (SPI SCLK)", "11"], ["ID SD (I2C)", "OPTIONNAME"], ["ID SC (I2C ID)", "OPTIONNAME"], ["GPIO5", "5"], ["GPIO6", "6"], ["GPIO13", "13"], ["GPIO19", "19"], ["GPIO26", "26"], ["----", "----"], ["GPIO14 (UART0 TXD)", "14"], ["GPIO15 (UART0 RXD)", "15"], ["GPIO18", "18"], ["GPIO23", "23"], ["GPIO24", "24"], ["GPIO25", "25"], ["GPIO8 (SPI CE0)", "8"], ["GPIO7 (SPI CE1)", "7"], ["GPIO12", "12"], ["GPIO16", "16"], ["GPIO20", "20"], ["GPIO21", "21"]]), "PORT");
        this.setOutput(true, "Number");
        this.setColour(0);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['pause'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("pause");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(0);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['sleep'] = {
    init: function () {
        this.appendDummyInput()
            .appendField("sleep")
            .appendField(new Blockly.FieldNumber(0, 0, 10000), "sleepTime")
            .appendField("ms");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(0);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['sleepvariable'] = {
    init: function () {
        this.appendValueInput("timeToSleep")
            .setCheck("Number")
            .appendField("time to sleep");
        this.setInputsInline(true);
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(0);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};