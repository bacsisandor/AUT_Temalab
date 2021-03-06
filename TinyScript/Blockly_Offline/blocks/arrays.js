Blockly.Blocks['create_array'] = {
    init: function () {
        this.appendDummyInput()
        .appendField("Create array");
        this.appendDummyInput()
        .appendField("type")
        .appendField(new Blockly.FieldDropdown([["integer", "int"], ["boolean", "bool"], ["string", "string"]]), "type")
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
    init: function () {
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

Blockly.Blocks['get_array'] = {
    init: function () {
        this.appendDummyInput()
        .appendField("Get")
        .appendField(new Blockly.FieldVariable("item"), "variable");
        this.appendValueInput("index")
        .setCheck("Number")
        .appendField("index");
        this.setInputsInline(true);
        this.setOutput(true, null);
        this.setColour(65);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.Blocks['init_array'] = {
    init: function () {
        this.itemCount_ = 3;
        this.updateShape_();
        this.setMutator(new Blockly.Mutator(['init_array_item']));
        this.setTooltip("");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(65);
    },

    mutationToDom: function () {
        var container = document.createElement('mutation');
        container.setAttribute('items', this.itemCount_);
        return container;
    },
    domToMutation: function (xmlElement) {
        this.itemCount_ = parseInt(xmlElement.getAttribute('items'), 10);
        this.updateShape_();
    },
    decompose: function (workspace) {
        var containerBlock = workspace.newBlock('init_array_container');
        containerBlock.initSvg();
        var connection = containerBlock.getInput('STACK').connection;
        for (var i = 0; i < this.itemCount_; i++) {
            var itemBlock = workspace.newBlock('init_array_item');
            itemBlock.initSvg();
            connection.connect(itemBlock.previousConnection);
            connection = itemBlock.nextConnection;
        }
        return containerBlock;
    },
    compose: function (containerBlock) {
        var itemBlock = containerBlock.getInputTargetBlock('STACK');
        var connections = [];
        while (itemBlock) {
            connections.push(itemBlock.valueConnection_);
            itemBlock = itemBlock.nextConnection &&
                itemBlock.nextConnection.targetBlock();
        }
        for (var i = 0; i < this.itemCount_; i++) {
            var connection = this.getInput('ADD' + i).connection.targetConnection;
            if (connection && connections.indexOf(connection) == -1) {
                connection.disconnect();
            }
        }
        this.itemCount_ = connections.length;
        this.updateShape_();
        for (var i = 0; i < this.itemCount_; i++) {
            Blockly.Mutator.reconnect(connections[i], this, 'ADD' + i);
        }
    },
    saveConnections: function (containerBlock) {
        var itemBlock = containerBlock.getInputTargetBlock('STACK');
        var i = 0;
        while (itemBlock) {
            var input = this.getInput('ADD' + i);
            itemBlock.valueConnection_ = input && input.connection.targetConnection;
            i++;
            itemBlock = itemBlock.nextConnection &&
                itemBlock.nextConnection.targetBlock();
        }
    },
    updateShape_: function () {
        if (this.itemCount_ && this.getInput('EMPTY')) {
            this.removeInput('EMPTY');
        } else if (!this.itemCount_ && !this.getInput('EMPTY')) {
            this.appendDummyInput('EMPTY')
            .appendField(Blockly.Msg.LISTS_CREATE_EMPTY_TITLE);
        }
        for (var i = 0; i < this.itemCount_; i++) {
            if (!this.getInput('ADD' + i)) {
                var input = this.appendValueInput('ADD' + i);
                if (i == 0) {
                    input.appendField("Create array");
                    input.appendField(new Blockly.FieldDropdown([["integer", "int"], ["boolean", "bool"], ["string", "string"]]), "type")
                    input.appendField(new Blockly.FieldVariable("myArray"), "name");
                }
            }
        }
        while (this.getInput('ADD' + i)) {
            this.removeInput('ADD' + i);
            i++;
        }
    }
};

Blockly.Blocks['init_array_container'] = {
    init: function () {
        this.setColour(Blockly.Blocks.lists.HUE);
        this.appendDummyInput()
        .appendField(Blockly.Msg.LISTS_CREATE_WITH_CONTAINER_TITLE_ADD);
        this.appendStatementInput('STACK');
        this.setTooltip(Blockly.Msg.LISTS_CREATE_WITH_CONTAINER_TOOLTIP);
        this.contextMenu = false;
    }
};

Blockly.Blocks['init_array_item'] = {
    init: function () {
        this.setColour(Blockly.Blocks.lists.HUE);
        this.appendDummyInput()
        .appendField(Blockly.Msg.LISTS_CREATE_WITH_ITEM_TITLE);
        this.setPreviousStatement(true);
        this.setNextStatement(true);
        this.setTooltip(Blockly.Msg.LISTS_CREATE_WITH_ITEM_TOOLTIP);
        this.contextMenu = false;
    }
};
